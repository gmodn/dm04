using Sandbox;


[Library( "hl2_crossbowbolt" )]
[Hammer.Skip]
partial class hl2_crossbowbolt : ModelEntity
{
	bool Stuck;
	float Speed = 3000.0f;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_crossbow/hl2_crossbow_bolt.vmdl" );
	}

	[Event.Tick.Server]
	public virtual void Tick()
	{
		if ( !IsServer )
			return;

		if ( Stuck )
			return;

		//Velocity = Rotation.Forward * Speed;

		var start = Position;
		var end = start + Velocity * Time.Delta;

		var tr = Trace.Ray( start, end )
				.UseHitboxes()
				//.HitLayer( CollisionLayer.Water, !InWater )
				.Ignore( Owner )
				.Ignore( this )
				.Size( 1.0f )
				.Run();


		if ( tr.Hit )
		{
			if ( tr.Entity.IsWorld )
			{
				//ConsoleSystem.Run( "say " + Vector3.Dot( tr.Normal, Velocity ) ); //testing

				//if ( tr.Entity.IsWorld )
				if ( (tr.Entity.IsWorld) && (Vector3.Dot( tr.Normal, Velocity ) > -600))
				{
					Speed = Speed * 0.75f;
					Velocity = Vector3.Reflect( Rotation.Forward, tr.Normal ) * Speed;
					Rotation = Rotation.From(Vector3.VectorAngle(Velocity));

					PlaySound( "hl2_crossbow.hit" );
					return;
				}
			}

			Stuck = true;
			Position = tr.EndPosition + Rotation.Forward * -8;

			//
			// Surface impact effect
			//
			tr.Normal = Rotation.Forward * 2;
			tr.Surface.DoBulletImpact( tr );
			Velocity = default;

			if ( tr.Entity.IsValid() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, tr.Direction * 200, 100.0f )
													.UsingTraceResult( tr )
													.WithAttacker( Owner )
													.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );

				PlaySound( "hl2_crossbow.skewer" );

				if ( (tr.Entity.GetType() == typeof( Sandbox.Prop )) && (tr.Entity.Health == -1) )
				{
					SetParent( tr.Entity );
					Owner = null;
				}
			}
			// delete self in 60 seconds
			_ = DeleteAsync( 30.0f );
		}
		else
		{
			Position = end;
		}
	}
}
