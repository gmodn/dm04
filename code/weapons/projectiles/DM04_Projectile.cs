using System;
using Sandbox;

	[Library("dm98_baseprojectile")]
	[Hammer.Skip]
partial class BaseProjctile : Prop
{
	public override void Spawn()
	{
		base.Spawn();

		SetModel("models/rust_props/barrels/fuel_barrel.vmdl");
	}


	[Event.Tick.Server]
	public virtual void Tick()
	{
		if ( !IsServer )
			return;

		float Speed = 2000.0f;
		var velocity = Rotation.Forward * Speed;

		var start = Position;
		var end = start + velocity * Time.Delta;

		var tr = Trace.Ray( start, end )
			.UseHitboxes()
			//.HitLayer( CollisionLayer.Water, !InWater )
			.Ignore( Owner )
			.Ignore( this )
			.Size( 1.0f )
			.Run();

		if ( tr.Entity is DeathmatchPlayer player )
		{
			if(Owner is DeathmatchPlayer plOwner )
			{

			}

			int hitBone = player.GetHitboxBone( tr.HitboxIndex );

			//Head
			if(hitBone == 5)
				player.Health -= 50f;
			else
				player.Health -= 25f;

			if ( player.Health <= 0 )
			{
				player.LastAttacker = this;
				player.OnKilled();

			}
			Delete();
		}
	}
}
