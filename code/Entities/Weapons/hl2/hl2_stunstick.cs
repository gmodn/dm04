using Sandbox;


[Library( "hl2_stunstick", Title = "STUNSTICK" )]
[EditorModel( "models/weapons/hl2_crowbar/w_hl2_crowbar.vmdl" )]
partial class hl2_stunstick : HLDMWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_stunstick/v_stunbaton.vmdl";
	public override float PrimaryRate => 1.0f;
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

		PlaySound( "hl2_stunstick.swing" );
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
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 100, 40 )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
			}
		}

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

		ViewModelEntity?.SetAnimParameter( "hit", true );
		PlaySound("hl2_stunstick.fleshhit");
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 0 ); // TODO this is shit
		//anim.SetAnimParameter( "aimat_weight", 1.0f );
		if ( Input.Pressed( InputButton.Reload ) )
		{
			ViewModelEntity?.SetAnimParameter( "inspect", true );
		}
	}
}
