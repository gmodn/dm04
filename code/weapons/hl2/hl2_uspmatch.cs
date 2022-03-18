using Sandbox;


[Library( "hl2_uspmatch", Title = "9MM PISTOL" )]
[Hammer.EditorModel( "models/weapons/hl2_uspmatch/w_hl2_uspmatch.vmdl" )]
partial class hl2_uspmatch : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/weapons/hl2_uspmatch/v_hl2_uspmatch.vmdl";

	public override float PrimaryRate => 10.0f;
	public override float SecondaryRate => 2.0f;
	public override int ClipSize => 18;
	public override float ReloadTime => 1.4f;
	public override int Bucket => 1;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_uspmatch/w_hl2_uspmatch.vmdl" );

		AmmoClip = 18;
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed( InputButton.Attack1 );
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( !TakeAmmo( 1 ) )
		{
			Reload();
			PlaySound( "hl2_uspmatch.empty" );
			return;
		}

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "hl2_uspmatch.fire" );

		//
		// Shoot the bullets
		//
		//Rand.SetSeed( Time.Tick );
		ShootBullet( 0.1f, 1.5f, 8f, 3.0f );
	}

	public override void StartReloadEffects()
	{
		ViewModelEntity?.SetAnimParameter( "reload", true );
		PlaySound( "hl2_uspmatch.reload" );
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();

		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
		Particles.Create( "particles/casings/hl2_uspmatch_casing.vpcf", EffectEntity, "ejection_point" );

		//if ( IsLocalPawn )
		//{
		//	new Sandbox.ScreenShake.Perlin();
		//}

		ViewModelEntity?.SetAnimParameter( "fire", true );
		CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;
	}
	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 1 ); // TODO this is shit
												//anim.SetAnimParameter( "aimat_weight", 1.0f );
		anim.SetAnimParameter( "holdtype_handedness", 1 );
		if ( Input.Pressed( InputButton.Reload ) )
		{
			ViewModelEntity?.SetAnimParameter( "inspect", true );
		}
	}
}
