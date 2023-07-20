[Library( "dm_crossbow" ), HammerEntity]
[EditorModel( "weapons/rust_crossbow/rust_crossbow.vmdl" )]
[Title( "Crossbow" ), Category( "Weapons" )]
partial class Crossbow : HLDMWeapon
{
	public static readonly Model WorldModel = Model.Load( "models/weapons/hl2_crossbow/w_hl2_crossbow.vmdl" );
	public override string ViewModelPath => "models/weapons/hl2_crossbow/v_hl2_crossbow.vmdl";

	public override float PrimaryRate => 0.52f;
	public override float SecondaryRate => 0f;
	public override float ReloadTime => 3.6f;
	public override int SlotColumn => 3;
	public override int SlotOrder => 2;
	public override AmmoType AmmoType => AmmoType.Crossbow;
	public override int ClipSize => 1;

	[Net, Predicted]
	public bool Zoomed { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		Model = WorldModel;
		AmmoClip = 1;
	}

	public override void ActiveEnd( Entity ent, bool dropped )
	{
		ToggleVMDrawing( To.Single( Owner ), true );
		Zoomed = false;

		base.ActiveEnd( ent, dropped );
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

		if ( Game.IsServer )
		{
			var aim = Owner.AimRay;

			var bolt = new CrossbowBolt();
			bolt.Position = aim.Position;
			bolt.Rotation = Rotation.LookAt( aim.Forward );
			bolt.Owner = Owner;
			bolt.Velocity = aim.Forward * 100;

			Reload();
		}
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		if ( Input.Pressed( "attack2" ) )
			ZoomToggle();
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
			Camera.FieldOfView = Screen.CreateVerticalFieldOfView( 20 );
	}

	public void ZoomToggle()
	{
		Zoomed = !Zoomed;
		ToggleVMDrawing( To.Single( Owner ) );
	}

	/// <summary>
	/// Sets viewmodel drawing with parameter
	/// </summary>
	[ClientRpc]
	void ToggleVMDrawing(bool set)
	{
		Game.AssertClient();

		ViewModelEntity.EnableDrawing = set;
	}

	/// <summary>
	/// Toggles viewmodel drawing
	/// </summary>
	[ClientRpc]
	void ToggleVMDrawing()
	{
		Game.AssertClient();

		ViewModelEntity.EnableDrawing = !ViewModelEntity.EnableDrawing;
	}
}
