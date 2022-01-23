using Sandbox;


[Library( "dm_python", Title = "Python" )]
[Hammer.EditorModel( "weapons/rust_pistol/rust_pistol.vmdl" )]
partial class Python : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/357/python_357_reference.vmdl";

	public override float PrimaryRate => 1.6f;

	public override int ClipSize => 6;
	public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 3.0f;

	public override int Bucket => 1;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
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
			DryFire();
			return;
		}


		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "rust_pistol.shoot" );

		//
		// Shoot the bullets
		//
		ShootBullet( 0.2f, 1.5f, 75.0f, 3.0f );

	}

	public override void Simulate(Client cl) 
	{
		if (AmmoClip < ClipSize && TimeSincePrimaryAttack >= 1f || AmmoClip == 0 && TimeSincePrimaryAttack > 1f) 
		{
			Reload();
		}

		base.Simulate(cl);
	}
}
