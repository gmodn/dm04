using Sandbox;


[Library( "hl2_grenade", Title = "GRENADE" )]
[Hammer.EditorModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" )]
partial class hl2_grenade : BaseDmWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_grenade/v_hl2_grenade.vmdl";
	public override float PrimaryRate => 0.1f;
	public override int ClipSize => 1;
	public override AmmoType AmmoType => AmmoType.Grenade;

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

			PlaySound( "hl2_grenade.throw" );
			ViewModelEntity?.SetAnimParameter( "fire_alt", true );
			player.TakeAmmo( SecondaryAmmo, 1 );

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

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;
	}
}
