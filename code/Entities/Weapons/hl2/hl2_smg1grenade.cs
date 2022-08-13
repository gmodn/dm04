using Sandbox;
using System;

[Library( "hl2_smg1grenade" )]
[HideInEditor]
partial class hl2_smg1grenade : ModelEntity
{
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/hl2_smg1/hl2_smg1_grenade.vmdl" );

		Tags.Add( "projectile" );
		PhysicsEnabled = true;
		UsePhysicsCollision = true;
	}

	[Event.Tick.Server]
	public void Simulate()
	{
		var trace = Trace.Ray( Position, Position )
			.Size( 24 )
			.Ignore( this )
			.Ignore( Owner )
			.Run();

		Position = trace.EndPosition;

		if ( trace.Hit )
		{
			Explode();
		}
	}

	public virtual void Explode()
	{
		bool InWater = Map.Physics.IsPointWater( Position );

		var overlaps = FindInSphere( Position, 64);

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

			var dist = Vector3.DistanceBetween( Position, targetPos );
			if ( dist > 250 )
				continue;

			var tr = Trace.Ray( Position, targetPos )
				.Ignore( this )
				.WorldOnly()
				.Run();

			if ( tr.Fraction < 0.98f )
				continue;

			var distanceMul = 1.0f - Math.Clamp( dist / 250, 0.0f, 1.0f );
			var dmg = 100 * distanceMul;
			var force = (1.0f * distanceMul) * ent.PhysicsBody.Mass;
			var forceDir = (targetPos - Position).Normal;

			var damageInfo = DamageInfo.Explosion( Position, forceDir * force, dmg )
				.WithWeapon( this )
				.WithAttacker( Owner );

			ent.TakeDamage( damageInfo );
		}
	}
}
