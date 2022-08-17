using Sandbox;


[Library( "hl2_spas12", Title = "SHOTGUN" )]
[EditorModel( "models/weapons/hl2_spas12/w_hl2_spas12.vmdl" )]
partial class hl2_spas12 : HLDMWeapon
{ 
	public override string ViewModelPath => "models/weapons/hl2_spas12/v_hl2_spas12.vmdl";
	public override float PrimaryRate => 0.95f;
	public override float SecondaryRate => 0.8f;
	public override int BucketWeight => 50;
	public override AmmoType AmmoType => AmmoType.Buckshot;
	public override int ClipSize => 6;
	public override float ReloadTime => 0.8f;
	public override int Bucket => 3;

	[Net, Predicted]
	public bool StopReloading { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_spas12/w_hl2_spas12.vmdl" );

		AmmoClip = 6;
	}

	public override void Simulate( Client owner )
	{
		base.Simulate( owner );

		if ( IsReloading && (Input.Pressed( InputButton.PrimaryAttack ) || Input.Pressed( InputButton.SecondaryAttack )) )
		{
			StopReloading = true;
		}

		if ( !IsReloading && ViewModelEntity?.GetAnimParameterBool( "pumpreload" ) == true && TimeSinceReload >= (ReloadTime * 2.25f) )
			ViewModelEntity?.SetAnimParameter( "pumpreload", false );
	}

	public override void AttackPrimary() 
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( !TakeAmmo( 1 ) )
		{
			Reload();
			PlaySound( "hl2_spas12.empty" );
			return;
		}

		//(Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "hl2_spas12.fire" );

		//
		// Shoot the bullets
		//
		Rand.SetSeed( Time.Tick );
		for ( int i = 0; i < 7; i++ )
		{
			ShootBullet( 0.1f, 0.3f, 9f, 3.0f );
		}
	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = -0.5f;
		TimeSinceSecondaryAttack = -0.5f;

		if ( !TakeAmmo( 2 ) )
		{
			AttackPrimary();
			return;
		}

		//(Owner as AnimatedEntity).SetAnimParameter( "altfire", true );

		//
		// Tell the clients to play the shoot effects
		//
		DoubleShootEffects();
		PlaySound( "hl2_spas12.dbl_fire" );

		//
		// Shoot the bullets
		//
		Rand.SetSeed( Time.Tick );
		for ( int i = 0; i < 12; i++ )
		{
			ShootBullet( 0.2f, 0.3f, 9f, 3.0f );
		}
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();
		(Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );
		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
		Particles.Create( "particles/pistol_ejectbrass.vpcf", EffectEntity, "ejection_point" );

		ViewModelEntity?.SetAnimParameter( "fire", true );

		//CrosshairPanel?.CreateEvent( "fire" );
		
	}

	[ClientRpc]
	protected virtual void DoubleShootEffects()
	{
		Host.AssertClient();

		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );

		ViewModelEntity?.SetAnimParameter( "fire_alt", true );
		//CrosshairPanel?.CreateEvent( "fire" );
	}

	[ClientRpc]
	public override void StartReloadEffects()
	{
		ViewModelEntity?.SetAnimParameter( "reload", true );
	}

	public override void OnReloadFinish()
	{
		var stop = StopReloading;

		StopReloading = false;
		IsReloading = false;

		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;
		TimeSinceReload = 0;

		if ( AmmoClip >= ClipSize )
			return;

		if ( Owner is DeathmatchPlayer player )
		{
			var ammo = player.TakeAmmo( AmmoType, 1 );

			if ( ammo == 0 )
			{
				FinishReload();
				return;
			}

			AmmoClip += ammo;

			if ( AmmoClip < ClipSize && !stop )
			{
				Reload();
			}
			else
			{
				FinishReload();
			}
		}
	}

	public override void Reload()
	{
		if(AmmoClip == 0)
			ViewModelEntity?.SetAnimParameter( "pumpreload", true);

		base.Reload();
	}

	[ClientRpc]
	protected virtual void FinishReload()
	{
		ViewModelEntity?.SetAnimParameter( "reload", false );
		//ViewModelEntity?.SetAnimParameter( "reload_finished", true );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 2 ); // TODO this is shit
		//anim.SetAnimParameter( "aimat_weight", 1.0f );
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;
	}
}
