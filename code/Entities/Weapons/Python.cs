using Sandbox.Utility;

[Library( "dm_357" ), HammerEntity]
[EditorModel( "weapons/rust_pistol/rust_pistol.vmdl" )]
[Title( ".357 Magnum Revolver" ), Category( "Weapons" )]
partial class Python : HLDMWeapon
{
	public static readonly Model WorldModel = Model.Load( "models/weapons/hl2_357/w_hl2_357.vmdl" );
	public override string ViewModelPath => "models/weapons/hl2_357/v_hl2_357.vmdl";

	public override float PrimaryRate => 1.3f;
	public override float ReloadTime => 4f;
	public override int ClipSize => 6;
	public override AmmoType AmmoType => AmmoType.Magnum;

	public override int SlotColumn => 1;
	public override int SlotOrder => 2;

	[Net, Predicted]
	public bool Zoomed { get; set; }

	private float? LastFov;
	private float? LastViewmodelFov;

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
		base.AttackPrimary();

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
		PlaySound( "hl2_357.fire" );

		//
		// Shoot the bullets
		//
		ShootBullet( 0.01f, 1.5f, 40.0f, 2.0f );
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );
	}

	public override void RenderCrosshair( in Vector2 center, float lastAttack, float lastReload )
	{
		var shootEase = Easing.EaseInOut( lastAttack.LerpInverse( 0.3f, 0.0f ) );
		var color = Color.Lerp( Color.Red, Color.Yellow, lastReload.LerpInverse( 0.0f, 0.4f ) );

		//Graphics.BlendMode = BlendMode.Lighten;
		//Graphics.Color = color.WithAlpha( 0.2f + CrosshairLastShoot.Relative.LerpInverse( 1.2f, 0 ) * 0.5f );

		var length = 3.0f + shootEase * 5.0f;

		//Graphics.Ring( center, length, length - 3.0f );
	}

}
