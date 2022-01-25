using Sandbox;


[Library( "dm04_357", Title = ".357 Magnum" )]
[Hammer.EditorModel( "weapons/rust_pistol/rust_pistol.vmdl" )]
partial class Magnum : BaseDmWeapon
{
	public override string ViewModelPath => "models/357/python_357_reference.vmdl";

	public override float PrimaryRate => 1.4f;

	public override int ClipSize => 6;

	public override AmmoType AmmoType => AmmoType.Magnum;

	//357 magnum doesn't have a secondary fire
	//public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 3.75f;
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

		ShootEffects();
		PlaySound( "357_fire" );
		
		if ( Owner == Local.Pawn )
			new Sandbox.ScreenShake.Perlin( 0.5f, 2.0f, 5.0f, 1.5f );

		ShootBullet( 0.2f, 1.5f, 75.0f, 3.0f );
	}

	[ClientRpc]
	public override void DryFire()
	{
		
	}

	public override void Reload()
	{
		base.Reload();
	}

	public override void Simulate(Client cl) 
	{
		base.Simulate(cl); 
	}
}
