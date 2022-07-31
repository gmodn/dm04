using Sandbox;
using System;
using System.Threading.Tasks;

[Library( "hl2_grenadethrown" )]
[HideInEditor]
partial class hl2_grenadethrown : Prop
{
	private PointLightEntity bleepLight;
	int bleeps = 0;
	bool blowingup;
	private PointLightEntity CreateLight()
	{
		var light = new PointLightEntity
		{
			Enabled = false,

			Range = 384,
			Falloff = 1.0f,
			LinearAttenuation = 5.0f,
			QuadraticAttenuation = 1.0f,
			Brightness = 2,
			Color = Color.Red,
			FogStength = 1.0f,
			Owner = Owner,
		};

		return light;
	}

	public override void Spawn()
	{
		Tags.Add( "projectile" );
		base.Spawn();
		SetModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" );

		bleepLight = CreateLight();
		bleepLight.SetParent( this, "light", Transform );
		bleepLight.EnableShadowCasting = false;

		MoveType = MoveType.Physics;
		PhysicsEnabled = true;
		UsePhysicsCollision = true;
		SetInteractsExclude( CollisionLayer.Player );
		Particles.Create( "particles/weapons/hl2_grenade_trail.vpcf", this, "light" );
		Bleep();
	}

	public async Task Bleep()
	{
		await Task.DelaySeconds( 0.2f );
		if ( bleeps < 6 )
		{
			PlaySound( "hl2_grenade.tick_new" );
			bleepLight.Enabled = true;

			await Task.DelaySeconds( 0.1f );
			bleepLight.Enabled = false;
			bleeps += 1;
			Bleep();
		}
		else if ( blowingup == false )
		{
			DoExplosion();
		}
	}
	public void DoExplosion()
	{

		///facepunch did dumb dumb i cant acess explosiondata properly anymore i think so this is how they worked around it its not the best and if i can i will go back to my method
		///
		DeathmatchGame.Explosion( this, Owner, Position, 400, 100, 1.0f );
		Delete();
		blowingup = true;
		if ( Model == null || Model.IsError )
			return;

		//if ( !Model.HasExplosionBehavior() )
			return;

		var srcPos = Position;
		if ( PhysicsBody.IsValid() ) srcPos = PhysicsBody.MassCenter;

		//var explosionBehavior = Model.GetExplosionBehavior();

		// Damage and push away all other entities
		//if ( explosionBehavior.Radius > 0.0f )
	//	{
		//	new ExplosionEntity
		//	{
		//		Position = srcPos,
		//		Radius = explosionBehavior.Radius,
		//		Damage = explosionBehavior.Damage,
		//		ForceScale = explosionBehavior.Force,
		//		ParticleOverride = explosionBehavior.Effect,
		//		SoundOverride = explosionBehavior.Sound
		//	}.Explode( this );
		//}
	//	Delete();
	}
}
