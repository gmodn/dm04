using Sandbox;


[Library( "hl2_bugbait", Title = "BugBait" )]
[EditorModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" )]
partial class hl2_bugbait : HLDMWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_bugbait/v_hl2_bugbait.vmdl";
	public override float PrimaryRate => 1f;
	public override int ClipSize => 1;
	public override AmmoType AmmoType => AmmoType.Bugbait;
	//public override string AmmoIcon => "j";

	public override int Bucket => 4;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/worldmodels/w_bugbait_reference.vmdl" );
	}

	public override bool CanReload()
	{
		return false;
	}

	public override void AttackPrimary()
	{
		if ( Owner is DeathmatchPlayer player )
		{
			//TimeSincePrimaryAttack = 0;

			//if (TimeSinceSecondaryAttack < 0.5) return;

			//if ( !TakeAmmo( 1 ) )
			//{
			//	Reload();
			//	return;
			//}
			ShootEffects();
			PlaySound( "hl2_grenade.throw" );
			player.TakeAmmo( SecondaryAmmo, 1 );

			if ( IsServer )
				using ( Prediction.Off() )
				{
					var grenade = new hl2_bugbaitthrown();
					grenade.Position = Owner.EyePosition;
					grenade.Rotation += Owner.EyeRotation;
					grenade.Owner = Owner;
					grenade.Velocity = Owner.EyeRotation.Forward * 1000;
				}
		}
	}
	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();
		(Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );
		ViewModelEntity?.SetAnimParameter( "fire", true );
		//CrosshairPanel?.CreateEvent( "fire" );

		//if ( IsLocalPawn )
		//{
		//	new Sandbox.ScreenShake.Perlin();
		//}


	}
	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;
	}
	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 3 ); // TODO this is shit
												//anim.SetAnimParameter( "aimat_weight", 1.0f );
		if ( Input.Pressed( InputButton.SecondaryAttack ) )
		{
			ViewModelEntity?.SetAnimParameter( "inspect", true );
		}
	}
}
