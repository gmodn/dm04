using Sandbox;


[Library( "hl2_slam", Title = "S.L.A.M (Selectable Lightweight Attack Munition" )]
[Hammer.EditorModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" )]
partial class hl2_slam : BaseDmWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_grenade/v_hl2_grenade.vmdl";
	public override float PrimaryRate => 1.0f;
	public override int ClipSize => 1;
	public override AmmoType AmmoType => AmmoType.SLAM;

	public override int Bucket => 4;

	private hl2_slamthrown slamthrown;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" );
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

			foreach (var tr in TraceBullet(Owner.EyePosition, Owner.EyePosition + forward * 40, 1.5f))
            {
				if (!tr.Entity.IsValid())
                {
					if (IsServer)
						using (Prediction.Off())
						{
							slamthrown = new hl2_slamthrown();
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
							var slammounted = new hl2_slammounted();
							slammounted.Position = tr.EndPosition;
							slammounted.Rotation = Rotation.From(Vector3.VectorAngle(tr.Normal));
							slammounted.Owner = Owner;
							slammounted.SetParent(tr.Entity);
						}
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

				slamthrown.Explode();
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
