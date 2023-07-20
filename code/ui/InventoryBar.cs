﻿using Sandbox.UI;
using Sandbox.Diagnostics;

/// <summary>
/// The main inventory panel, top left of the screen.
/// </summary>
public class InventoryBar : Panel
{
	List<InventoryColumn> columns = new();
	List<HLDMWeapon> Weapons = new();

	public bool IsOpen;
	HLDMWeapon SelectedWeapon;

	public InventoryBar()
	{
		for ( int i = 0; i < 6; i++ )
		{
			var icon = new InventoryColumn( i, this );
			columns.Add( icon );
		}
	}

	public override void Tick()
	{
		base.Tick();

		SetClass( "active", IsOpen );

		var player = Game.LocalPawn as DeathmatchPlayer;
		if ( player == null ) return;

		Weapons.Clear();
		Weapons.AddRange( player.Children.Select( x => x as HLDMWeapon ).Where( x => x.IsValid() && x.IsUsable() ) );

		foreach ( var weapon in Weapons )
		{
			columns[weapon.SlotColumn].UpdateWeapon( weapon );
		}
	}

	bool becameOpen = false;

	/// <summary>
	/// IClientInput implementation, calls during the client input build.
	/// You can both read and write to input, to affect what happens down the line.
	/// </summary>
	[GameEvent.Client.BuildInput]
	public void ProcessClientInput()
	{
		bool wantOpen = IsOpen;
		var localPlayer = Game.LocalPawn as DeathmatchPlayer;

		// If we're not open, maybe this input has something that will 
		// make us want to start being open?
		wantOpen = wantOpen || Input.MouseWheel != 0;
		wantOpen = wantOpen || Input.Pressed( "slot1" );
		wantOpen = wantOpen || Input.Pressed( "slot2" );
		wantOpen = wantOpen || Input.Pressed( "slot3" );
		wantOpen = wantOpen || Input.Pressed( "slot4" );
		wantOpen = wantOpen || Input.Pressed( "slot5" );
		wantOpen = wantOpen || Input.Pressed( "slot6" );

		if ( Weapons.Count == 0 )
		{
			IsOpen = false;
			return;
		}

		// We're not open, but we want to be
		if ( IsOpen != wantOpen )
		{
			SelectedWeapon = localPlayer?.ActiveChild as HLDMWeapon;
			IsOpen = true;
		}

		// Not open fuck it off
		if ( !IsOpen ) return;

		// Fire pressed when we're open - select the weapon and close.
		if ( Input.Down( "Attack1" ) )
		{
			Input.SetAction( "Attack1", false );
			localPlayer.ActiveChildInput = SelectedWeapon;
			IsOpen = false;
			Sound.FromScreen( "sounds/ui/wpn_select.sound" );
			becameOpen = false;
			return;
		}

		Log.Info( becameOpen );

		var sortedWeapons = Weapons.OrderBy( x => x.SlotOrder )
			.OrderBy( x => x.SlotColumn )
			.ToList();

		// get our current index
		var oldSelected = SelectedWeapon;
		int SelectedIndex = sortedWeapons.IndexOf( SelectedWeapon );
		SelectedIndex = SlotPressInput( SelectedIndex, sortedWeapons );

		if ( Input.MouseWheel != 0 && !becameOpen )
			becameOpen = true;

		// forward if mouse wheel was pressed
		SelectedIndex -= Input.MouseWheel;
		SelectedIndex = SelectedIndex.UnsignedMod( Weapons.Count );

		SelectedWeapon = sortedWeapons[SelectedIndex];

		for ( int i = 0; i < 6; i++ )
		{
			columns[i].TickSelection( SelectedWeapon );
		}

		Input.MouseWheel = 0;

		if ( oldSelected != SelectedWeapon )
		{
			Sound.FromScreen( "sounds/ui/wpn_moveselect.sound" );
		}
	}

	int SlotPressInput( int SelectedIndex, List<HLDMWeapon> sortedWeapons )
	{
		var columninput = -1;

		if ( Input.Pressed( "slot1" ) ) columninput = 0;
		if ( Input.Pressed( "slot2" ) ) columninput = 1;
		if ( Input.Pressed( "slot3" ) ) columninput = 2;
		if ( Input.Pressed( "slot4" ) ) columninput = 3;
		if ( Input.Pressed( "slot5" ) ) columninput = 4;
		if ( Input.Pressed( "slot6" ) ) columninput = 5;

		if ( columninput == -1 ) return SelectedIndex;

		// Are we already selecting a weapon with this column?
		var firstOfColumn = sortedWeapons.Where( x => x.SlotColumn == columninput )
			.Where(s => s.SlotOrder == 1)
			.FirstOrDefault();

		if ( !becameOpen )
		{
			becameOpen = true;

			if( SelectedWeapon.SlotColumn == columninput )
			{
				SelectedIndex = sortedWeapons.IndexOf( firstOfColumn );
				return SelectedIndex;
			}
		}

		if ( SelectedWeapon.IsValid() && SelectedWeapon.SlotColumn == columninput )
		{
			return NextInBucket( sortedWeapons );
		}

		if ( firstOfColumn == null )
		{
			// DOOP sound
			return SelectedIndex;
		}

		return sortedWeapons.IndexOf( firstOfColumn );
	}

	int NextInBucket( List<HLDMWeapon> sortedWeapons )
	{
		Assert.NotNull( SelectedWeapon );

		HLDMWeapon first = null;
		HLDMWeapon prev = null;
		foreach ( var weapon in sortedWeapons.Where( x => x.SlotColumn == SelectedWeapon.SlotColumn ) )
		{
			if ( first == null ) first = weapon;
			if ( prev == SelectedWeapon ) return sortedWeapons.IndexOf( weapon );
			prev = weapon;
		}

		return sortedWeapons.IndexOf( first );
	}
}
