﻿[Library( "dm_pistol" ), HammerEntity]
[EditorModel( "weapons/rust_pistol/rust_pistol.vmdl" )]
[Title( "Pistol" ), Category( "Weapons" )]
partial class Pistol : HLDMWeapon
{
	public static readonly Model WorldModel = Model.Load( "models/weapons/hl2_uspmatch/w_hl2_uspmatch.vmdl" );
	public override string ViewModelPath => "models/weapons/hl2_uspmatch/v_hl2_uspmatch.vmdl";

	public override float PrimaryRate => 10.0f;
	public override float SecondaryRate => 2.0f;
	public override AmmoType AmmoType => AmmoType.Pistol;
	public override int ClipSize => 18;
	public override float ReloadTime => 1.4f;
	public override int SlotColumn => 1;
	public override int SlotOrder => 1;

	public override void Spawn()
	{
		base.Spawn();

		Model = WorldModel;
		AmmoClip = ClipSize;
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed( "Attack1" );
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


		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "hl2_uspmatch.fire" );

		//
		// Shoot the bullets
		//
		ShootBullet( 0.05f, 1, 12.0f, 2.0f );

	}

	public override void RenderCrosshair( in Vector2 center, float lastAttack, float lastReload )
	{
		/*
		var draw = Render.Draw2D;

		var shootEase = Easing.EaseIn( lastAttack.LerpInverse( 0.2f, 0.0f ) );
		var color = Color.Lerp( Color.Red, Color.Yellow, lastReload.LerpInverse( 0.0f, 0.4f ) );

		draw.BlendMode = BlendMode.Lighten;
		draw.Color = color.WithAlpha( 0.2f + lastAttack.LerpInverse( 1.2f, 0 ) * 0.5f );

		var length = 8.0f - shootEase * 2.0f;
		var gap = 10.0f + shootEase * 30.0f;
		var thickness = 2.0f;

		draw.Line( thickness, center + Vector2.Left * gap, center + Vector2.Left * (length + gap) );
		draw.Line( thickness, center - Vector2.Left * gap, center - Vector2.Left * (length + gap) );

		draw.Line( thickness, center + Vector2.Up * gap, center + Vector2.Up * (length + gap) );
		draw.Line( thickness, center - Vector2.Up * gap, center - Vector2.Up * (length + gap) );
		*/
	}

}
