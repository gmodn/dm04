﻿using Sandbox;


[Library( "dm_grenade", Title = "GRENADE" )]
[EditorModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" )]
partial class Grenade : HLDMWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_grenade/v_hl2_grenade.vmdl";
	public override float PrimaryRate => 1f;
	public override float ReloadTime => 0;
	public override int ClipSize => 1;
	public override AmmoType AmmoType => AmmoType.Grenade;
	//public override string AmmoIcon => "v";

	public override int Bucket => 4;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" );
	}

	public override bool CanReload()
	{
		return false;
	}
	public override bool CanSecondaryAttack()
	{
		return base.CanSecondaryAttack() && Input.Pressed( InputButton.SecondaryAttack );
	}

	public override void AttackPrimary()
	{
		if ( Owner is DeathmatchPlayer player )
		{
			//TimeSincePrimaryAttack = 0;

			//if (TimeSinceSecondaryAttack < 0.5) return;

			if ( !TakeAmmo( 1 ) )
			{
				Reload();
				return;
			}
			ShootEffects();
			PlaySound( "hl2_grenade.throw" );
			ViewModelEntity?.SetAnimParameter( "fire", true );
			player.TakeAmmo( AmmoType, 1 );

			if ( Game.IsServer )
				using ( Prediction.Off() )
				{
					var grenade = new GrenadeThrown();
					grenade.Position = player.EyePosition;
					grenade.Rotation += player.EyeRotation;
					grenade.Owner = player;
					grenade.Velocity = player.EyeRotation.Forward * 1000;
				}
		}
	}
	public override void AttackSecondary()
	{
		if ( Owner is DeathmatchPlayer player )
		{
			//TimeSincePrimaryAttack = 0;

			//if (TimeSinceSecondaryAttack < 0.5) return;

			if ( !TakeAmmo( 1 ) )
			{
				Reload();
				return;
			}
			ShootEffects();
			PlaySound( "hl2_grenade.throw" );
			ViewModelEntity?.SetAnimParameter( "fire_alt", true );
			player.TakeAmmo( AmmoType, 1 );

			if ( Game.IsServer )
				using ( Prediction.Off() )
				{
					var grenade = new GrenadeThrown();
					grenade.Position = player.EyePosition;
					grenade.Rotation += player.EyeRotation;
					grenade.Owner = player;
					grenade.Velocity = player.EyeRotation.Forward * 400;
				}
		}
	}
	[ClientRpc]
	protected override void ShootEffects()
	{
		(Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;
	}
	public override void SimulateAnimator( CitizenAnimationHelper anim )
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Swing;	
	}
}