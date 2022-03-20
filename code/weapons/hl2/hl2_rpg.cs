using Sandbox;

[Library( "hl2_rpg", Title = "RPG" )]
[Hammer.EditorModel( "models/weapons/hl2_rpg/w_hl2_rpg.vmdl" )]
[Hammer.EntityTool( "RPG", "DM:04" )]
partial class hl2_rpg : BaseDmWeapon
{ 
	public override string ViewModelPath => "models/weapons/hl2_rpg/v_hl2_rpg.vmdl";

	public override float PrimaryRate => 0.52f;
	public override float ReloadTime => 2.8f;
	public override int Bucket => 4;
	public override AmmoType AmmoType => AmmoType.RPG;
	public override string AmmoIcon => "x";
	public override int ClipSize => 1;
	public bool targeting = true;

	public override void Spawn()
	{
		base.Spawn();
		
		SetModel( "models/weapons/hl2_rpg/w_hl2_rpg.vmdl" );

		AmmoClip = 1;
	}

	public override bool CanSecondaryAttack()
	{
		return base.CanSecondaryAttack() && Input.Pressed( InputButton.Attack2 );
	}

	public override void AttackPrimary()
	{
		if ( !TakeAmmo( 1 ) )
		{
			Reload();
			return;
		}

		PlaySound( "hl2_rpg.fire" );
		ShootEffects();

		if ( IsServer )
		using ( Prediction.Off() )
		{
			var missle = new hl2_rpgmissile();
			missle.Position = Owner.EyePosition;
			missle.Rotation = Owner.EyeRotation;
			missle.Owner = Owner;
			missle.Velocity = Owner.EyeRotation.Forward * 1000;
		}
	}

	public override void AttackSecondary()
	{
		if ( targeting ) targeting = false;
		else targeting = true;

		PlaySound( "hl2_uspmatch.empty" );
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;
	}

	public override void StartReloadEffects()
	{
		ViewModelEntity?.SetAnimParameter( "reload", true );
		PlaySound( "hl2_rpg.reload" );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 2 ); // TODO this is shit
		//anim.SetAnimParameter( "aimat_weight", 1.0f );
	}
}
