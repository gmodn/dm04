using Sandbox;

[Library( "hl2_crossbow", Title = "CROSSBOW" )]
[Hammer.EditorModel( "models/weapons/hl2_crossbow/w_hl2_crossbow.vmdl" )]
[Hammer.EntityTool( "Crossbow", "DM:04" )]
partial class hl2_crossbow : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/weapons/hl2_crossbow/v_hl2_crossbow.vmdl";

	public override float PrimaryRate => 0.52f;
	public override float ReloadTime => 3.8f;
	public override int Bucket => 3;
	public override AmmoType AmmoType => AmmoType.Crossbow;
	public override string AmmoIcon => "w";
	public override int ClipSize => 1;

	float glow = 0f;

	[Net]
	public bool Zoomed { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		
		SetModel( "models/weapons/hl2_crossbow/w_hl2_crossbow.vmdl" );

		AmmoClip = 1;
	}

	public override void AttackPrimary()
	{
		if ( !TakeAmmo( 1 ) )
		{
			Reload();
			return;
		}

		ShootEffects();

		Reload();

		if ( IsServer )
		using ( Prediction.Off() )
		{
			var bolt = new hl2_crossbowbolt();
			bolt.Position = Owner.EyePosition;
			bolt.Rotation = Owner.EyeRotation;
			bolt.Owner = Owner;
			bolt.Velocity = Owner.EyeRotation.Forward * 3000;
		}
	}
	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;

		if ( AmmoClip < 1 ) 
		{
			ViewModelEntity?.SetAnimParameter( "loaded", false );
		}
		else ViewModelEntity?.SetAnimParameter( "loaded", true );
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		Zoomed = Input.Down( InputButton.Attack2 );

		if ( AmmoClip < 1 )
			if ( TimeSinceDeployed > 2.2f )
				Reload();
	}

	public override void PostCameraSetup( ref CameraSetup camSetup )
	{
		base.PostCameraSetup( ref camSetup );

		if ( Zoomed )
		{
			camSetup.FieldOfView = 20;
		}
	}



	public override void BuildInput( InputBuilder owner ) 
	{
		if ( Zoomed )
		{
			owner.ViewAngles = Angles.Lerp( owner.OriginalViewAngles, owner.ViewAngles, 0.2f );
		}
	}

	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();
		///doesnt work when you use the fire event for animgraph so jsut leave it without one it looks fine without it
		if ( Owner == Local.Pawn )
		{
			new Sandbox.ScreenShake.Perlin( 0.5f, 4.0f, 1.0f, 0.5f );
		}

		ViewModelEntity?.SetAnimParameter( "fire", true );

		glow = 4f;
		CrosshairPanel?.CreateEvent( "fire" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 2 ); // TODO this is shit
		//anim.SetAnimParameter( "aimat_weight", 1.0f );

		//temp:
		if ( AmmoClip == 1 ) glow = 5;
		else glow = 0;

		ViewModelEntity?.SceneObject.Attributes.Set( "glow", glow );
	}

	//how the FUCK do I get this to work
	public override void OnAnimEventGeneric( string name, int intData, float floatData, Vector3 vectorData, string stringData )
	{
		ViewModelEntity?.SceneObject.Attributes.Set( "glow", 10 );
	}
}
