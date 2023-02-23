using System.Collections.Generic;

using System;
using static Sandbox.Package;

[GameResource( "Weapon Definition", "hlweapon", "Define A Half-Life: Deathmatch Weapon", Icon = "approval", IconBgColor = "#fdea90", IconFgColor = "blue" )]
public class WeaponDef : GameResource
{
	
	// Cosmetic info is anything that doesn't affect the gun gameplay wise
	[Category( "Cosmetic Information" )]
	public string Name { get; set; } = "My weapon name";

	[Category( "Cosmetic Information" ), ResourceType( "vmdl" )]
	public string WorldModel { get; set; }

	[Category( "Cosmetic Information" ), ResourceType( "vmdl" )]
	public string ViewModel { get; set; }

	[Category( "Cosmetic Information" ), ResourceType( "sound" )]
	public string FireSound { get; set; }
	
	[Category( "Cosmetic Information" ), ResourceType( "sound" )]
	public string DryFireSound { get; set; }

	[Category( "Cosmetic Information" ), ResourceType( "sound" )]
	public string AltFireSound { get; set; }

	[Category( "Cosmetic Information" ), ResourceType( "sound" )]
	public string ReloadSound { get; set; }

	[Category( "Cosmetic Information" ), ResourceType( "sound" )]
	public string EmptyChamber { get; set; }

	[Category( "Cosmetic Information" )]
	public int Bucket { get; set; } = 1;

	[Category( "Cosmetic Information" )]
	public int BucketWeight { get; set; } = 1;

	[Category( "Cosmetic Information" )]
	public string PrimaryAmmoIcon { get; set; }

	[Category( "Cosmetic Information" )]
	public string SecondaryAmmoIcon { get; set; }

	//Gameplay info is anything that affects the gun gameplay wise
	[Category( "Gameplay Information" )]
	public float ReloadTime { get; set; } = 1.0f;

	[Category( "Gameplay Information" )]
	public float FireRate { get; set; } = 1.0f;

	[Category( "Gameplay Information" )]
	public float AltFireRate { get; set; } = 1.0f;

	[Category( "Gameplay Information" )]
	public int ClipSize { get; set; } = 1;

	[Category( "Bullet Information" )]
	public float BulletSpread { get; set; } = 1.0f;

	[Category( "Bullet Information" )]
	public float BulletForce { get; set; } = 1.0f;

	[Category( "Bullet Information" )]
	public float BulletDamage { get; set; } = 1.0f;

	[Category( "Bullet Information" )]
	public float BulletSize { get; set; } = 1.0f;

	[Category( "Bullet Information" )]
	public int BulletCount { get; set; } = 1;
}
