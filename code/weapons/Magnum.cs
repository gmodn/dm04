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

	private TimeSince timeSinceReloading;

	private bool isPastSequenceOne;
	private bool isPastSequenceTwo;
	private bool isPastSequenceThree;
	private bool isPastSequenceFour;

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
		PlaySound( "357_fire" + Rand.Int(2,3).ToString() );
		
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

		timeSinceReloading = 0;
		isPastSequenceOne = false;
		isPastSequenceTwo = false;
		isPastSequenceThree = false;
		isPastSequenceFour = false;
	}

	public override void Simulate(Client cl) 
	{
		base.Simulate(cl); 
		
		if ( IsReloading )
		{
			//Barrel eject
			if ( timeSinceReloading > 1.0f && isPastSequenceOne != true )
			{
				isPastSequenceOne = true;
				PlaySound( "357_reload1" );
			}

			//Bullets eject
			if ( timeSinceReloading > 1.5f && isPastSequenceTwo != true )
			{
				isPastSequenceTwo = true;
				PlaySound( "357_reload4" );
			}

			//Bullets insert
			if ( timeSinceReloading > 2.25f && isPastSequenceThree != true )
			{
				isPastSequenceThree = true;
				PlaySound( "357_reload3" );
			}

			//Spin barrel
			if ( timeSinceReloading > 3.0f && isPastSequenceFour != true )
			{
				isPastSequenceFour = true;
				PlaySound( "357_spin1" );
			}
		} 
	}
}
