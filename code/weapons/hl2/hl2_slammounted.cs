using Sandbox;
using System;
using System.Threading.Tasks;

[Library( "hl2_slammounted" )]
[Hammer.Skip]
partial class hl2_slammounted : ModelEntity
{
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/hl2_slam/w_hl2_slam_open.vmdl" );

		Health = 5;
		MoveType = MoveType.Physics;
		PhysicsEnabled = false;
		UsePhysicsCollision = true;
		SetInteractsExclude( CollisionLayer.Player );
	}



	public override void OnKilled()
	{
		Explode();
	}

	public virtual void Explode()
	{
		Sound.FromWorld( "hl2_spas12.fire", PhysicsBody.MassCenter );

		Delete();
	}
}
