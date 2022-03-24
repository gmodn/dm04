using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

partial class DeathmatchPlayer
{
	[Net]
	public IList<int> Ammo { get; set; }

	//In order using AmmoType
	public int[] AmmoLimit = new int[13]
	{
		//None
		0,
		//Pistol
		150,
		//357
		12,
		//SMG
		225,
		//SMG Nades
		3,
		//AR2
		60,
		//AR2 Ball
		3,
		//Shotgun
		30,
		//Crossbow
		10,
		//RPG Rockets
		3,
		//Grenades
		5,
		//SLAM
		5,
		//bugbait
		99,
	};

	public void ClearAmmo()
	{
		Ammo.Clear();
	}

	public int AmmoCount( AmmoType type )
	{
		var iType = (int)type;
		if ( Ammo == null ) return 0;
		if ( Ammo.Count <= iType ) return 0;

		return Ammo[(int)type];
	}

	public bool SetAmmo( AmmoType type, int amount )
	{
		var iType = (int)type;
		if ( !Host.IsServer ) return false;
		if ( Ammo == null ) return false;

		while ( Ammo.Count <= iType )
		{
			Ammo.Add( 0 );
		}

		Ammo[(int)type] = amount;
		return true;
	}

	public bool GiveAmmo( AmmoType type, int amount )
	{
		if ( !Host.IsServer ) return false;
		if ( Ammo == null ) return false;

		SetAmmo( type, AmmoCount( type ) + amount );
		return true;
	}

	public int TakeAmmo( AmmoType type, int amount )
	{
		if ( Ammo == null ) return 0;

		var available = AmmoCount( type );
		amount = Math.Min( available, amount );

		SetAmmo( type, available - amount );
		return amount;
	}
}
public enum AmmoType
{
	None,
	Pistol,
	Magnum,
	SMG,
	SMG_grenade,
	AR2,
	AR2_ball,
	Buckshot,
	Crossbow,
	RPG,
	Grenade,
	SLAM,
	Bugbait,
	Egon
}
