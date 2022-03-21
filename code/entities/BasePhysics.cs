namespace Sandbox
{
	/// <summary>
	/// Base entity with phyiscal properties, enables imapct damage and the like
	/// </summary>
	[Library( "prop_physics_respawnable", Title = "prop_physics_respawnable", Spawnable = true )]
	[Hammer.EntityTool( "prop_physics_respawnable", "DM:04" )]
	[Hammer.Model]
	public partial class BasePhysics : ModelEntity, IRespawnableEntity
	{
		public override void Spawn()
		{
			base.Spawn();

			MoveType = MoveType.Physics;
			CollisionGroup = CollisionGroup.Interactive;
			PhysicsEnabled = true;
			UsePhysicsCollision = true;
		}

		public override void TakeDamage( DamageInfo info )
		{
			ApplyDamageForces( info );

			base.TakeDamage( info );
		}

		protected void ApplyDamageForces( DamageInfo info )
		{
			var body = info.Body;
			if ( !body.IsValid() )
				body = PhysicsBody;

			if ( body.IsValid() && !info.Flags.HasFlag( DamageFlags.PhysicsImpact ) )
			{
				body.ApplyImpulseAt( info.Position, info.Force * 100 );
			}
		}

		protected virtual ModelPropData GetModelPropData()
		{
			if ( Model != null && !Model.IsError && Model.HasPropData() )
			{
				return Model.GetPropData();
			}

			ModelPropData defaultData = new ModelPropData();
			defaultData.Health = -1;
			defaultData.ImpactDamage = 10;
			if ( PhysicsGroup != null )
			{
				defaultData.ImpactDamage = PhysicsGroup.Mass / 10;
			}
			defaultData.MinImpactDamageSpeed = 500;
			return defaultData;
		}

		protected override void OnPhysicsCollision( CollisionEventData eventData )
		{
			var propData = GetModelPropData();
			if ( propData == null ) return;

			var minImpactSpeed = propData.MinImpactDamageSpeed;
			if ( minImpactSpeed <= 0.0f ) minImpactSpeed = 500;

			var impactDmg = propData.ImpactDamage;
			if ( impactDmg <= 0.0f ) impactDmg = 10;

			float speed = eventData.Speed;

			if ( speed > minImpactSpeed )
			{
				// I take damage from high speed impacts
				if ( Health > 0 )
				{
					var damage = speed / minImpactSpeed * impactDmg;
					TakeDamage( DamageInfo.Generic( damage ).WithFlag( DamageFlags.PhysicsImpact ) );
				}

				// Whatever I hit takes more damage
				if ( eventData.Entity.IsValid() && eventData.Entity != this )
				{
					var damage = speed / minImpactSpeed * impactDmg * 1.2f;
					eventData.Entity.TakeDamage( DamageInfo.Generic( damage )
						.WithFlag( DamageFlags.PhysicsImpact )
						.WithAttacker( this )
						.WithPosition( eventData.Position )
						.WithForce( eventData.PreVelocity ) );
				}
			}
		}
	}
}
