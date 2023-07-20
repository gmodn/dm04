[Library( "dm_crowbar" ), HammerEntity]
[EditorModel( "models/dm_crowbar.vmdl" )]
[Title(  "Crowbar" ), Category( "Weapons" )]
partial class Crowbar : HLDMWeapon
{
	public static Model WorldModel = Model.Load( "models/weapons/hl2_crowbar/w_hl2_crowbar.vmdl" );
	public override string ViewModelPath => "models/weapons/hl2_crowbar/v_hl2_crowbar.vmdl";

	public override float PrimaryRate => 2.0f;
	public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 3.0f;
	public override AmmoType AmmoType => AmmoType.None;
	public override int ClipSize => 0;
	public override int SlotColumn => 0;
	public override int SlotOrder => 1;

	public override void Spawn()
	{
		base.Spawn();

		Model = WorldModel;
		AmmoClip = 0;
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack();
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		// woosh sound
		// screen shake
		PlaySound( "sounds/weapons/hl2_crowbar/hl2_crowbar.swing.sound" );

		Game.SetRandomSeed( Time.Tick );

		var aim = Owner.AimRay;

		var forward = aim.Forward;
		forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * 0.1f;
		forward = forward.Normal;

		foreach ( var tr in TraceBullet( aim.Position, aim.Position + forward * 70, 15 ) )
		{
			tr.Surface.DoBulletImpact( tr );

			if ( !Game.IsServer ) continue;
			if ( !tr.Entity.IsValid() ) continue;

			var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 32, 25 )
				.UsingTraceResult( tr )
				.WithAttacker( Owner )
				.WithWeapon( this );

			tr.Entity.TakeDamage( damageInfo );
		}
		ViewModelEntity?.SetAnimParameter( "attack_has_hit", true );
		ViewModelEntity?.SetAnimParameter( "attack", true );
		ViewModelEntity?.SetAnimParameter( "holdtype_attack", false ? 2 : 1 );
		if ( Owner is DeathmatchPlayer player )
		{
			player.SetAnimParameter( "b_attack", true );
		}
	}

	public override void SimulateAnimator( CitizenAnimationHelper anim )
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Swing;
		anim.AimBodyWeight = 1.0f;
	}
}
