using Sandbox;
using System;

[Library( "dm04_grenade", Title = "Grenade" )]
[Hammer.EditorModel( "weapons/rust_smg/rust_smg.vmdl" )]
partial class Grenade : BaseDmWeapon
{
	
	//TODO: Fix animations they are shit lol

	public override string ViewModelPath => "models/grenade/grenade_reference.vmdl";

	public override float PrimaryRate => 15.0f;
	public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 1.0f;

	public override AmmoType AmmoType => AmmoType.Grenade;
	public override int ClipSize => 3;

	public override int Bucket => 4;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
		AmmoClip = 3;
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

		ShootGrenade();

		ViewModelEntity?.SetAnimBool( "fire", true );
	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( !TakeAmmo( 1 ) )
		{
			DryFire();
			return;
		}

		AltToss();

		ViewModelEntity?.SetAnimBool( "reload", true );
	}

	public override void Reload()
	{
		//unused
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );
	}

	private void ShootGrenade()
	{
		if ( Host.IsClient )
			return;

		var grenade = new Prop
		{
			Position = Owner.EyePos + Owner.EyeRot.Forward * 50,
			Rotation = Owner.EyeRot,
			Scale = 0.25f
		};

		//TODO: Should be replaced with an actual grenade model
		grenade.SetModel( "models/rust_props/barrels/fuel_barrel.vmdl" );
		grenade.Velocity = Owner.EyeRot.Forward * 1000;

		grenade.ExplodeAsync( 4f );

	}

	private void AltToss()
	{
		if ( Host.IsClient )
			return;

		var grenade = new Prop
		{
			Position = Owner.EyePos + Owner.EyeRot.Forward * 50,
			Rotation = Owner.EyeRot,
			Scale = 0.25f
		};

		//TODO: Should be replaced with an actual grenade model
		grenade.SetModel( "models/rust_props/barrels/fuel_barrel.vmdl" );
		grenade.Velocity = Owner.EyeRot.Forward * 400;

		grenade.ExplodeAsync( 4f );

	}

}
