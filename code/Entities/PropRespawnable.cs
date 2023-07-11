using System;
using Editor;
using Sandbox.ModelEditor.Nodes;

[Library( "prop_respawnable" ), HammerEntity]
[CanBeClientsideOnly, Model, RenderFields, VisGroup( VisGroup.Physics )]
[Title( "Prop Respawnable" ), Category( "Gameplay" ), Icon( "chair" )]
partial class PropRespawnable : BasePhysics, IRespawnableEntity
{
	/// <summary>
	/// If set, the prop will spawn with motion disabled and will act as a navigation blocker until broken.
	/// </summary>
	[Property]
	public bool Static { get; set; } = false;

	/// <summary>
	/// Set during map compile for multi physics body models based on Hammer physics simulation tool.
	/// </summary>
	[Property( "boneTransforms" ), HideInEditor]
	private string BoneTransforms { get; set; }

	/// <summary>
	/// Multiplier for the object's mass.
	/// </summary>
	[Property( "massscale", Title = "Mass Scale" ), Category( "Physics" )]
	private float MassScale { get; set; } = 1.0f;

	/// <summary>
	/// Physics linear damping.
	/// </summary>
	[Property( "lineardamping", Title = "Linear Damping" ), Category( "Physics" )]
	private float LinearDamping { get; set; } = 0.0f;

	/// <summary>
	/// Physics angular damping.
	/// </summary>
	[Property( "angulardamping", Title = "Angular Damping" ), Category( "Physics" )]
	private float AngularDamping { get; set; } = 0.0f;

	public override void Spawn()
	{
		base.Spawn();

		PhysicsEnabled = true;
		UsePhysicsCollision = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		Tags.Add( "prop", "solid" );

		SetupPhysics();

		if ( Static )
		{
			PhysicsEnabled = false;
		}
	}

	private void SetupPhysics()
	{
		var physics = SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		if ( !physics.IsValid() )
			return;

		// Apply any saved bone transforms
		ApplyBoneTransforms();

		if ( MassScale != 1.0f )
		{
			physics.Mass *= MassScale;
		}

		physics.LinearDamping = LinearDamping;
		physics.AngularDamping = AngularDamping;
	}

	private void ApplyBoneTransforms()
	{
		if ( string.IsNullOrWhiteSpace( BoneTransforms ) )
			return;

		var bones = BoneTransforms.Split( ';', StringSplitOptions.RemoveEmptyEntries );
		foreach ( var bone in bones )
		{
			var split = bone.Split( ':', StringSplitOptions.TrimEntries );
			if ( split.Length != 2 )
				continue;

			var boneName = split[0];
			var boneTransform = Transform.Parse( split[1] );

			var body = GetBonePhysicsBody( GetBoneIndex( boneName ) );
			if ( body.IsValid() )
			{
				body.Transform = Transform.ToWorld( boneTransform );
			}
		}
	}

	public override void OnNewModel( Model model )
	{
		base.OnNewModel( model );

		// When a model is reloaded, all entities get set to NULL model first
		if ( model == null || model.IsError ) return;

		if ( Game.IsServer )
		{
			UpdatePropData( model );
		}
	}

	/// <summary>
	/// Called on new model to update the prop with <see cref="ModelPropData"/> data of the new model.
	/// </summary>
	protected virtual void UpdatePropData( Model model )
	{
		Game.AssertServer();

		if ( model.TryGetData( out ModelPropData propInfo ) )
		{
			Health = propInfo.Health;
		}

		//
		// If health is unset, set it to -1 - which means it cannot be destroyed
		//
		if ( Health <= 0 )
			Health = -1;
	}

	DamageInfo LastDamage;

	/// <summary>
	/// Fired when the entity gets damaged.
	/// </summary>
	protected Output OnDamaged { get; set; }

	/// <summary>
	/// This prop won't be able to be damaged for this amount of time
	/// </summary>
	public RealTimeUntil Invulnerable { get; set; }

	public override void TakeDamage( DamageInfo info )
	{
		if ( Invulnerable > 0 )
		{
			// We still want to apply forces
			ApplyDamageForces( info );

			return;
		}

		LastDamage = info;

		base.TakeDamage( info );

		// TODO: Add damage type as argument? Or should it be the new health value?
		OnDamaged.Fire( this );
	}

	public override void OnKilled()
	{
		if ( LifeState != LifeState.Alive )
			return;

		LifeState = LifeState.Dead;

		if ( LastDamage.HasTag( "physics_impact" ) )
		{
			Velocity = lastCollision.This.PreVelocity;
		}

		if ( HasExplosionBehavior() )
		{
			if ( LastDamage.HasTag( "blast" ) )
			{
				LifeState = LifeState.Dying;

				// Don't explode right away and cause a stack overflow
				var rand = new Random();
				_ = ExplodeAsync( Game.Random.Float( 0.05f, 0.25f ) );

				return;
			}
			else
			{
				DoGibs();
				DoExplosion();
				Delete(); // LifeState.Dead prevents this in OnKilled\
			}
		}
		else
		{
			DoGibs();
			Delete(); // LifeState.Dead prevents this in OnKilled
		}

		base.OnKilled();
	}

