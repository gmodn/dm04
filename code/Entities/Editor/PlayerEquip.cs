[Library( "dm04_playerequip" ), HammerEntity]
[Title( "Equipment" ), Category( "Hammer" ), EditorSprite( "materials/dev/editor/dm04playerequip.vmat" ), Icon( "backpack" )]

public partial class PlayerSetup : Entity
{
	public static PlayerSetup Equipment;

	List<(HLDMWeapon Wep, int Ammo, int Start)> Weapons;
	List<(AmmoType AltType, int AltAmmo)> AltWeapons;

	//[Property, Description( "The equipment players will start with" )]
	//public WeaponEquipment[] Weapon { get; set; }

	//[Property( "EnableFlashlight" ), Description( "Is flashlight enabled or disabled on spawn" )]
	//public bool Enable_Flashlight { get; set; } = true;
	//Will do something. Someday.

	//Spawn with weapons
	#region
	[Property, Category( "Melee" )]
	public bool GiveCrowbar { get; set; } = false;

	[Property, Category( "Melee" )]
	public bool GiveStunstick { get; set; } = false;

	[Property, Category( "Pistol" )]
	public bool GivePistol { get; set; } = false;

	[Property, Category( "Magnum" )]
	public bool GiveMagnum { get; set; } = false;

	[Property, Category( "SMG" )]
	public bool GiveSMG { get; set; } = false;

	[Property, Title( "Give AR2" ), Category( "AR2" )]
	public bool GiveAR2 { get; set; } = false;

	[Property, Category( "Shotgun" )]
	public bool GiveShotgun { get; set; } = false;

	[Property, Category( "Crossbow" )]
	public bool GiveCrossbow { get; set; } = false;

	[Property, Category( "RPG" )]
	public bool GiveRPG { get; set; } = false;

	[Property, Category( "Throwable" )]
	public bool GiveGrenades { get; set; } = false;

	[Property, Category( "Throwable" )]
	public bool GiveSlams { get; set; } = false;

	[Property, Category( "Misc" )]
	public bool GiveGravityGun { get; set; } = false;
	#endregion

	//Starting ammo/clip
	#region
	[Property, Title( "Starting Ammo" ), Category( "Pistol" )]
	public int PistolAmmo { get; set; } = 30;

	[Property, Title( "Starting Clip" ), Category( "Pistol" )]
	public int PistolClip { get; set; } = 18;

	[Property, Title( "Starting Ammo" ), Category( "Magnum" )]
	public int MagnumAmmo { get; set; } = 9;

	[Property, Title( "Starting Clip" ), Category( "Magnum" )]
	public int MagnumClip { get; set; } = 6;

	[Property, Title( "Starting Ammo" ), Category( "SMG" )]
	public int SMGAmmo { get; set; } = 75;

	[Property, Title( "Starting Clip" ), Category( "SMG" )]
	public int SMGClip { get; set; } = 45;

	[Property, Title( "Starting Alternative Ammo" ), Category( "SMG" )]
	public int SMGAltAmmo { get; set; } = 1;

	[Property, Title( "Starting Ammo" ), Category( "AR2" )]
	public int AR2Ammo { get; set; } = 30;

	[Property, Title( "Starting Clip" ), Category( "AR2" )]
	public int AR2Clip { get; set; } = 30;

	[Property, Title( "Starting Alternative Ammo" ), Category( "AR2" )]
	public int AR2AltAmmo { get; set; } = 1;

	[Property, Title( "Starting Ammo" ), Category( "Shotgun" )]
	public int ShotgunAmmo { get; set; } = 6;

	[Property, Title( "Starting Clip" ), Category( "Shotgun" )]
	public int ShotgunClip { get; set; } = 12;

	[Property, Title( "Starting Ammo" ), Category( "Crossbow" )]
	public int CrossbowAmmo { get; set; } = 2;

	[Property, Title( "Starting Grenades" ), Category( "Throwable" )]
	public int Grenades { get; set; } = 2;

	[Property, Title( "Starting Ammo" ), Category( "RPG" )]
	public int RPGAmmo { get; set; } = 0;

	[Property, Title( "Starting Slams" ), Category( "Throwable" )]
	public int Slams { get; set; } = 2;
	#endregion

	public override void Spawn()
	{
		base.Spawn();
		Equipment = this;

		GenerateWeapons();
	}

