using Sandbox;
using System;
using System.Collections.Generic;

[Library( "hl2_slam", Title = "S.L.A.M (Selectable Lightweight Attack Munition" )]
[EditorModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" )]
partial class hl2_slam : HLDMWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_slam/v_slam.vmdl";
	public override float PrimaryRate => 1.0f;
	public override int ClipSize => 1;
	public override AmmoType AmmoType => AmmoType.SLAM;
	//public override string AmmoIcon => "v";
	List<Entity> slamsactive = new List<Entity>();
	public override int Bucket => 4;

	private hl2_slamthrown slamthrown;
	private hl2_slamthrown slammounted;
	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_slam/w_slam.vmdl" );
	}

	public override bool CanReload()
	{
		return false;
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

			var forward = Owner.EyeRotation.Forward;

			TraceResult tr = Trace.Ray( Owner.EyePosition, Owner.EyePosition + forward * 40 )
			.Radius( 1.5f )
			.Ignore( Owner )
			.Run();


			if (!tr.Entity.IsValid())
            {
					if (IsServer)
						using (Prediction.Off())
						{
							slamthrown = new hl2_slamthrown();
							slamsactive.Add( slamthrown );
							slamthrown.Position = Owner.EyePosition;
							slamthrown.Rotation += Owner.EyeRotation;
							slamthrown.Owner = Owner;
							slamthrown.Velocity = Owner.EyeRotation.Forward * 450;
						}
			}
				else
                {
					if (IsServer)
						using (Prediction.Off())
						{
							slammounted = new hl2_slamthrown();
							slammounted.PhysicsEnabled = false;
							slammounted.mounted = true;
							slammounted.SetModel( "models/weapons/hl2_slam/w_hl2_slam_open.vmdl" );
							slamsactive.Add( slammounted );
							slammounted.Position = tr.EndPosition;
							slammounted.Rotation = Rotation.From(Vector3.VectorAngle(tr.Normal));
							slammounted.Owner = Owner;
							slammounted.SetParent(tr.Entity);
						_ = slammounted.Arm( 3f );
					}
				}
			
		}
	}

	public override void AttackSecondary()
	{
		if (Owner is DeathmatchPlayer player)
		{
			TimeSinceSecondaryAttack = 0;

			ViewModelEntity?.SetAnimParameter("fire_alt", true);
		}

		if (IsServer)
			using ( Prediction.Off() )
			{
				if ( slamthrown.IsValid() || slammounted.IsValid())
				{
					
					foreach(var active in slamsactive )
					{
						
						(active as hl2_slamthrown).DoExplosion();

					}
					slamsactive.Clear();
					foreach ( var month in slamsactive )
					{
						Log.Info( month );
					}
				}
			}
	}

	public override void SimulateAnimator(PawnAnimator anim)
	{
		anim.SetAnimParameter("holdtype", 1); // TODO this is shit
		//anim.SetAnimParameter("aimat_weight", 1.0f);
		// TODO - Check if close enough to wall and set animbool if you are
		//if (  )
		//{
		//	ViewModelEntity?.SetAnimParameter("close", true);
		//}
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;
	}
}
