using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

partial class DeathmatchPlayer
{
	[Net]
	public IList<int> Ammo { get; set; }

	public IList<int> Alt { get; set; }

	public void ClearAmmo()
	{
		Ammo.Clear();
	}

	public void ClearAlt()
	{
		Alt.Clear();
	}

	public int AmmoCount( AmmoType type )
	{
		var iType = (int)type;
		if ( Ammo == null ) return 0;
		if ( Ammo.Count <= iType ) return 0;

		return Ammo[(int)type];
	}

	public int AltCount( AltType type )
	{
		var iType = (int)type;
		if ( Alt == null ) return 0;
		if ( Alt.Count <= iType ) return 0;

		return Alt[(int)type];
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

	public bool SetAlt( AltType type, int amount )
	{
		var iType = (int)type;
		if ( !Host.IsServer ) return false;
		if ( Alt == null ) return false;

		while ( Ammo.Count <= iType )
		{
			Alt.Add( 0 );
		}

		Alt[(int)type] = amount;
		return true;
	}

	public bool GiveAmmo( AmmoType type, int amount )
	{
		if ( !Host.IsServer ) return false;
		if ( Ammo == null ) return false;

		SetAmmo( type, AmmoCount( type ) + amount );
		return true;
	}

	public bool GiveAlt( AltType type, int amount )
	{
		if ( !Host.IsServer ) return false;
		if ( Alt == null ) return false;

		SetAlt( type, AltCount( type ) + amount );
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

	public int TakeAlt( AltType type, int amount )
	{
		if ( Alt == null ) return 0;

		var available = AltCount( type );
		amount = Math.Min( available, amount );

		SetAlt( type, available - amount );
		return amount;
	}
}

public enum AmmoType
{
	Pistol,
	Magnum,
	SMG,
	Pulse,
	Buckshot,
	Crossbow
}

public enum AltType
{
	Grenade,
	EnergyPellet
}