	public void GenerateWeapons()
	{
		Weapons = new();
		AltWeapons = new();

		if ( GiveCrowbar )
			Weapons.Add( (new Crowbar(), -1, -1) );

		if ( GiveStunstick )
			Weapons.Add( (new Stunstick(), -1, -1) );

		if ( GivePistol )
			Weapons.Add( (new Pistol(), PistolAmmo, PistolClip) );

		if ( GiveMagnum )
			Weapons.Add( (new Python(), MagnumAmmo, MagnumClip) );

		if ( GiveSMG )
		{
			Weapons.Add( (new SMG(), SMGAmmo, SMGClip) );
			AltWeapons.Add( (AmmoType.SMGGrenade, SMGAltAmmo) );
		}

		if ( GiveAR2 )
		{
			Weapons.Add( (new AR2(), AR2Ammo, AR2Clip) );
			AltWeapons.Add( (AmmoType.AR2Alt, AR2AltAmmo) );
		}

		if ( GiveShotgun )
			Weapons.Add( (new Shotgun(), ShotgunAmmo, ShotgunClip) );

		//if ( GiveGrenades )
		//Weapons.Add( (new HandGrenade(), Grenades, 0) );

		if ( GiveCrossbow )
			Weapons.Add( (new Crossbow(), CrossbowAmmo, 1) );

		//if ( GiveRPG )
		//Weapons.Add( (new RPG(), 0) );

		//if ( GiveGravityGun )
		//	Weapons.Add( (new GravGun(), -1, -1) );
	}

	//Clamps reserve ammo
	int GetMaxAmmo( AmmoType wep, bool alt )
	{
		//If the weapon has alt fire, return max alt ammo
		if ( alt )
		{
			return wep switch
			{
				AmmoType.SMGGrenade => 3,
				AmmoType.AR2Alt => 3,

				//Default to -1 if above don't match
				_ => -1
			};
		}

		//Return max ammo the player can hold for this weapon
		return wep switch
		{
			AmmoType.Pistol => 150,
			AmmoType.Magnum => 12,
			AmmoType.SMG => 225,
			AmmoType.AR2 => 60,
			AmmoType.Buckshot => 30,
			AmmoType.Crossbow => 10,
			AmmoType.Grenade => 5,
			AmmoType.RPG => 3,

			//Default to -1 if weapon is considered a melee
			_ => -1
		};
	}

	//Clamps starting ammo
	int GetMaxStartAmmo( AmmoType wep )
	{
		return wep switch
		{
			AmmoType.Pistol => 18,
			AmmoType.Magnum => 6,
			AmmoType.AR2 => 30,
			AmmoType.SMG => 45,
			AmmoType.Buckshot => 6,
			AmmoType.Crossbow => 1,
			AmmoType.RPG => 1,

			//Default to -1 if weapon is a melee
			_ => -1
		};
	}

	//Returns true if weapon has alternative fire with ammo for said weapon
	bool HasAltFire( HLDMWeapon wep )
	{
		return wep.SecondaryAmmo != AmmoType.None;
	}

	/// <summary>
	/// Gives weapons to the player
	/// </summary>
	public void GiveWeapons( DeathmatchPlayer player )
	{
		foreach ( var equipItem in Equipment.Weapons )
		{
			HLDMWeapon weapon = CreateByName<HLDMWeapon>( equipItem.Wep.ClassName );
			int ammo = equipItem.Ammo;
			ammo = ammo.Clamp( -1, GetMaxAmmo( weapon.AmmoType, false ) );

			int startClip = equipItem.Start;
			startClip = startClip.Clamp( -1, GetMaxStartAmmo( weapon.AmmoType ) );

			player.SetAmmo( weapon.AmmoType, ammo );

			weapon.AmmoClip = startClip;

			var altAmmo = AltWeapons.Where( a => a.AltType == weapon.SecondaryAmmo )
				.FirstOrDefault();

			if ( altAmmo.AltAmmo > -1 )
				player.SetAmmo( altAmmo.AltType, altAmmo.AltAmmo );

			player.Inventory.Add( weapon );
		}
	}

	/// <summary>
	/// If no equipment entity is present, give player default weapons and ammo
	/// </summary>
	public static void GiveWeaponsDefault( DeathmatchPlayer player )
	{
		HLDMWeapon crowbar = new Crowbar();
		player.Inventory.Add( crowbar );

		HLDMWeapon usp = new Pistol();
		usp.AmmoClip = 12;

		player.Inventory.Add( usp );
		player.SetAmmo( AmmoType.Pistol, 50 );

		HLDMWeapon smg = new SMG();
		smg.AmmoClip = 30;

		player.Inventory.Add( smg );
		player.SetAmmo( AmmoType.SMG, 75 );
	}
}
