
/// <summary>
/// Temp disabled due to it not being able to pull from ammo count
/// </summary>

[Library( "dm_slam" ), HammerEntity]
[EditorModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" )]
[Title( "S.L.A.M." ), Category( "Weapons" )]
partial class SLAMWeapon : HLDMWeapon
{
/*	public override string ViewModelPath => "models/weapons/hl2_slam/v_slam.vmdl";

	public override float PrimaryRate => 1.0f;
	public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 1.0f;
	public override AmmoType AmmoType => AmmoType.Slam;
	public override int ClipSize => 1;
	public override int Bucket => 4;

	private SlamEntity slamthrown;
	private SlamEntity slammounted;
	List<Entity> slamsactive = new List<Entity>();

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_slam/w_slam.vmdl" );
		AmmoClip = 1;
	}

	public override bool CanPrimaryAttack()
	{
		return Input.Released( InputButton.PrimaryAttack );
	}

	public override void AttackPrimary()
	{
		if ( Owner is DeathmatchPlayer player )
		{



			//TimeSincePrimaryAttack = 0;

			//if (TimeSinceSecondaryAttack < 0.5) return;

			//if ( !TakeAmmo(1) )
			//{
			//	Reload();
			//	return;
			//}

			PlaySound( "hl2_grenade.throw" );
			ViewModelEntity?.SetAnimParameter( "fire_alt", true );

			var forward = player.EyeRotation.Forward;

			TraceResult tr = Trace.Ray( player.EyePosition, player.EyePosition + forward * 40 )
			.Radius( 1.5f )
			.Ignore( Owner )
			.Run();


			if ( !tr.Entity.IsValid() )
			{
				if ( Game.IsServer )
					using ( Prediction.Off() )
					{
						slamthrown = new SlamEntity();
						slamsactive.Add( slamthrown );
						slamthrown.Position = player.EyePosition;
						slamthrown.Rotation += player.EyeRotation;
						slamthrown.Owner = Owner;
						slamthrown.Velocity = player.EyeRotation.Forward * 450;
					}
			}
			else
			{
				if ( Game.IsServer )
					using ( Prediction.Off() )
					{
						slammounted = new SlamEntity();
						slammounted.PhysicsEnabled = false;
						slammounted.mounted = true;
						slammounted.SetModel( "models/weapons/hl2_slam/w_hl2_slam_open.vmdl" );
						slamsactive.Add( slammounted );
						slammounted.Position = tr.EndPosition;
						slammounted.Rotation = Rotation.From( Vector3.VectorAngle( tr.Normal ) );
						slammounted.Owner = Owner;
						slammounted.SetParent( tr.Entity );
						_ = slammounted.Arm( 3f );
					}
			}

		}
	}

	public override void AttackSecondary()
	{
		if ( Owner is DeathmatchPlayer player )
		{
			TimeSinceSecondaryAttack = 0;

			ViewModelEntity?.SetAnimParameter( "fire_alt", true );
		}

		if ( Game.IsServer )
			using ( Prediction.Off() )
			{
				if ( slamthrown.IsValid() || slammounted.IsValid() )
				{

					foreach ( var active in slamsactive )
					{

						(active as SlamEntity).DoExplosion();

					}
					slamsactive.Clear();
					foreach ( var month in slamsactive )
					{
						Log.Info( month );
					}
				}
			}
	}

	public override void SimulateAnimator( CitizenAnimationHelper anim )
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Punch;
		anim.AimBodyWeight = 1.0f;
	}*/
}
