using Sandbox;
using System;

[Library( "dm04_pulsesmg", Title = "Pulse Rifle" )]
[Hammer.EditorModel("models/worldmodels/w_irifle_reference.vmdl")]
[Hammer.EntityTool( "Pulse Rifle", "DM:04" )]
partial class PulseSMG : BaseDmWeapon
{
	public override string ViewModelPath => "models/viewmodels/irifle/irifle_reference.vmdl";

	public override float PrimaryRate => 12.0f;
	public override float SecondaryRate => 1.0f;
	public override int ClipSize => 30;
	public override float ReloadTime => 1.5f;
	public override int Bucket => 2;

	public override AmmoType AmmoType => AmmoType.AR2;

	public override void Spawn()
	{
		base.Spawn();

		SetModel("models/worldmodels/w_irifle_reference.vmdl");
		AmmoClip = 30;
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( !TakeAmmo( 1 ) )
		{
			return;
		}

		(Owner as AnimEntity).SetAnimBool( "b_attack", true );

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "fire1" );

		//
		// Shoot the bullets
		//
		ShootBullet( 0.1f, 1.5f, 5.0f, 3.0f );

	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = 0f;
		TimeSinceSecondaryAttack = 0f;

		//PlaySound( "rust_pumpshotgun.shoot" );
		ViewModelEntity?.SetAnimBool("alt-fire", true);
		ShootBalls();
	}

	public override void Simulate( Client cl )
	{

		base.Simulate( cl );
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();

		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
		Particles.Create( "particles/pistol_ejectbrass.vpcf", EffectEntity, "ejection_point" );

		if ( Owner == Local.Pawn )
		{
			new Sandbox.ScreenShake.Perlin( 0.5f, 4.0f, 1.0f, 0.5f );
		}

		ViewModelEntity?.SetAnimBool( "fire", true );
		CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 2 ); // TODO this is shit
		anim.SetParam( "aimat_weight", 1.0f );
	}

	private void ShootBalls()
	{

		//Shoot balls at some point

	}

}
