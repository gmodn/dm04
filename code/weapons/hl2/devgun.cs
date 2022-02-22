using Sandbox;


[Library( "devgun", Title = "Dev Gun" )]
[Hammer.EditorModel( "models/weapons/hl2_uspmatch/w_hl2_uspmatch.vmdl" )]
partial class devgun : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/weapons/dev/v_devgun.vmdl";

	public override float PrimaryRate => 20.0f;
	public override float SecondaryRate => 1.0f;
	public override int ClipSize => 1;
	public override float ReloadTime => 1f;

	public override int Bucket => 5;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_uspmatch/w_hl2_uspmatch.vmdl" );
		AmmoClip = 100;
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
		ShootBullet( 0f, 25f, 1000f, 1f );

	}
	
	public override void StartReloadEffects()
	{
		ViewModelEntity?.SetAnimParameter( "reload", true );
		PlaySound( "hl2_uspmatch.reload" );
	}
	
	public override void OnReloadFinish()
	{
		IsReloading = false;

		if ( Owner is DeathmatchPlayer player )
		{
			AmmoClip += 100;
		}
	}
}
