using Sandbox;
using System;

[Library( "dm04_grenade", Title = "Grenade" )]
[Hammer.EditorModel("models/worldmodels/grenade_reference.vmdl")]
[Hammer.EntityTool( "Grenade", "DM:04" )]
partial class Grenade : BaseDmWeapon
{
	public override string ViewModelPath => "models/viewmodels/grenade/grenade_reference.vmdl";

	public override float PrimaryRate => 15.0f;
	public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 1f;

	public override AmmoType AmmoType => AmmoType.Grenade;
	public override int ClipSize => 1;

	public override int Bucket => 4;

	public override void Spawn()
	{
		base.Spawn();

		SetModel("models/worldmodels/grenade_reference.vmdl");
		AmmoClip = 1;
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed( InputButton.Attack1 );
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( !TakeAmmo( 1 ) )
		{
			DryFire();
			return;
		}

		(Owner as AnimEntity).SetAnimBool( "b_attack", true );

		ShootGrenade();

		ViewModelEntity?.SetAnimBool( "throw", true );
	}

	public override void AttackSecondary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( !TakeAmmo( 1 ) )
		{
			DryFire();
			return;
		}

		AltToss();

		ViewModelEntity?.SetAnimBool( "lob", true );
	}

	public override void Reload()
	{
		//NOT unused
		base.Reload();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if (AmmoClip == 0) 
		{
			Reload();
		}
	}

	private void ShootGrenade()
	{
		if ( Host.IsClient )
			return;

		var grenade = new Prop
		{
			Position = Owner.EyePos + Owner.EyeRot.Forward * 50,
			Rotation = Owner.EyeRot,
		};

		//TODO: Should be replaced with an actual grenade model
		grenade.SetModel("models/worldmodels/grenade_reference.vmdl");
		grenade.Velocity = Owner.EyeRot.Forward * 1000;

		grenade.ExplodeAsync( 4f );

	}

	private void AltToss()
	{
		if ( Host.IsClient )
			return;

		var grenade = new Prop
		{
			Position = Owner.EyePos + Owner.EyeRot.Forward * 50,
			Rotation = Owner.EyeRot,
		};

		//TODO: Should be replaced with an actual grenade model
		grenade.SetModel("models/worldmodels/grenade_reference.vmdl");
		grenade.Velocity = Owner.EyeRot.Forward * 400;

		grenade.ExplodeAsync( 4f );

	}
	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 4 );
		anim.SetParam( "aimat_weight", 1.0f );
		anim.SetParam( "holdtype_handedness", 1 );
	}

}
