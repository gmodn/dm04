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
		bool InWater = Map.Physics.IsPointWater( this.Position );

		var tr = Trace.Sphere( 100, this.Position, this.Position )
				.UseHitboxes()

				.HitLayer( CollisionLayer.Water, !InWater )
				.HitLayer( CollisionLayer.Debris )
				///.Ignore( Owner )
				.Ignore( this )

				.RunAll();
		foreach ( var Entity in tr )
		{

			Entity.Surface.DoBulletImpact( Entity );
			DebugOverlay.Sphere( this.Position, 100, Color.Red, true, 100 );

			//
			// We turn predictiuon off for this, so aany exploding effects don't get culled etc
			//
			using ( Prediction.Off() )
			{
				Log.Info( Entity.Entity );
				var damage = DamageInfo.Explosion( this.Position, 100, 15 )
					.UsingTraceResult( Entity )
					.WithAttacker( Owner )
					.WithWeapon( this );

				Entity.Entity.TakeDamage( damage );
			}
		}
		Sound.FromWorld( "hl2_spas12.fire", PhysicsBody.MassCenter );

		Delete();
	}
}
