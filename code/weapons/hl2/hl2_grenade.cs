using Sandbox;


[Library( "hl2_grenade", Title = "GRENADE" )]
[Hammer.EditorModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" )]
partial class hl2_grenade : BaseDmWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_grenade/v_hl2_grenade.vmdl";
	public override float PrimaryRate => 1f;
	public override int ClipSize => 1;
	public override AmmoType AmmoType => AmmoType.Grenade;
	public override string AmmoIcon => "v";

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
		return base.CanSecondaryAttack() && Input.Pressed( InputButton.Attack2 );
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
			ViewModelEntity?.SetAnimParameter( "fire", true );
			player.TakeAmmo( AmmoType, 1 );

			if ( IsServer )
				using ( Prediction.Off() )
				{
					var grenade = new hl2_grenadethrown();
					grenade.Position = Owner.EyePosition;
					grenade.Rotation += Owner.EyeRotation;
					grenade.Owner = Owner;
					grenade.Velocity = Owner.EyeRotation.Forward * 1000;
				}
		}
	}
	public override void AttackSecondary()
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
			ViewModelEntity?.SetAnimParameter( "fire_alt", true );
			player.TakeAmmo( AmmoType, 1 );

			if ( IsServer )
				using ( Prediction.Off() )
				{
					var grenade = new hl2_grenadethrown();
					grenade.Position = Owner.EyePosition;
					grenade.Rotation += Owner.EyeRotation;
					grenade.Owner = Owner;
					grenade.Velocity = Owner.EyeRotation.Forward * 400;
				}
		}
	}
	[ClientRpc]
	protected override void ShootEffects()
	{
		Host.AssertClient();
		(Owner as AnimEntity).SetAnimParameter( "b_attack", true );
		
		CrosshairPanel?.CreateEvent( "fire" );

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
		if ( Input.Pressed( InputButton.Reload ) )
		{
			ViewModelEntity?.SetAnimParameter( "inspect", true );
		}
	}
}
