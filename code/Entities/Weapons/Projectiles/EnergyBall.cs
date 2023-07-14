using Sandbox;


partial class prop_combine_ball : ModelEntity
{
	float Speed = 1000.0f;
	TimeSince LifeSpan = 0;
	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_ar2/hl2_ar2_ball.vmdl" );		
	}


	[GameEvent.Tick.Server]
	public virtual void Tick()
	{
		Velocity = Rotation.Forward * Speed;

		var start = Position;
		var end = start + Velocity * Time.Delta;

		var tr = Trace.Ray( start, end )
			.UseHitboxes()
			//.HitLayer( CollisionLayer.Water, !InWater )
			.Ignore( Owner )
			.Ignore( this )
			.Size( 8 )
			.Run();
		
		if ( tr.Hit )
		{	
			if ( (tr.Entity.IsValid()) && (!tr.Entity.IsWorld) )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, tr.Direction * 300, 500 )
													.UsingTraceResult( tr )
													.WithAttacker( Owner )
													.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
				Position = end;
				return;
			}
			else
            {
				Velocity = Vector3.Reflect( Rotation.Forward, tr.Normal );
				Rotation = Rotation.From(Vector3.VectorAngle(Velocity));

				PlaySound( "sounds/weapons/hl2_ar2ball/hl2_ar2ball.bounce.sound" );
			}
		}

		else Position = end;
		if ( LifeSpan >= 4.27f ) Explode();
	}

	public virtual void Explode()
	{
		PlaySound( "sounds/weapons/hl2_ar2ball/hl2_ar2ball.explosion.sound" );
		Delete();
	}
}
