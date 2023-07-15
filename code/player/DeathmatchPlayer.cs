using Sandbox;
using System.Runtime.CompilerServices;

public partial class DeathmatchPlayer : Player
{
	TimeSince timeSinceDropped;

	private DamageInfo lastDamage;

	[Net]
	public float Armour { get; set; } = 0;

	[Net]
	public float MaxHealth { get; set; } = 100;

	public bool SupressPickupNotices { get; private set; }

	public int ComboKillCount { get; set; } = 0;
	public TimeSince TimeSinceLastKill { get; set; }

	[Net, Predicted]
	public bool ThirdPerson { get; set; }

	[Net]
	public Vector3 WishVelocity { get; set; }

	public static string PlayerModel { get; set; } = ("models/citizen/citizen.vmdl");

	public DeathmatchPlayer()
	{
		Inventory = new DmInventory( this );
	}

	public override void Respawn()
	{
		SetModel( PlayerModel );

		// I think this might just be our base movement controller, it feels really good.
		Controller = new HLDMWalkController
		{
			sv_walkspeed = 150,
			sv_sprintspeed = 320,
			sv_defaultspeed = 320,
			sv_airaccelerate = 10,
		};

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		Log.Info( $"Player Clothing is marked {dm04_enableclothing}." );

		if ( dm04_enableclothing )
			Clothing.DressEntity( this );
		else

			ClearAmmo();

		SupressPickupNotices = true;

		if ( Game.IsServer )
		{
			Inventory.DeleteContents();

			if ( PlayerSetup.Equipment != null )
				PlayerSetup.Equipment.GiveWeapons( this );
			else
				PlayerSetup.GiveWeaponsDefault( this );
		}

		SupressPickupNotices = false;
		Health = 100;
		Armour = 0;

		base.Respawn();
	}

	[ConCmd.Admin( "impulse" )]
	public static void Impulse( int Value )
	{
		if ( Value == 101 ) 
		{
			var ply = ConsoleSystem.Caller.Pawn as DeathmatchPlayer;

			ply.GiveAmmo( AmmoType.Pistol, 1000 );
			ply.GiveAmmo( AmmoType.Magnum, 1000 );
			ply.GiveAmmo( AmmoType.Buckshot, 1000 );
			ply.GiveAmmo( AmmoType.Crossbow, 1000 );
			ply.GiveAmmo( AmmoType.Grenade, 1000 );
			ply.GiveAmmo( AmmoType.Slam, 1000 );
			ply.GiveAmmo( AmmoType.AR2, 1000 );
			ply.GiveAmmo( AmmoType.AR2Alt, 3 );
			ply.GiveAmmo( AmmoType.SMGGrenade, 3 );

			ply.Inventory.Add( new Pistol() );
			ply.Inventory.Add( new Python() );
			ply.Inventory.Add( new Shotgun() );
			ply.Inventory.Add( new SMG() );
			ply.Inventory.Add( new AR2() );
			ply.Inventory.Add( new Crossbow() );
			ply.Inventory.Add( new GravGun() );
		}

		if ( Value == 203 )
		{
			Log.Info( "TODO: Make this remove the current entity that the player is looking at" );
		}
	}

	[ConCmd.Admin]
	public static void GiveDevTools()
	{
		var ply = ConsoleSystem.Caller.Pawn as DeathmatchPlayer;

		if ( Game.IsServerHost )
		{
			ply.Inventory.Add( new PhysGun() );
		}
		else 
		{
			Log.Warning( $"{ply.Name} Tried to activate a command that they don't have permission to, twist their dick!" );
		}
	}

	[ConCmd.Server( "noclip" )]
	public static void Noclip()
	{
		if ( ConsoleSystem.Caller.Pawn is DeathmatchPlayer basePlayer )
		{
			if ( basePlayer.DevController is NoclipController )
			{
				basePlayer.DevController = null;
			}
			else
			{
				basePlayer.DevController = new NoclipController();
			}
		}
	}

	public override void OnKilled()
	{
		base.OnKilled();

		var coffin = new Coffin();
		coffin.Position = Position + Vector3.Up * 30;
		coffin.Rotation = Rotation;
		coffin.PhysicsBody.Velocity = Velocity + Rotation.Forward * 100;

		coffin.Populate( this );

		Inventory.DeleteContents();

		if ( LastDamage.HasTag( "blast" ) )
		{
			using ( Prediction.Off() )
			{
				var particles = Particles.Create( "particles/gib.vpcf" );
				if ( particles != null )
				{
					particles.SetPosition( 0, Position + Vector3.Up * 40 );
				}
			}
		}
		else
		{
			BecomeRagdollOnClient( LastDamage.Force, LastDamage.BoneIndex );
		}

		Controller = null;
		EnableAllCollisions = false;
		EnableDrawing = false;

		foreach ( var child in Children.OfType<ModelEntity>() )
		{
			child.EnableDrawing = false;
		}

		DeathStat();
	}

