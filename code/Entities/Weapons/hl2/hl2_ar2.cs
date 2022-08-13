using Sandbox;
using System;

[Library( "hl2_ar2", Title = "AR2" )]
[EditorModel( "models/weapons/hl2_ar2/w_hl2_ar2.vmdl" )]
partial class hl2_ar2 : HLDMWeapon
{ 
	public override string ViewModelPath => "models/weapons/hl2_ar2/v_hl2_ar2.vmdl";

	public override float PrimaryRate => 10.0f;
	public override float SecondaryRate => 0.5f;
	public override int ClipSize => 30;
	public override AmmoType AmmoType => AmmoType.AR2;
	public override AmmoType SecondaryAmmo => AmmoType.AR2_ball; //Secondary Ammo Type
	//public override string AmmoIcon => "u";
	public override float ReloadTime => 1.7f;
	public override int Bucket => 2;

	float glow = 0f;
	/// <summary>
	/// bug if you have no ammo in the alt fire but right click you have to wait before you can primary fire
	/// </summary>
	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_ar2/w_hl2_ar2.vmdl" );

		AmmoClip = 30;
		SecondaryAmmoClip = 3;
	}
	public override bool CanSecondaryAttack()
	{
		return base.CanSecondaryAttack() && Input.Pressed( InputButton.SecondaryAttack );
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		//TimeSinceSecondaryAttack = 0;

		if ( TimeSinceSecondaryAttack < 2 ) return;

		if ( !TakeAmmo( 1 ) )
		{
			Reload();
			if (Input.Pressed( InputButton.PrimaryAttack )) PlaySound( "hl2_ar2.empty" );
			return;
		}

		(Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "hl2_ar2.fire" );
		glow = 8;

		//
		// Shoot the bullets
		//
		Rand.SetSeed(Time.Tick);
		ShootBullet( 0.1f, 1.5f, 11, 3.0f );
	}

	public override void AttackSecondary()
	{
		if ( Owner is DeathmatchPlayer player )
		{
			if ( !TakeSecondaryAmmo( player, SecondaryAmmo ) )
			{
				PlaySound( "hl2_uspmatch.empty" );
				return;
			}
		
			PlaySound( "hl2_ar2.secondary_fire" );
			ViewModelEntity?.SetAnimParameter( "fire_alt", true );
			glow = 20;

			if ( IsServer )
				using ( Prediction.Off() )
				{
					var grenade = new hl2_ar2ball();
					grenade.Position = Owner.EyePosition;
					grenade.Rotation = Owner.EyeRotation;
					grenade.Owner = Owner;
					grenade.Velocity = Owner.EyeRotation.Forward * 1000;
			}
			
		}
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

	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();
		Particles.Create( "particles/casings/hl2_ar2_casing.vpcf", EffectEntity, "ejection_point" );
		


		ViewModelEntity?.SetAnimParameter( "fire", true );
		//CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 2 );
		////anim.SetAnimParameter( "aimat_weight", 1.0f );

		ViewModelEntity?.SceneObject.Attributes.Set( "glow", glow );
		if (glow > 0) glow -= 0.5f;
	}
}
