using Sandbox;
using System;

[Library( "dm_smg", Title = "SMG" )]
[Hammer.EditorModel( "weapons/rust_smg/rust_smg.vmdl" )]
partial class SMG : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/smg1/smg1_reference.vmdl";

	public override float PrimaryRate => 15.0f;
	public override float SecondaryRate => 1.0f;
	public override int ClipSize => 45;
	public override float ReloadTime => 1.5f;
	public override int Bucket => 2;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_smg/rust_smg.vmdl" );
		AmmoClip = 45;
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

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "smg1_fire1" );

		//
		// Shoot the bullets
		//
		ShootBullet( 0.1f, 1.5f, 5.0f, 3.0f );

	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = 0f;
		TimeSinceSecondaryAttack = 0f;

		PlaySound( "rust_pumpshotgun.shoot" );
		ShootGrenade();
	}

	public override void Reload()
	{
		base.Reload();

		PlaySound( "smg1_reload" );
	}

	public override void Simulate(Client cl) 
	{
		base.Simulate(cl);
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();

		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
		Particles.Create( "particles/pistol_ejectbrass.vpcf", EffectEntity, "ejection_point" );

		if ( Owner == Local.Pawn )
		{
			new Sandbox.ScreenShake.Perlin(0.5f, 4.0f, 1.0f, 0.5f);
		}

		ViewModelEntity?.SetAnimBool( "fire", true );
		CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 2 ); // TODO this is shit
		anim.SetParam( "aimat_weight", 1.0f );
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

		grenade.ExplodeAsync( 1f );

	}

}
