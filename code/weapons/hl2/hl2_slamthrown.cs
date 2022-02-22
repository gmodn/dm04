using Sandbox;
using System;
using System.Threading.Tasks;

[Library( "hl2_slamthrown" )]
[Hammer.Skip]
partial class hl2_slamthrown : ModelEntity
{
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/hl2_slam/w_hl2_slam_closed.vmdl" );

		Health = 5;
		MoveType = MoveType.Physics;
		PhysicsEnabled = true;
		UsePhysicsCollision = true;
		SetInteractsExclude( CollisionLayer.Player );
	}

	public override void OnKilled()
	{
		base.OnKilled();

		//Blow up but do no damage
		PlaySound("balloon_pop_cute");
	}

	public virtual void Explode()
	{
		Sound.FromWorld( "hl2_spas12.fire", PhysicsBody.MassCenter );

		Delete();
	}
}
