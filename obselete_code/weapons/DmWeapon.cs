﻿using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

partial class BaseDmWeapon : BaseWeapon, IRespawnableEntity
{
	public virtual AmmoType AmmoType => AmmoType.Pistol;

	public virtual AmmoType SecondaryAmmo => AmmoType.None;
	public virtual int ClipSize => 16;
	public virtual float ReloadTime => 3.0f;
	public virtual int Bucket => 1;
	public virtual int BucketWeight => 100;

	public virtual string AmmoIcon => "p";

	public virtual string AltIcon => "z";

	[Net, Predicted]
	public int AmmoClip { get; set; }

	[Net, Predicted]
	public int SecondaryAmmoClip { get; set; }

	[Net, Predicted]
	public TimeSince TimeSinceReload { get; set; }

	[Net, Predicted]
	public bool IsReloading { get; set; }

	[Net, Predicted]
	public TimeSince TimeSinceDeployed { get; set; }


	public PickupTrigger PickupTrigger { get; protected set; }
	public PickupTrigger GravPickupTrigger { get; protected set; }

	public int AvailableAmmo()
	{
		var owner = Owner as DeathmatchPlayer;
		if ( owner == null ) return 0;
		return owner.AmmoCount( AmmoType );
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;
	}
	
	public void gravhitbox()
	{
		GravPickupTrigger = new PickupTrigger();
		GravPickupTrigger.SetTriggerSize( 64 );
		GravPickupTrigger.Parent = this;
		GravPickupTrigger.Position = Position;

	}
	public void gravhitboxremove()
	{
		GravPickupTrigger.Delete();

	}
	public override void StartTouch( Entity other )
	{
		if ( other is DeathmatchPlayer player )
		{
			if(!player.Inventory.Contains(this))
				base.StartTouch( other );

			if ( (player.AmmoCount( AmmoType ) + ClipSize) > player.AmmoLimit[(int)AmmoType] )
			{
				AmmoClip = player.AmmoLimit[(int)AmmoType] - player.AmmoCount( AmmoType );
			}
			else if ( player.AmmoCount( AmmoType ) >= player.AmmoLimit[(int)AmmoType] )
			{
				return;
			}
		}

		base.StartTouch( other );
	}
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );

		PickupTrigger = new PickupTrigger();
		PickupTrigger.SetTriggerSize( 16 );
		PickupTrigger.Parent = this;
		PickupTrigger.Position = Position;
	}

	public override void Reload()
	{
		if ( IsReloading )
			return;

		if ( AmmoClip >= ClipSize )
		{
			ViewModelEntity?.SetAnimParameter( "inspect", true );
			return;
		}	

		TimeSinceReload = 0;

		if ( Owner is DeathmatchPlayer player )
		{
			if ( player.AmmoCount( AmmoType ) <= 0 )
				return;
		}

		IsReloading = true;

		(Owner as AnimatedEntity).SetAnimParameter( "b_reload", true );

		StartReloadEffects();
	}

	public override void Simulate( Client owner ) 
	{
		if ( TimeSinceDeployed < 0.6f )
			return;

		if ( !IsReloading )
		{
			base.Simulate( owner );
		}

		if ( IsReloading && TimeSinceReload > ReloadTime )
		{
			OnReloadFinish();
		}

		//Sprint Animation System
		if ( Owner is DeathmatchPlayer player )
		{
			ViewModelEntity?.SetAnimParameter( "speed", Owner.Velocity.Length.LerpInverse( 0, 320 ) );
			return;
		}	
	}

	public virtual void OnReloadFinish()
	{
		IsReloading = false;

		if ( Owner is DeathmatchPlayer player )
		{
			var ammo = player.TakeAmmo( AmmoType, ClipSize - AmmoClip );
			if ( ammo == 0 )
				return;

			AmmoClip += ammo;
		}
	}

	[ClientRpc]
	public virtual void StartReloadEffects()
	{
		ViewModelEntity?.SetAnimParameter( "reload", true );
		// TODO - player third person model reload
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;
		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();

		//
		// ShootBullet is coded in a way where we can have bullets pass through shit
		// or bounce off shit, in which case it'll return multiple results
		//
		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + Owner.EyeRotation.Forward * 5000 ) )
		{
			tr.Surface.DoBulletImpact( tr );

			if ( !IsServer ) continue;
			if ( !tr.Entity.IsValid() ) continue;

			//
			// We turn predictiuon off for this, so aany exploding effects don't get culled etc
			//
			using ( Prediction.Off() )
			{
				var damage = DamageInfo.FromBullet( tr.EndPosition, Owner.EyeRotation.Forward * 100, 15 )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damage );
			}
		}
	}

	[ClientRpc]
	protected virtual void ShootEffects()
	{
		Host.AssertClient();

		//Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );

		//if ( IsLocalPawn )
		//{
		//	new Sandbox.ScreenShake.Perlin();
		//}
		(Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );
		ViewModelEntity?.SetAnimParameter( "fire", true );
	//we dont have it	///CrosshairPanel?.CreateEvent( "fire" );
	}

	/// <summary>
	/// Shoot a single bullet
	/// </summary>
	public virtual void ShootBullet( float spread, float force, float damage, float bulletSize )
	{
		var forward = Owner.EyeRotation.Forward;
		forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
		forward = forward.Normal;

		//
		// ShootBullet is coded in a way where we can have bullets pass through shit
		// or bounce off shit, in which case it'll return multiple results
		//
		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + forward * 5000, bulletSize ) )
		{
			tr.Surface.DoBulletImpact( tr );

			if ( !IsServer ) continue;
			if ( !tr.Entity.IsValid() ) continue;

			//
			// We turn predictiuon off for this, so any exploding effects don't get culled etc
			//
			using ( Prediction.Off() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 100 * force, damage )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
			}
		}
	}

	public bool TakeAmmo( int amount )
	{
		if ( AmmoClip < amount )
			return false;

		AmmoClip -= amount;
		return true;
	}

	//[ClientRpc]
	//public virtual void DryFire()
	//{

	//}

	public override void CreateViewModel()
	{
		Host.AssertClient();

		if ( string.IsNullOrEmpty( ViewModelPath ) )
			return;

		ViewModelEntity = new DmViewModel();
		ViewModelEntity.Position = Position;
		ViewModelEntity.Owner = Owner;
		ViewModelEntity.EnableViewmodelRendering = true;
		ViewModelEntity.SetModel( ViewModelPath );
	}

	public override void CreateHudElements()
	{
		if ( Local.Hud == null ) return;
	}

	public bool IsUsable()
	{
		//Need to fix secondary check since it's ass
		var owner = Owner as DeathmatchPlayer;
		if ( owner == null ) return true;

		if ( AmmoClip > 0 ) return true;
		if ( owner.AmmoCount( SecondaryAmmo ) > 0 ) return true;
		if ( AmmoType == AmmoType.None ) return true;
		return AvailableAmmo() > 0;
	}

	public override void OnCarryStart( Entity carrier )
	{
		base.OnCarryStart( carrier );

		if ( PickupTrigger.IsValid() )
		{
			PickupTrigger.EnableTouch = false;
		}
	}

	public override void OnCarryDrop( Entity dropper )
	{
		base.OnCarryDrop( dropper );

		if ( PickupTrigger.IsValid() )
		{
			PickupTrigger.EnableTouch = true;
		}
	}
}
