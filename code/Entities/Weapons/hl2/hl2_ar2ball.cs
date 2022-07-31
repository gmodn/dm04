﻿using Sandbox;


[Library( "hl2_ar2ball" )]
[HideInEditor]
partial class hl2_ar2ball : ModelEntity
{
	public Particles Sprite;
	float Speed = 1000.0f;
	TimeSince LifeSpan = 0;
	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_ar2/hl2_ar2_ball.vmdl" );
		Sprite = Particles.Create( "particles/combineball.vpcf",this,true );
		
	}


	[Event.Tick.Server]
	public virtual void Tick()
	{
		if ( !IsServer )
			return;
		
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

				PlaySound( "hl2_ar2ball.bounce_new" );
			}
		}

		else Position = end;
		if ( LifeSpan >= 4.27f ) Explode();
	}

	public virtual void Explode()
	{
		PlaySound("hl2_ar2ball.explosion");
		Sprite.Destroy(true);
		Delete();
	}
}