	CollisionEventData lastCollision;

	protected override void OnPhysicsCollision( CollisionEventData eventData )
	{
		lastCollision = eventData;

		base.OnPhysicsCollision( eventData );
	}

	private bool HasExplosionBehavior()
	{
		if ( Model == null || Model.IsError )
			return false;

		return Model.HasData<ModelExplosionBehavior>();
	}

	/// <summary>
	/// Fired when the entity gets destroyed.
	/// </summary>
	protected Output OnBreak { get; set; }

	private void DoGibs()
	{
		var result = new Breakables.Result();
		result.CopyParamsFrom( LastDamage );
		Breakables.Break( this, result );

		OnBreak.Fire( LastDamage.Attacker );
	}

	/// <summary>
	/// TODO: Internal, explodes and spawns gibs
	/// </summary>
	public async Task ExplodeAsync( float fTime )
	{
		if ( LifeState != LifeState.Alive && LifeState != LifeState.Dying )
			return;

		LifeState = LifeState.Dead;

		await Task.DelaySeconds( fTime );

		DoGibs();
		DoExplosion();

		Delete();
	}

	private void DoExplosion()
	{
		if ( Model == null || Model.IsError )
			return;

		if ( !Model.TryGetData( out ModelExplosionBehavior explosionBehavior ) )
			return;

		// Damage and push away all other entities
		var srcPos = Position;
		if ( PhysicsBody.IsValid() ) srcPos = PhysicsBody.MassCenter;
		if ( explosionBehavior.Radius > 0.0f )
		{
			new ExplosionEntity
			{
				Position = srcPos,
				Radius = explosionBehavior.Radius,
				Damage = explosionBehavior.Damage,
				ForceScale = explosionBehavior.Force,
				ParticleOverride = explosionBehavior.Effect,
				SoundOverride = explosionBehavior.Sound
			}.Explode( this );
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	#region Hammer Inputs

	/// <summary>
	/// Causes this prop to break, regardless if it is actually breakable or not. (i.e. ignores health and whether the model has gibs)
	/// </summary>
	[Input]
	public void Break()
	{
		OnKilled();
		LifeState = LifeState.Dead;
		Delete();
	}

	/// <summary>
	/// Deletes this prop.
	/// </summary>
	[Input( "Delete" )]
	protected void DeleteInput()
	{
		Delete();
	}

	/// <summary>
	/// Sets the scale of the prop, affecting physics and visual scale.
	/// </summary>
	[Input]
	protected void SetScale( float scale )
	{
		Scale = scale.Clamp( 0.1f, 100f );

		if ( PhysicsGroup != null )
		{
			// Set mass as well. Without this objects feel really floaty at large scales.
			PhysicsGroup.RebuildMass(); // First rebuild mass so we are at the default.
			PhysicsGroup.Mass *= scale.Clamp( 1f, 100f );

			// Wake up physics
			PhysicsGroup.Sleeping = false;
		}
	}

	/// <summary>
	/// Sets the material group of the props' model by name, as set in ModelDoc.
	/// </summary>
	[Input( "SetMaterialGroup" )]
	protected void SetMaterialGroupInput( string group )
	{
		SetMaterialGroup( group );
	}

	/// <summary>
	/// Sets the body group of the props' model by name, as set in ModelDoc.
	/// Format is "name,option"
	/// </summary>
	[Input( "SetBodyGroup" )]
	protected void SetBodyGroupInput( string group )
	{
		string[] opts = group.Split( new[] { ' ', ',' } );
		if ( opts.Length != 2 )
		{
			Log.Warning( $"Prop.SetBodyGroup was given invalid input \"{group}\", expceted \"name,option\"!" );
			return;
		}

		SetBodyGroup( opts[0], opts[1].ToInt() );
	}

	/// <summary>
	/// Enables or disables collisions on this prop.
	/// </summary>
	[Input]
	protected void SetCollisions( bool enabled )
	{
		EnableAllCollisions = enabled;
	}

	/// <summary>
	/// Enables or disables visibility of this prop.
	/// </summary>
	[Input]
	protected void SetVisible( bool enabled )
	{
		EnableDrawing = enabled;
	}

	/// <summary>
	/// Enables or disables physics (gravity) simulation of this prop.
	/// </summary>
	[Input]
	protected void SetStatic( bool enabled )
	{
		if ( enabled )
		{
			PhysicsEnabled = false;
		}
		else
		{
			PhysicsEnabled = true;
		}

	}

	#endregion
}
