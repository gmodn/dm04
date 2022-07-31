using Sandbox;
using System;
using System.Threading.Tasks;

[Library( "hl2_slamthrown" )]
[HideInEditor]
partial class hl2_slamthrown : Prop
{
	Particles LaserParticle;
	public bool mounted { get; set; }
	public bool armed { get; set; }
	public override void Spawn()
	
	{
		Tags.Add( "projectile" );
		base.Spawn();
		SetModel( "models/weapons/hl2_slam/w_hl2_slam_closed.vmdl" );

		Health = 5;
		MoveType = MoveType.Physics;
		if ( mounted == true )
		{
			PhysicsEnabled = false;
		}
		else
		{
			PhysicsEnabled = true;
		}
		UsePhysicsCollision = true;
		SetInteractsExclude( CollisionLayer.Player );
	}

	public async Task Arm( float seconds )
	{
		// arming noise

		await GameTask.DelaySeconds( seconds );

		LaserParticle = Particles.Create( "particles/weapons/hl2_slamthrown.vpcf", this, "laser", true );
		//LaserParticle.SetPosition( 1, Position );
		armed = true;
		

		// armed chirp

		
	}
	[Event.Tick.Server]
	protected virtual async Task OnTickAsync()
	{
		if ( armed == true)
		{
			var tr = Trace.Ray( Position, Position + Rotation.Forward * 4000.0f )
				.Ignore( this )
				.Run();
			LaserParticle.SetPosition( 3, tr.EndPosition );
			if ( tr.Entity != null && tr.Entity is not WorldEntity )
			{
				await TriggeredExplosion( 0.5f );
			}

		}
	}
	public override void OnKilled()
	{
		base.OnKilled();

		//Blow up but do no damage
		PlaySound("balloon_pop_cute");
	}

	async Task TriggeredExplosion( float delay )
	{
		await Task.DelaySeconds( delay );

		if ( !IsValid ) return;
		Sound.FromWorld( "rust_pumpshotgun.shootdouble", Position );
		// dumb dumb no longer works Aaaaah DoExplosion();
		DeathmatchGame.Explosion( this, Owner, Position, 400, 100, 1.0f );
		
		LaserParticle?.Destroy( true );
		LaserParticle = null;

		Delete();
	}
	public void DoExplosion()
	{
		if ( Model == null || Model.IsError )
			return;

		///idk what this is replaced with	if ( !Model.HasExplosionBehavior() )
		//return;

		var srcPos = Position;
		if ( PhysicsBody.IsValid() ) srcPos = PhysicsBody.MassCenter;
		DeathmatchGame.Explosion( this, Owner, Position, 400, 100, 1.0f );
		Delete();
		///idk what this is replaced with	///var explosionBehavior = Model.GetExplosionBehavior();

		// Damage and push away all other entities
		///if ( explosionBehavior.Radius > 0.0f )
		///{
		///new ExplosionEntity
		//	{
		///		Position = srcPos,
		//		Radius = explosionBehavior.Radius,
		///		Damage = explosionBehavior.Damage,
		//	ForceScale = explosionBehavior.Force,
		//	ParticleOverride = explosionBehavior.Effect,
		//	SoundOverride = explosionBehavior.Sound
		//}.Explode( this );
		//	Delete();
		//}
	}
}