	[ClientRpc]
	public void DeathStat()
	{
		Sandbox.Services.Stats.Increment( "deaths", 1 );
		Log.Info( "Client has died, increase death stat by 1." );
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		if ( LifeState != LifeState.Alive )
			return;

		TickPlayerUse();

		DoPlayerAnimation();
		SimulateActiveChild( cl, ActiveChild );

		//
		// If the current weapon is out of ammo and we last fired it over half a second ago
		// lets try to switch to a better wepaon
		//
		if ( ActiveChild is HLDMWeapon weapon && !weapon.IsUsable() && weapon.TimeSincePrimaryAttack > 0.5f && weapon.TimeSinceSecondaryAttack > 0.5f )
		{
			SwitchToBestWeapon();
		}
	}

	public void SwitchToBestWeapon()
	{
		var best = Children.Select( x => x as HLDMWeapon )
			.Where( x => x.IsValid() && x.IsUsable() )
			.OrderByDescending( x => x.BucketWeight )
			.FirstOrDefault();

		if ( best == null ) return;

		ActiveChild = best;
	}

	public override void StartTouch( Entity other )
	{
		if ( timeSinceDropped < 1 ) return;

		base.StartTouch( other );
	}

	public override void FrameSimulate( IClient cl )
	{
		UpdateCamera();
	}

	DamageInfo LastDamage;

	public override void TakeDamage( DamageInfo info )
	{
		if ( LifeState == LifeState.Dead )
			return;

		LastDamage = info;

		if ( info.Hitbox.HasTag( "head" ) )
		{
			info.Damage *= 2.0f;
		}

		this.ProceduralHitReaction( info );

		LastAttacker = info.Attacker;
		LastAttackerWeapon = info.Weapon;

		if ( Game.IsServer && Armour > 0 )
		{
			Armour -= info.Damage;

			if ( Armour < 0 )
			{
				info.Damage = Armour * -1;
				Armour = 0;
			}
			else
			{
				info.Damage = 0;
			}
		}

		if ( info.HasTag( "blast" ) )
		{
			Deafen( To.Single( Client ), info.Damage.LerpInverse( 0, 60 ) );
		}

		if ( Health > 0 && info.Damage > 0 )
		{
			Health -= info.Damage;
			if ( Health <= 0 )
			{
				Health = 0;
				OnKilled();
			}
		}

		if ( info.Attacker is DeathmatchPlayer attacker )
		{
			if ( attacker != this )
			{
				attacker.DidDamage( To.Single( attacker ), info.Position, info.Damage, Health.LerpInverse( 100, 0 ) );
			}

			TookDamage( To.Single( this ), info.Weapon.IsValid() ? info.Weapon.Position : info.Attacker.Position );
		}

		//
		// Add a score to the killer
		//
		if ( LifeState == LifeState.Dead && info.Attacker != null )
		{
			if ( info.Attacker.Client != null && info.Attacker != this )
			{
				info.Attacker.Client.AddInt( "kills" );
			}
		}
	}

	[ClientRpc]
	public void DidDamage( Vector3 pos, float amount, float healthinv )
	{
		Sound.FromScreen( "dm.ui_attacker" )
			.SetPitch( 1 + healthinv * 1 );

		HitIndicator.Current?.OnHit( pos, amount );
	}

	public TimeSince TimeSinceDamage = 1.0f;

	[ClientRpc]
	public void TookDamage( Vector3 pos )
	{
		//DebugOverlay.Sphere( pos, 10.0f, Color.Red, true, 10.0f );

		TimeSinceDamage = 0;
		DamageIndicator.Current?.OnHit( pos );
	}

	[ClientRpc]
	public void PlaySoundFromScreen( string sound )
	{
		Sound.FromScreen( sound );
	}

	public static void InflictDamage()
	{
		if ( Game.LocalPawn is DeathmatchPlayer ply )
		{
			ply.TookDamage( ply.Position + ply.EyeRotation.Forward * 100.0f );
			ply.Health = 0;
		}
	}

	[ConCmd.Server("kill")]
	public static void KillCMD() 
	{
		var target = ConsoleSystem.Caller.Pawn as DeathmatchPlayer;
		if ( target == null ) return;

		target.OnKilled();
	}

	TimeSince timeSinceLastFootstep = 0;

	public override void OnAnimEventFootstep( Vector3 pos, int foot, float volume )
	{
		if ( LifeState != LifeState.Alive )
			return;

		if ( !Game.IsServer )
			return;

		if ( timeSinceLastFootstep < 0.2f )
			return;

		volume *= FootstepVolume();

		timeSinceLastFootstep = 0;

		//DebugOverlay.Box( 1, pos, -1, 1, Color.Red );
		//DebugOverlay.Text( pos, $"{volume}", Color.White, 5 );

		var tr = Trace.Ray( pos, pos + Vector3.Down * 20 )
			.Radius( 1 )
			.Ignore( this )
			.Run();

		if ( !tr.Hit ) return;

		tr.Surface.DoFootstep( this, tr, foot, volume * 10 );
	}

	[ConCmd.Client("cl_playermodel")]
	public static void SetPlayerModel( string PMPath )
	{
		DeathmatchPlayer.PlayerModel = PMPath;
		Log.Info( $"Done! On next spawn your model will be set to: {PMPath}"  );
	}

	public void RenderHud( Vector2 screenSize )
	{
		if ( LifeState != LifeState.Alive )
			return;

		// RenderOverlayTest( screenSize );

		if ( ActiveChild is HLDMWeapon weapon )
		{
			weapon.RenderHud( screenSize );
		}
	}

}
