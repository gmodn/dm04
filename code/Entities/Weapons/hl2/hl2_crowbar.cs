using Sandbox;


[Library( "hl2_crowbar", Title = "CROWBAR" )]
[EditorModel( "models/weapons/hl2_crowbar/w_hl2_crowbar.vmdl" )]
partial class hl2_crowbar : DeathmatchWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_crowbar/v_hl2_crowbar.vmdl";
	public override float PrimaryRate => 2.0f;
	public override AmmoType AmmoType => AmmoType.None;
	public override int Bucket => 0;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_crowbar/w_hl2_crowbar.vmdl" );
	}

	public override bool CanReload()
	{
		return false;
	}

	public override void AttackPrimary()
	{
		if ( MeleeAttack() )
		{
			OnMeleeHit();
		}
		else
		{
			OnMeleeMiss();
		}

		PlaySound( "hl2_crowbar.swing" );
	}

	private bool MeleeAttack()
	{
		var forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;

		bool hit = false;

		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + forward * 80, 20.0f ) )
		{
			if ( !tr.Entity.IsValid() ) continue;

			tr.Surface.DoBulletImpact( tr );

			hit = true;

			if ( !IsServer ) continue;

			using ( Prediction.Off() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 100, 25 )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
			}
		}

		(Owner as AnimatedEntity).SetAnimParameter("b_attack", true);

		return hit;
	}

	[ClientRpc]
	private void OnMeleeMiss()
	{
		Host.AssertClient();

		//if ( IsLocalPawn )
		//{
		//	_ = new Sandbox.ScreenShake.Perlin();
		//}
		(Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );

		ViewModelEntity?.SetAnimParameter( "miss", true );
	}

	[ClientRpc]
	private void OnMeleeHit()
	{
		Host.AssertClient();

		//if ( IsLocalPawn )
		//{
		//	_ = new Sandbox.ScreenShake.Perlin( 1.0f, 1.0f, 3.0f );
		//}
		(Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );
		ViewModelEntity?.SetAnimParameter( "hit", true );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 0 ); // TODO this is shit
		//anim.SetAnimParameter( "aimat_weight", 1.0f );
		//anim.SetAnimParameter( "holdtype_handedness", 1 );
		if ( Input.Pressed( InputButton.Reload ) )
		{
			ViewModelEntity?.SetAnimParameter( "inspect", true );
		}
	}
}
