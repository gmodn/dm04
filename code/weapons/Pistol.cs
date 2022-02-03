using Sandbox;


[Library( "dm04_pistol", Title = "Pistol" )]
[Hammer.EditorModel("models/worldmodels/pistol_reference.vmdl")]
[Hammer.EntityTool( "Pistol", "DM:04" )]
partial class Pistol : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/viewmodels/pistol/pistol_reference.vmdl";

	public override float PrimaryRate => 15.0f;
	public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 1.0f;

	public override AmmoType AmmoType => AmmoType.Pistol;
	public override int ClipSize => 18;

	public override int Bucket => 1;

	public override void Spawn()
	{
		base.Spawn();

		SetModel("models/worldmodels/pistol_reference.vmdl");
		AmmoClip = ClipSize;
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

		(Owner as AnimEntity).SetAnimBool( "b_attack", true );

		ShootEffects();
		PlaySound( "pistol_fire" );

		ShootBullet( 0.2f, 1.5f, 8.0f, 3.0f );

	}

	public override void Simulate(Client cl) 
	{
		base.Simulate(cl);
	}
}
