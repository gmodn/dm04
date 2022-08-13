using Sandbox;
using System;
using System.Threading.Tasks;

[Library( "hl2_bugbaitthrown" )]
[HideInEditor]
partial class hl2_bugbaitthrown : ModelEntity
{

	public override void Spawn()
	{
		Tags.Add( "projectile" );
		base.Spawn();
		SetModel( "models/worldmodels/w_bugbait_reference.vmdl" );

		PhysicsEnabled = true;
		UsePhysicsCollision = true;
		//Particles.Create( "particles/weapons/hl2_grenade_trail.vpcf", this, "light" );
	}
}
