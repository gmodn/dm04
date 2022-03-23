using Sandbox;


[Library( "hl2_rpgmissile" )]
[Hammer.Skip]
partial class hl2_rpgmissile : Prop
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
			DoExplosion();
		}
		else
		{
			Position = end;
		}
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
