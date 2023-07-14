using Sandbox;
using System;
using System.Threading.Tasks;

[Library( "hl2_slammounted" )]
[HideInEditor]
partial class hl2_slammounted : Prop
{
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/weapons/hl2_slam/w_hl2_slam_open.vmdl" );

		Health = 5;
		PhysicsEnabled = false;
		UsePhysicsCollision = true;
	}



	public override void OnKilled()
	{
		DoExplosion();
	}

	private void DoExplosion()
	{
		if ( Model == null || Model.IsError )
			return;

		///if ( !Model.HasExplosionBehavior() )
		///	return;

		var srcPos = Position;
		if ( PhysicsBody.IsValid() ) srcPos = PhysicsBody.MassCenter;

		///var explosionBehavior = Model.GetExplosionBehavior();

		// Damage and push away all other entities
	///	if ( explosionBehavior.Radius > 0.0f )
		//{
		//	new ExplosionEntity
		//	{
		//		Position = srcPos,
		//		Radius = explosionBehavior.Radius,
		//		Damage = explosionBehavior.Damage,
		//		ForceScale = explosionBehavior.Force,
		//		ParticleOverride = explosionBehavior.Effect,
		//		SoundOverride = explosionBehavior.Sound
		//	}.Explode( this );
	//	}

	}
}
