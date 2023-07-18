﻿[Library( "dm_crossbow" ), HammerEntity]
[EditorModel( "weapons/rust_crossbow/rust_crossbow.vmdl" )]
[Title( "Crossbow" ), Category( "Weapons" )]
partial class Crossbow : HLDMWeapon
{
	public static readonly Model WorldModel = Model.Load( "models/weapons/hl2_crossbow/w_hl2_crossbow.vmdl" );
	public override string ViewModelPath => "models/weapons/hl2_crossbow/v_hl2_crossbow.vmdl";

	public override float PrimaryRate => 0.52f;
	public override float SecondaryRate => 0f;
	public override float ReloadTime => 3.6f;
	public override int Bucket => 3;
	public override AmmoType AmmoType => AmmoType.Crossbow;
	public override int ClipSize => 1;

	[Net, Predicted]
	public bool Zoomed { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		AmmoClip = 1;
		Model = WorldModel;
	}

	public override void AttackPrimary()
	{
		if ( !TakeAmmo( 1 ) )
		{
			DryFire();

			if ( AvailableAmmo() > 0 )
			{
				Reload();
			}
			return;
		}

		ShootEffects();

		// TODO - if zoomed in then instant hit, no travel, 120 damage


		if ( Game.IsServer )
		{
			var aim = Owner.AimRay;
			var bolt = new CrossbowBolt();
			bolt.Position = aim.Position;
			bolt.Rotation = Rotation.LookAt( aim.Forward );
			bolt.Owner = Owner;
			bolt.Velocity = aim.Forward * 100;
			// Have Crossbow AutoReload after firing
			Reload();
		}
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		if ( Input.Pressed( "attack2" ) )
		{ 
			ZoomToggle();
		}
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Game.AssertClient();

		ViewModelEntity?.SetAnimParameter( "fire", true );
		CrosshairLastShoot = 0;
	}

	public override void UpdateCamera()
	{
		if ( Zoomed )
		{
			Camera.FieldOfView = Screen.CreateVerticalFieldOfView( 30 );
		}
	}

	public void ZoomToggle()
	{
		if ( Zoomed )
		{
			Zoomed = false;
		}
		else
		{
			Zoomed = true;
		}
	}

}
