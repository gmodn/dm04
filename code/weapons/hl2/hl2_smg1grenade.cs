using Sandbox;
using System;

[Library( "hl2_smg1grenade" )]
[Hammer.Skip]
partial class hl2_smg1grenade : ModelEntity
{
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/hl2_smg1/hl2_smg1_grenade.vmdl" );

		MoveType = MoveType.Physics;
		PhysicsEnabled = true;
		UsePhysicsCollision = true;
		SetInteractsExclude( CollisionLayer.Player );
	}

	protected override void OnPhysicsCollision( CollisionEventData eventData )
	{
		if ( eventData.Entity == Owner ) return;

		Explode();
	}

	public virtual void Explode()
	{
		Sound.FromWorld( "hl2_spas12.fire", PhysicsBody.MassCenter );

		Delete();
	}
}
