[Library( "dm_gravgun" ), HammerEntity]
[EditorModel( "models/weapons/hl2_gravgun/v_hl2_gravgun.vmdl" )]
[Title( "GravGun" ), Category( "Weapons" )]
partial class GravGun : HLDMWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_gravgun/v_hl2_gravgun.vmdl";
	public override AmmoType AmmoType => AmmoType.None;
	public override int SlotColumn => 0;
	public override int SlotOrder => 3;

	public PhysicsBody HeldBody { get; private set; }
	public Vector3 HeldPos { get; private set; }
	public Rotation HeldRot { get; private set; }
	public ModelEntity HeldEntity { get; private set; }
	public Vector3 HoldPos { get; private set; }
	public Rotation HoldRot { get; private set; }

	protected virtual float MaxPullDistance => 2000.0f;
	protected virtual float MaxPushDistance => 500.0f;
	protected virtual float LinearFrequency => 10.0f;
	protected virtual float LinearDampingRatio => 1.0f;
	protected virtual float AngularFrequency => 10.0f;
	protected virtual float AngularDampingRatio => 1.0f;
	protected virtual float PullForce => 20.0f;
	protected virtual float PushForce => 1000.0f;
	protected virtual float ThrowForce => 2000.0f;
	protected virtual float HoldDistance => 50.0f;
	protected virtual float AttachDistance => 150.0f;
	protected virtual float DropCooldown => 0.5f;
	protected virtual float BreakLinearForce => 2000.0f;

	private const string grabbedTag = "grabbed";

	private TimeSince timeSinceDrop;
	public override void Spawn()
	{
		base.Spawn();

		Tags.Add( "weapon" );
		SetModel( "models/weapons/hl2_gravgun/w_hl2_gravgun.vmdl" );
	}

	public override void Simulate( IClient client )
	{
		if ( !Game.IsServer || Owner is not DeathmatchPlayer owner ) return;

		using ( Prediction.Off() )
		{
			var eyePos = owner.EyePosition;
			var eyeRot = owner.EyeRotation;
			var eyeDir = owner.EyeRotation.Forward;

			if ( HeldBody.IsValid() && HeldBody.PhysicsGroup != null )
			{
				if ( Input.Pressed( "attack1" ) )
				{
					if ( HeldBody.PhysicsGroup.BodyCount > 1 )
					{
						// Don't throw ragdolls as hard
						HeldBody.PhysicsGroup.ApplyImpulse( eyeDir * (ThrowForce * 0.5f), true );
						HeldBody.PhysicsGroup.ApplyAngularImpulse( Vector3.Random * ThrowForce, true );
					}
					else
					{
						HeldBody.ApplyImpulse( eyeDir * (HeldBody.Mass * ThrowForce) );
						HeldBody.ApplyAngularImpulse( Vector3.Random * (HeldBody.Mass * ThrowForce) );
					}

					GrabEnd();
				}
				else if ( Input.Pressed( "attack2" ) )
				{
					GrabEnd();
				}
				else
				{
					GrabMove( eyePos, eyeDir, eyeRot );
				}

				return;
			}

			if ( timeSinceDrop < DropCooldown )
				return;

			var tr = Trace.Ray( eyePos, eyePos + eyeDir * MaxPullDistance )
				.UseHitboxes()
				.WithAnyTags( "solid", "debris" )
				.Ignore( this )
				.Radius( 2.0f )
				.Run();

			if ( !tr.Hit || !tr.Body.IsValid() || !tr.Entity.IsValid() || tr.Entity.IsWorld )
				return;

			if ( tr.Entity.PhysicsGroup == null )
				return;

			var modelEnt = tr.Entity as ModelEntity;
			if ( !modelEnt.IsValid() )
				return;

			if ( modelEnt.Tags.Has( grabbedTag ) )
				return;

			var body = tr.Body;

			if ( body.BodyType != PhysicsBodyType.Dynamic )
				return;

			if ( Input.Pressed( "attack1" ) )
			{
				if ( tr.Distance < MaxPushDistance )
				{
					var pushScale = 1.0f - Math.Clamp( tr.Distance / MaxPushDistance, 0.0f, 1.0f );
					body.ApplyImpulseAt( tr.EndPosition, eyeDir * (body.Mass * (PushForce * pushScale)) );
				}
			}
			else if ( Input.Down( "attack2" ) )
			{
				var physicsGroup = tr.Entity.PhysicsGroup;

				if ( physicsGroup.BodyCount > 1 )
				{
					body = modelEnt.PhysicsBody;
					if ( !body.IsValid() )
						return;
				}

				var attachPos = body.FindClosestPoint( eyePos );

				if ( eyePos.Distance( attachPos ) <= AttachDistance )
				{
					var holdDistance = HoldDistance + attachPos.Distance( body.MassCenter );
					GrabStart( modelEnt, body, eyePos + eyeDir * holdDistance, eyeRot );
				}
				else
				{
					physicsGroup.ApplyImpulse( eyeDir * -PullForce, true );
				}
			}
		}
	}

	private void Activate()
	{
	}

	private void Deactivate()
	{
		GrabEnd();
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		if ( Game.IsServer )
		{
			Activate();
		}
	}

	public override void ActiveEnd( Entity ent, bool dropped )
	{
		base.ActiveEnd( ent, dropped );

		if ( Game.IsServer )
		{
			Deactivate();
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		if ( Game.IsServer )
		{
			Deactivate();
		}
	}

	public override void OnCarryDrop( Entity dropper )
	{
	}

	[Event.Physics.PreStep]
	public void OnPrePhysicsStep()
	{
		if ( !Game.IsServer )
			return;

		if ( !HeldBody.IsValid() )
			return;

		if ( HeldEntity is Player )
			return;

		var velocity = HeldBody.Velocity;
		Vector3.SmoothDamp( HeldBody.Position, HoldPos, ref velocity, 0.1f, Time.Delta );
		HeldBody.Velocity = velocity;

		var angularVelocity = HeldBody.AngularVelocity;
		Rotation.SmoothDamp( HeldBody.Rotation, HoldRot, ref angularVelocity, 0.1f, Time.Delta );
		HeldBody.AngularVelocity = angularVelocity;
	}

	private static bool IsBodyGrabbed(PhysicsBody body)
	{
		// There for sure is a better way to deal with this
		if (All.OfType<GravGun>().Any(x => x?.HeldBody?.PhysicsGroup == body?.PhysicsGroup)) return true;

		return false;
	}

	private void GrabStart( ModelEntity entity, PhysicsBody body, Vector3 grabPos, Rotation grabRot )
	{
		if ( !body.IsValid() )
			return;

		if ( body.PhysicsGroup == null )
			return;

		GrabEnd();

		HeldBody = body;
		HeldPos = HeldBody.LocalMassCenter;
		HeldRot = grabRot.Inverse * HeldBody.Rotation;

		HoldPos = HeldBody.Position;
		HoldRot = HeldBody.Rotation;

		HeldBody.Sleeping = false;
		HeldBody.AutoSleep = false;

		if ( entity is GrenadeThrown grenade )
			grenade.ResetTimerGrav();

		HeldEntity = entity;
		HeldEntity.Tags.Add( grabbedTag );

		Client?.Pvs.Add( HeldEntity );
	}

	private void GrabEnd()
	{
		timeSinceDrop = 0;

		if ( HeldBody.IsValid() )
		{
			HeldBody.AutoSleep = true;
		}

		if ( HeldEntity.IsValid() )
		{
			Client?.Pvs.Remove( HeldEntity );
		}

		HeldBody = null;
		HeldRot = Rotation.Identity;

		if ( HeldEntity.IsValid() )
		{
			HeldEntity.Tags.Remove( grabbedTag );
		}

		HeldEntity = null;
	}

	private void GrabMove( Vector3 startPos, Vector3 dir, Rotation rot )
	{
		if ( !HeldBody.IsValid() )
			return;

		var attachPos = HeldBody.FindClosestPoint( startPos );
		var holdDistance = HoldDistance + attachPos.Distance( HeldBody.MassCenter );

		HoldPos = startPos - HeldPos * HeldBody.Rotation + dir * holdDistance;
		HoldRot = rot * HeldRot;
	}

	public bool IsUsable( Entity user )
	{
		return Owner == null || HeldBody.IsValid();
	}

	public override void SimulateAnimator( CitizenAnimationHelper anim )
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Rifle;
		anim.AimBodyWeight = 1.0f;
	}
}
