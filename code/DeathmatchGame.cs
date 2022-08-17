global using Sandbox;
global using SandboxEditor;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;

/// <summary>
/// This is the heart of the gamemode. It's responsible
/// for creating the player and stuff.
/// </summary>
partial class DeathmatchGame : Game
{
	StandardPostProcess postProcess;
	public List<string> Playermodels = new List<string>();
	public DeathmatchGame()
	{
		//
		// Create the HUD entity. This is always broadcast to all clients
		// and will create the UI panels clientside.
		//
		if ( IsServer )
		{
			_ = GameLoopAsync();
		}

		if ( IsClient )
		{
			postProcess = new StandardPostProcess();
			PostProcess.Add( postProcess );

			_ = new DeathmatchHud();
		}
	}

	[Event.Hotload]
	public void HotloadDeathmatch()
	{
		if(IsClient)
		{
			_ = new DeathmatchHud();
		}
	}

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();
		
		listplayermodels();
		ItemRespawn.Init();
	}
	public void listplayermodels()
	{
		Playermodels.Add( "models/playermodels/female/female_02/female_02.vmdl" );
		Playermodels.Add( "models/playermodels/coomer/drcoomer.vmdl" );
		Playermodels.Add( "models/playermodels/female/female_01/female_01.vmdl" );
		
	}
	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );

		var player = new DeathmatchPlayer();
		player.clonedList = new List<string>( Playermodels );
		player.UpdateClothes( cl );
		player.Respawn();

		cl.Pawn = player;
	}

	public override void MoveToSpawnpoint( Entity pawn )
	{
		var spawnpoint = Entity.All
								.OfType<SpawnPoint>()
								.OrderByDescending( x => SpawnpointWeight( pawn, x ) )
								.ThenBy( x => Guid.NewGuid() )
								.FirstOrDefault();

		//Log.Info( $"chose {spawnpoint}" );

		if ( spawnpoint == null )
		{
			Log.Warning( $"Couldn't find spawnpoint for {pawn}!" );
			return;
		}

		pawn.Transform = spawnpoint.Transform;
	}

	/// <summary>
	/// The higher the better
	/// </summary>
	public float SpawnpointWeight( Entity pawn, Entity spawnpoint )
	{
		float distance = 0;

		foreach ( var client in Client.All )
		{
			if ( client.Pawn == null ) continue;
			if ( client.Pawn == pawn ) continue;
			if ( client.Pawn.LifeState != LifeState.Alive ) continue;

			var spawnDist = (spawnpoint.Position - client.Pawn.Position).Length;
			distance = MathF.Max( distance, spawnDist );
		}

		//Log.Info( $"{spawnpoint} is {distance} away from any player" );

		return distance;
	}

	[ClientRpc]
	public override void OnKilledMessage( long leftid, string left, long rightid, string right, string method )
	{
		Sandbox.UI.KillFeed.Current?.AddEntry( leftid, left, rightid, right, method );
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		postProcess.Sharpen.Enabled = false;
		postProcess.Sharpen.Strength = 0.5f;

		postProcess.FilmGrain.Enabled = false;
		postProcess.FilmGrain.Intensity = 0.2f;
		postProcess.FilmGrain.Response = 1;

		postProcess.Vignette.Enabled = true;
		postProcess.Vignette.Intensity = 1.0f;
		postProcess.Vignette.Roundness = 1.5f;
		postProcess.Vignette.Smoothness = 0.5f;
		postProcess.Vignette.Color = Color.Black;

		postProcess.Saturate.Enabled = true;
		postProcess.Saturate.Amount = 1;

		postProcess.Blur.Enabled = false;

		Audio.SetEffect( "core.player.death.muffle1", 0 );

		if ( Local.Pawn is DeathmatchPlayer localPlayer )
		{
			var timeSinceDamage = localPlayer.TimeSinceDamage.Relative;
			var damageUi = timeSinceDamage.LerpInverse( 0.25f, 0.0f, true ) * 0.3f;
			if ( damageUi > 0 )
			{
				postProcess.Saturate.Amount -= damageUi;
				postProcess.Vignette.Color = Color.Lerp( postProcess.Vignette.Color, Color.Red, damageUi );
				postProcess.Vignette.Intensity += damageUi;
				postProcess.Vignette.Smoothness += damageUi;
				postProcess.Vignette.Roundness += damageUi;

				postProcess.Blur.Enabled = true;
				postProcess.Blur.Strength = damageUi * 0.5f;
			}


			var healthDelta = localPlayer.Health.LerpInverse( 0, 100.0f, true );

			healthDelta = MathF.Pow( healthDelta, 0.5f );

			postProcess.Vignette.Color = Color.Lerp( postProcess.Vignette.Color, Color.Red, 1 - healthDelta );
			postProcess.Vignette.Intensity += (1 - healthDelta) * 0.5f;
			postProcess.Vignette.Smoothness += (1 - healthDelta);
			postProcess.Vignette.Roundness += (1 - healthDelta) * 0.5f;
			postProcess.Saturate.Amount *= healthDelta;
			postProcess.FilmGrain.Intensity += (1 - healthDelta) * 0.5f;

			Audio.SetEffect( "core.player.death.muffle1", 1 - healthDelta, velocity: 2.0f );

		}


		if ( CurrentState == GameStates.Warmup )
		{
			postProcess.FilmGrain.Intensity = 0.4f;
			postProcess.FilmGrain.Response = 0.5f;

			postProcess.Saturate.Amount = 0;
		}
	}

	public static void Explosion( Entity weapon, Entity owner, Vector3 position, float radius, float damage, float forceScale )
	{
		// Effects
		Sound.FromWorld( "rust_pumpshotgun.shootdouble", position );
		Particles.Create( "particles/explosion/barrel_explosion/explosion_barrel.vpcf", position );

		// Damage, etc
		var overlaps = Entity.FindInSphere( position, radius );

		foreach ( var overlap in overlaps )
		{
			if ( overlap is not ModelEntity ent || !ent.IsValid() )
				continue;

			if ( ent.LifeState != LifeState.Alive )
				continue;

			if ( !ent.PhysicsBody.IsValid() )
				continue;

			if ( ent.IsWorld )
				continue;

			var targetPos = ent.PhysicsBody.MassCenter;

			var dist = Vector3.DistanceBetween( position, targetPos );
			if ( dist > radius )
				continue;

			var tr = Trace.Ray( position, targetPos )
				.Ignore( weapon )
				.WorldOnly()
				.Run();

			if ( tr.Fraction < 0.98f )
				continue;

			var distanceMul = 1.0f - Math.Clamp( dist / radius, 0.0f, 1.0f );
			var dmg = damage * distanceMul;
			var force = (forceScale * distanceMul) * ent.PhysicsBody.Mass;
			var forceDir = (targetPos - position).Normal;

			var damageInfo = DamageInfo.Explosion( position, forceDir * force, dmg )
				.WithWeapon( weapon )
				.WithAttacker( owner );

			ent.TakeDamage( damageInfo );
		}
	}

	public override void RenderHud()
	{
		var localPawn = Local.Pawn as DeathmatchPlayer;
		if ( localPawn == null ) return;

		//
		// scale the screen using a matrix, so the scale math doesn't invade everywhere
		// (other than having to pass the new scale around)
		//

		var scale = Screen.Height / 1080.0f;
		var screenSize = Screen.Size / scale;
		var matrix = Matrix.CreateScale( scale );

		using ( Render.Draw2D.MatrixScope( matrix ) )
		{
			localPawn.RenderHud( screenSize );
		}
	}
	[ConCmd.Admin( "impulse" )]
	public static void Impulse( int impulse )
	{
		var caller = ConsoleSystem.Caller.Pawn as DeathmatchPlayer;

		if ( caller == null )
			return;

		if ( impulse == 101 )
		{
			caller.Inventory.Add( new hl2_crowbar() );
			caller.Inventory.Add( new hl2_stunstick() );
			caller.Inventory.Add( new hl2_gravgun() );
			caller.Inventory.Add( new hl2_uspmatch() );
			caller.Inventory.Add( new hl2_357() );
			caller.Inventory.Add( new hl2_smg1() );
			caller.Inventory.Add( new hl2_ar2() );
			caller.Inventory.Add( new hl2_spas12() );
			caller.Inventory.Add( new hl2_crossbow() );
			caller.Inventory.Add( new hl2_rpg() );
			caller.Inventory.Add( new hl2_grenade() );
			caller.Inventory.Add( new hl2_slam() );
			caller.Inventory.Add( new hl2_gauss() );
			caller.Inventory.Add( new hl2_egon() );
			caller.Inventory.Add( new hl2_bugbait() );
			caller.Inventory.Add( new Tripmine() );

			caller.SetAmmo( AmmoType.Pistol, 150 );
			caller.SetAmmo( AmmoType.Magnum, 12 );

			caller.SetAmmo( AmmoType.SMG, 225 );
			caller.SetAmmo( AmmoType.SMG_grenade, 3 );

			caller.SetAmmo( AmmoType.AR2, 60 );
			caller.SetAmmo( AmmoType.AR2_ball, 3 );

			caller.SetAmmo( AmmoType.Buckshot, 30 );
			caller.SetAmmo( AmmoType.Crossbow, 4 );
			caller.SetAmmo( AmmoType.Crossbow, 4 );
			caller.SetAmmo( AmmoType.Egon, 300 );

			caller.SetAmmo( AmmoType.RPG, 3 );
			caller.SetAmmo( AmmoType.Grenade, 5 );
			caller.SetAmmo( AmmoType.SLAM, 5 );

			caller.SetAmmo( AmmoType.Bugbait, 95 );
		}
	}
}
