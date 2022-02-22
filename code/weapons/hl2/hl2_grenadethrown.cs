using Sandbox;
using System;
using System.Threading.Tasks;

[Library( "hl2_grenadethrown" )]
[Hammer.Skip]
partial class hl2_grenadethrown : ModelEntity
{
	private PointLightEntity bleepLight;
	int bleeps = 0;

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
		base.Spawn();
		SetModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" );

		bleepLight = CreateLight();
		bleepLight.SetParent( this, "light", Transform );
		bleepLight.EnableShadowCasting = false;

		MoveType = MoveType.Physics;
		PhysicsEnabled = true;
		UsePhysicsCollision = true;
		SetInteractsExclude( CollisionLayer.Player );
		//Particles.Create( "particles/weapons/hl2_grenade_trail.vpcf", this, "light" );
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
		else Explode();
	}

	public virtual void Explode()
	{
		Sound.FromWorld( "hl2_spas12.fire", PhysicsBody.MassCenter );

		Delete();
	}
}
