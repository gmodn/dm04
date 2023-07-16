[Library( "dm_grenade" ), HammerEntity]
[EditorModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" )]
[Title( "Grenade" ), Category( "Weapons" )]
partial class GrenadeWeapon : HLDMWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_grenade/v_hl2_grenade.vmdl";

	public override float PrimaryRate => 1.0f;
	public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 1f;
	public override AmmoType AmmoType => AmmoType.Grenade;
	public override int ClipSize => 1;
	public override int Bucket => 4;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" );
		AmmoClip = 1;
	}

	public override bool CanPrimaryAttack()
	{
		return Input.Released( InputButton.PrimaryAttack );
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( Owner is not DeathmatchPlayer player ) return;

		if ( !TakeAmmo( 1 ) )
		{
			Reload();
			return;
		}

		var aim = Owner.AimRay;
		PlaySound( "sounds/weapons/hl2_grenade/hl2_grenade.throw.sound" );

		Game.SetRandomSeed( Time.Tick );


		if ( Game.IsServer )
			using ( Prediction.Off() )
			{
				var grenade = new GrenadeThrown
				{
					Position = aim.Position + aim.Forward * 3.0f,
					Owner = Owner
				};

				grenade.PhysicsBody.Velocity = aim.Forward * 600.0f + Owner.Rotation.Up * 200.0f + Owner.Velocity;
			}

		player.SetAnimParameter( "b_attack", true );

		Reload();

		if ( Game.IsServer && AmmoClip == 0 && player.AmmoCount( AmmoType.Grenade ) == 0 )
		{
			Delete();
			player.SwitchToBestWeapon();
		}
	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( Owner is not DeathmatchPlayer player ) return;

		if ( !TakeAmmo( 1 ) )
		{
			Reload();
			return;
		}

		var aim = Owner.AimRay;
		PlaySound( "sounds/weapons/hl2_grenade/hl2_grenade.throw.sound" );

		Game.SetRandomSeed( Time.Tick );


		if ( Game.IsServer )
			using ( Prediction.Off() )
			{
				var grenade = new GrenadeThrown
				{
					Position = aim.Position + aim.Forward * 3.0f,
					Owner = Owner
				};

				grenade.PhysicsBody.Velocity = aim.Forward * 200.0f + Owner.Rotation.Up * 100.0f + Owner.Velocity;
			}

		player.SetAnimParameter( "b_attack", true );

		Reload();

		if ( Game.IsServer && AmmoClip == 0 && player.AmmoCount( AmmoType.Grenade ) == 0 )
		{
			Delete();
			player.SwitchToBestWeapon();
		}
	}

	public override void SimulateAnimator( CitizenAnimationHelper anim )
	{
		anim.HoldType = CitizenAnimationHelper.HoldTypes.Swing;
		anim.AimBodyWeight = 1.0f;
	}
}
