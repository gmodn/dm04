using Sandbox;


[Library( "hl2_rpgmissile" )]
[Hammer.Skip]
partial class hl2_rpgmissile : ModelEntity
{
	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_rpg/hl2_rpg_missile.vmdl" );
	}

	[Event.Tick.Server]
	public virtual void Tick()
	{
		if ( !IsServer )
			return;

		var start = Position;
		var end = start + Velocity * Time.Delta;

		var tr = Trace.Ray( start, end )
				.UseHitboxes()
				//.HitLayer( CollisionLayer.Water, !InWater )
				.Ignore( Owner )
				.Ignore( this )
				.Size( 1.0f )
				.Run();

		//TODO: if mode set to targeting then follow the cursor

		if ( tr.Hit )
		{
			Explode();
		}
		else
		{
			Position = end;
		}
	}

	public virtual void Explode()
	{
		PlaySound( "hl2_spas12.fire" );

		Delete();
	}
}
