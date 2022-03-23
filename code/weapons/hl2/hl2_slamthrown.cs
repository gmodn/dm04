using Sandbox;
using System;
using System.Threading.Tasks;

[Library( "hl2_slamthrown" )]
[Hammer.Skip]
partial class hl2_slamthrown : ModelEntity
{
	public bool mounted { get; set; }
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/hl2_slam/w_hl2_slam_closed.vmdl" );

		Health = 5;
		MoveType = MoveType.Physics;
		if ( mounted == true )
		{
			PhysicsEnabled = false;
		}
		else
		{
			PhysicsEnabled = true;
		}
		UsePhysicsCollision = true;
		SetInteractsExclude( CollisionLayer.Player );
	}

	public override void OnKilled()
	{
		base.OnKilled();

		//Blow up but do no damage
		PlaySound("balloon_pop_cute");
	}

	public void DoExplosion()
	{
		if ( Model == null || Model.IsError )
			return;

		if ( !Model.HasExplosionBehavior() )
			return;

		var srcPos = Position;
		if ( PhysicsBody.IsValid() ) srcPos = PhysicsBody.MassCenter;

		var explosionBehavior = Model.GetExplosionBehavior();

		// Damage and push away all other entities
		if ( explosionBehavior.Radius > 0.0f )
		{
			new ExplosionEntity
			{
				Position = srcPos,
				Radius = explosionBehavior.Radius,
				Damage = explosionBehavior.Damage,
				ForceScale = explosionBehavior.Force,
				ParticleOverride = explosionBehavior.Effect,
				SoundOverride = explosionBehavior.Sound
			}.Explode( this );
		}
	}
}
