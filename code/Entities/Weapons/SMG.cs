﻿[Library( "dm_smg" ), HammerEntity]
[EditorModel( "weapons/rust_smg/rust_smg.vmdl" )]
[Title( "SMG" ), Category( "Weapons" )]
partial class SMG : HLDMWeapon
{
	public static readonly Model WorldModel = Model.Load( "models/weapons/hl2_smg1/w_hl2_smg1.vmdl" );
	public override string ViewModelPath => "models/weapons/hl2_smg1/v_hl2_smg1.vmdl";

	public override float PrimaryRate => 13.3f;
	public override float SecondaryRate => 1.0f;
	public override int ClipSize => 45;
	public override AmmoType AmmoType => AmmoType.SMG;
	public override AmmoType SecondaryAmmo => AmmoType.SMGGrenade;
	public override float ReloadTime => 1.7f;
	public override int SlotColumn => 2;
	public override int SlotOrder => 1;

	public override void Spawn()
	{
		base.Spawn();

		Model = WorldModel;
		AmmoClip = ClipSize;
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( !TakeAmmo( 1 ) )
		{
			DryFire();

			if ( AvailableAmmo() > 0 )
			{
				Reload();
			}
			return;
		}

		(Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );

		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "hl2_smg1.fire" );

		//
		// Shoot the bullets
		//
		ShootBullet( 0.1f, 1.5f, 12.0f, 3.0f );

	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( Owner is DeathmatchPlayer player )
		{
			if ( player.AmmoCount( SecondaryAmmo ) <= 0 )
			{
				PlaySound( "hl2_ar2.empty" );
			}
			else
			{
				Game.SetRandomSeed( Time.Tick );

				var aim = Owner.AimRay;

				if ( Game.IsServer )
					using ( Prediction.Off() )
					{
						var grenade = new SMGGrenade
						{
							Position = aim.Position + aim.Forward * 5.0f,
							Owner = Owner
						};

						grenade.PhysicsBody.Velocity = aim.Forward * 1000.0f + Owner.Velocity;
					}

				player.SetAnimParameter( "b_attack", true );
				PlaySound( "hl2_smg1.glauncher" );

				player.TakeAmmo( SecondaryAmmo, 1 );
				SecondaryAmmoClip = player.AmmoCount( SecondaryAmmo );
			}
		}
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Game.AssertClient();

		Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
		Particles.Create( "particles/pistol_ejectbrass.vpcf", EffectEntity, "ejection_point" );

		ViewModelEntity?.SetAnimParameter( "fire", true );
		CrosshairLastShoot = 0;
	}

	public override void SimulateAnimator( CitizenAnimationHelper anim )
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Rifle;
		anim.AimBodyWeight = 1.0f;
	}

	public override void RenderCrosshair( in Vector2 center, float lastAttack, float lastReload )
	{
		/*
		var draw = Render.Draw2D;

		var color = Color.Lerp( Color.Red, Color.Yellow, lastReload.LerpInverse( 0.0f, 0.4f ) );
		draw.BlendMode = BlendMode.Lighten;
		draw.Color = color.WithAlpha( 0.2f + CrosshairLastShoot.Relative.LerpInverse( 1.2f, 0 ) * 0.5f );

		// center circle
		{
			var shootEase = Easing.EaseInOut( lastAttack.LerpInverse( 0.1f, 0.0f ) );
			var length = 2.0f + shootEase * 2.0f;
			draw.Circle( center, length );
		}


		draw.Color = draw.Color.WithAlpha( draw.Color.a * 0.2f );

		// outer lines
		{
			var shootEase = Easing.EaseInOut( lastAttack.LerpInverse( 0.2f, 0.0f ) );
			var length = 3.0f + shootEase * 2.0f;
			var gap = 30.0f + shootEase * 50.0f;
			var thickness = 2.0f;

			draw.Line( thickness, center + Vector2.Up * gap + Vector2.Left * length, center + Vector2.Up * gap - Vector2.Left * length );
			draw.Line( thickness, center - Vector2.Up * gap + Vector2.Left * length, center - Vector2.Up * gap - Vector2.Left * length );

			draw.Line( thickness, center + Vector2.Left * gap + Vector2.Up * length, center + Vector2.Left * gap - Vector2.Up * length );
			draw.Line( thickness, center - Vector2.Left * gap + Vector2.Up * length, center - Vector2.Left * gap - Vector2.Up * length );
		}
		*/
	}

}
