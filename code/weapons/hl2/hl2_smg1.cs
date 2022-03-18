using Sandbox;
using System;

[Library( "hl2_smg1", Title = "SMG (SUBMACHINE GUN)" )]
[Hammer.EditorModel( "models/weapons/hl2_smg1/w_hl2_smg1.vmdl" )]
partial class hl2_smg1 : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/weapons/hl2_smg1/v_hl2_smg1.vmdl";

	public override float PrimaryRate => 13.3f;
	public override float SecondaryRate => 1.0f;
	public override int ClipSize => 45;
	public override AmmoType AmmoType => AmmoType.SMG;
	public override AmmoType SecondaryAmmo => AmmoType.SMG_grenade; //Secondary Ammo Type
	public override string AmmoIcon => "r";
	public override string AltIcon => "t";
	public override float ReloadTime => 1.7f;
	public override int Bucket => 2;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_smg1/w_hl2_smg1.vmdl" );

		AmmoClip = 45;
		SecondaryAmmoClip = 3;
	}
	public override bool CanSecondaryAttack()
	{
		return base.CanSecondaryAttack() && Input.Pressed( InputButton.Attack2 );
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;

		if ( TimeSinceSecondaryAttack < 0.5 ) return;

		if ( !TakeAmmo( 1 ) )
		{
			Reload();
			if ( Input.Pressed( InputButton.Attack1 ) ) PlaySound( "hl2_uspmatch.empty" );
			return;
		}

		(Owner as AnimEntity).SetAnimParameter( "b_attack", true );

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "hl2_smg1.fire" );

		//
		// Shoot the bullets
		//
		Rand.SetSeed(Time.Tick);
		ShootBullet( 0.1f, 1.5f, 5f, 3.0f );
	}

	public override void AttackSecondary()
	{
		if ( Owner is DeathmatchPlayer player )
		{
			if ( player.AmmoCount( SecondaryAmmo ) <= 0 )
			{
				PlaySound( "hl2_uspmatch.empty" );
				return;
			}
			else
			{
				PlaySound( "hl2_smg1.glauncher" );
				ViewModelEntity?.SetAnimParameter( "fire_alt", true );
				player.TakeAmmo( SecondaryAmmo, 1 );
				SecondaryAmmoClip = player.AmmoCount(SecondaryAmmo);

				if ( IsServer )
					using ( Prediction.Off() )
					{
						var grenade = new hl2_smg1grenade();
						grenade.Position = Owner.EyePosition;
						grenade.Rotation = Owner.EyeRotation;
						grenade.Owner = Owner;
						grenade.Velocity = Owner.EyeRotation.Forward * 1750;
					}
			}
		}
	}


	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();

		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
		Particles.Create( "particles/casings/hl2_uspmatch_casing.vpcf", EffectEntity, "ejection_point" );

		//if ( IsLocalPawn )
		//{
		//	new Sandbox.ScreenShake.Perlin();
		//}

		ViewModelEntity?.SetAnimParameter( "fire", true );
		CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 2 ); // TODO this is shit
		//anim.SetAnimParameter( "aimat_weight", 1.0f );
	}

	public override void StartReloadEffects()
	{
		ViewModelEntity?.SetAnimParameter( "reload", true );
		PlaySound( "hl2_smg1.reload" );
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;
	}
}
