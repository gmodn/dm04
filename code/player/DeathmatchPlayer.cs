﻿public partial class DeathmatchPlayer : Player
{
	TimeSince timeSinceDropped;

	[Net]
	public float Armour { get; set; } = 0;

	[Net]
	public float MaxHealth { get; set; } = 100;

	public bool SupressPickupNotices { get; private set; }

	public int ComboKillCount { get; set; } = 0;
	public TimeSince TimeSinceLastKill { get; set; }
	public List<string> clonedList;
	public DeathmatchPlayer()
	{
		Inventory = new DmInventory( this );
	}
	
	
	public override void Respawn()
	{
		SetModel( Rand.FromList(clonedList));

		Controller = new WalkController
		{
			WalkSpeed = 270,
			SprintSpeed = 300,
			DefaultSpeed = 270,
			AirAcceleration = 10,

		};

		Animator = new StandardPlayerAnimator();

		CameraMode = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		ClearAmmo();
		//Clothing.DressEntity( this );

		SupressPickupNotices = true;

		Inventory.DeleteContents();
		Inventory.Add( new hl2_crowbar() );
		Inventory.Add( new hl2_gravgun() );
		Inventory.Add( new hl2_357(), true );

		GiveAmmo( AmmoType.Pistol, 25 );

		SupressPickupNotices = false;
		Health = 100;
		Armour = 0;

		base.Respawn();
	}

	[ConCmd.Admin]
	public static void GiveAll()
	{
		var ply = ConsoleSystem.Caller.Pawn as DeathmatchPlayer;

		ply.GiveAmmo( AmmoType.Pistol, 1000 );
		ply.GiveAmmo( AmmoType.Buckshot, 1000 );
		ply.GiveAmmo( AmmoType.Crossbow, 1000 );
		ply.GiveAmmo( AmmoType.Grenade, 1000 );
		ply.GiveAmmo( AmmoType.SMG, 1000 );
		ply.GiveAmmo( AmmoType.SMG_grenade, 1000 );
		ply.GiveAmmo( AmmoType.AR2, 1000 );
		ply.GiveAmmo( AmmoType.AR2_ball, 1000 );
		ply.GiveAmmo( AmmoType.SLAM, 1000 );
		ply.GiveAmmo( AmmoType.RPG, 1000 );
		ply.GiveAmmo( AmmoType.Magnum, 1000 );



		ply.Inventory.Add( new hl2_uspmatch() );
		ply.Inventory.Add( new hl2_spas12() );
		ply.Inventory.Add( new hl2_smg1() );
		ply.Inventory.Add( new hl2_crossbow() );
		ply.Inventory.Add( new hl2_ar2() );
		ply.Inventory.Add( new hl2_357() );
		ply.Inventory.Add( new hl2_stunstick() );
		ply.Inventory.Add( new hl2_rpg() );
		ply.Inventory.Add( new hl2_grenade() );
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

		if ( LastDamage.Flags.HasFlag( DamageFlags.Blast ) )
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
			BecomeRagdollOnClient( LastDamage.Force, GetHitboxBone( LastDamage.HitboxIndex ) );
		}

		Controller = null;

		CameraMode = new SpectateRagdollCamera();

		EnableAllCollisions = false;
		EnableDrawing = false;

		foreach ( var child in Children.OfType<ModelEntity>() )
		{
			child.EnableDrawing = false;
		}
	}

	public override void BuildInput( InputBuilder input )
	{
		if ( DeathmatchGame.CurrentState == DeathmatchGame.GameStates.GameEnd )
		{
			input.ViewAngles = input.OriginalViewAngles;
			return;
		};

		base.BuildInput( input );
	}


	public override void Simulate( Client cl )
	{
		if ( DeathmatchGame.CurrentState == DeathmatchGame.GameStates.GameEnd )
			return;

		base.Simulate( cl );
		Simulateflashlight();
		//SimulateGrabbing();
		//
		// Input requested a weapon switch
		//
		if ( Input.ActiveChild != null )
		{
			ActiveChild = Input.ActiveChild;
		}

		if ( LifeState != LifeState.Alive )
			return;

		TickPlayerUse();

		if ( Input.Pressed( InputButton.View ) )
		{
			if ( CameraMode is ThirdPersonCamera )
			{
				CameraMode = new FirstPersonCamera();
			}
			else
			{
				CameraMode = new ThirdPersonCamera();
			}
		}

		if ( Input.Pressed( InputButton.Drop ) )
		{
			var dropped = Inventory.DropActive();
			if ( dropped != null )
			{
				if ( dropped.PhysicsGroup != null )
				{
					dropped.PhysicsGroup.Velocity = Velocity + (EyeRotation.Forward + EyeRotation.Up) * 300;
				}

				timeSinceDropped = 0;
				SwitchToBestWeapon();
			}
		}

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

	public override void PostCameraSetup( ref CameraSetup setup )
	{
		setup.ZNear = 0.1f;

		if ( DeathmatchGame.CurrentState == DeathmatchGame.GameStates.GameEnd )
			return;

		base.PostCameraSetup( ref setup );

		if ( setup.Viewer != null )
		{
			AddCameraEffects( ref setup );
		}
	}

	float walkBob = 0;
	float lean = 0;
	float fov = 0;

	private void AddCameraEffects( ref CameraSetup setup )
	{
		var speed = Velocity.Length.LerpInverse( 0, 320 );
		var forwardspeed = Velocity.Normal.Dot( setup.Rotation.Forward );

		var left = setup.Rotation.Left;
		var up = setup.Rotation.Up;

		if ( GroundEntity != null )
		{
			walkBob += Time.Delta * 25.0f * speed;
		}

		setup.Position += up * MathF.Sin( walkBob ) * speed * 2;
		setup.Position += left * MathF.Sin( walkBob * 0.6f ) * speed * 1;

		// Camera lean
		lean = lean.LerpTo( Velocity.Dot( setup.Rotation.Right ) * 0.01f, Time.Delta * 15.0f );

		var appliedLean = lean;
		appliedLean += MathF.Sin( walkBob ) * speed * 0.3f;
		setup.Rotation *= Rotation.From( 0, 0, appliedLean );

		speed = (speed - 0.7f).Clamp( 0, 1 ) * 3.0f;

		fov = fov.LerpTo( speed * 20 * MathF.Abs( forwardspeed ), Time.Delta * 4.0f );

		setup.FieldOfView += fov;

	}

	DamageInfo LastDamage;

	public override void TakeDamage( DamageInfo info )
	{
		if ( LifeState == LifeState.Dead )
			return;

		LastDamage = info;

		// hack - hitbox group 1 is head
		// we should be able to get this from somewhere (it's pretty specific to citizen though?)
		if ( GetHitboxGroup( info.HitboxIndex ) == 1 )
		{
			info.Damage *= 2.0f;
		}

		this.ProceduralHitReaction( info );

		LastAttacker = info.Attacker;
		LastAttackerWeapon = info.Weapon;

		if ( IsServer && Armour > 0 )
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

		if ( info.Flags.HasFlag( DamageFlags.Blast ) )
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

	[ConCmd.Client]
	public static void InflictDamage()
	{
		if ( Local.Pawn is DeathmatchPlayer ply )
		{
			ply.TookDamage( ply.Position + ply.EyeRotation.Forward * 100.0f );
		}
	}

	TimeSince timeSinceLastFootstep = 0;

	public override void OnAnimEventFootstep( Vector3 pos, int foot, float volume )
	{
		if ( LifeState != LifeState.Alive )
			return;

		if ( !IsServer )
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

	[ConCmd.Admin]
	public static void MapVote()
	{
		var vote = new MapVoteEntity();
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

	void RenderOverlayTest( Vector2 screenSize )
	{
		foreach ( var ent in Entity.FindInSphere( Position, 1500 ) )
		{
			var pos = ent.Position.ToScreen( screenSize );
			if ( !pos.HasValue ) continue;

			var str = $"{ent}";
			Render.Draw2D.FontFamily = "Poppins";
			Render.Draw2D.FontWeight = 1000;
			Render.Draw2D.FontSize = 14;

			var textRect = Render.Draw2D.TextSize( pos.Value, str );

			Render.Draw2D.BlendMode = BlendMode.Normal;
			Render.Draw2D.Color = Color.Black.WithAlpha( 0.7f );
			Render.Draw2D.BoxWithBorder( textRect.Expand( 16, 12 ), 2.0f, Color.Black.WithAlpha( 0.2f ), new Vector4( 4.0f ) );

			Render.Draw2D.Color = Color.White;
			Render.Draw2D.Text( pos.Value, str );
		}
	}

}
