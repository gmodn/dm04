﻿using Sandbox;
using System;
using System.Threading.Tasks;

[Library( "hl2_bugbaitthrown" )]
[Hammer.Skip]
partial class hl2_bugbaitthrown : ModelEntity
{

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/worldmodels/w_bugbait_reference.vmdl" );

		MoveType = MoveType.Physics;
		PhysicsEnabled = true;
		UsePhysicsCollision = true;
		SetInteractsExclude( CollisionLayer.Player );
		//Particles.Create( "particles/weapons/hl2_grenade_trail.vpcf", this, "light" );
	}
}