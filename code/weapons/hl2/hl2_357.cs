using Sandbox;


[Library( "hl2_357", Title = "357" )]
[Hammer.EditorModel( "models/weapons/hl2_357/w_hl2_357.vmdl" )]
partial class hl2_357 : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/weapons/hl2_357/v_hl2_357.vmdl";
	public override float PrimaryRate => 1.3f;
	public override int ClipSize => 6;
	public override AmmoType AmmoType => AmmoType.Magnum;
	public override string AmmoIcon => "q";
	public override float ReloadTime => 4f;
	public override int Bucket => 1;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_357/w_hl2_357.vmdl" );

		AmmoClip = 6;
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
		PlaySound( "hl2_357.fire" );

		//
		// Shoot the bullets
		//
		//Rand.SetSeed( Time.Tick );
		ShootBullet( 0f, 1.5f, 75f, 3.0f );

	}
	
	public override void StartReloadEffects()
	{
		ViewModelEntity?.SetAnimParameter( "reload", true );
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
		if ( Input.Pressed( InputButton.Reload ) )
		{
			ViewModelEntity?.SetAnimParameter( "inspect", true );
		}
	}
}
