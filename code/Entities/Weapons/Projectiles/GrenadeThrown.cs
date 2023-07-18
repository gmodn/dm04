using Sandbox;
using System;
using System.Threading.Tasks;

[Library( "dm_grenadethrown" )]
[HideInEditor]
partial class GrenadeThrown : Prop
{
	PointLightEntity bleepLight;

	//Similar to S1 when grabbing with gravity gun
	//Only do this once so timer isn't constantly reset
	bool gravGrabbed;

	public TimeUntil TimeToExplode;
	float baseTimer => 3.5f;

	bool canExplode = false;

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
			Owner = Owner,
		};

		return light;
	}

	public override void Spawn()
	{
		TimeToExplode = baseTimer;
		Tags.Add( "projectile" );
		base.Spawn();
		SetModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" );

		bleepLight = CreateLight();
		bleepLight.SetParent( this, "light", Transform );
		bleepLight.EnableShadowCasting = false;

		PhysicsEnabled = true;
		UsePhysicsCollision = true;
		Particles.Create( "particles/weapons/hl2_grenade_trail.vpcf", this, "light" );
		
		canExplode = true;
	}

	[GameEvent.Tick.Server]
	protected void TickGrenade()
	{
		if ( !IsValid || TimeToExplode < 0.0f ) return;

		if ( TimeToExplode > 1.0f && TimeToExplode % 0.75f == 0 )
			DoBleep();
		else if ( TimeToExplode < 1.0f && TimeToExplode % 0.25f == 0 )
			DoBleep();

		if ( TimeToExplode <= 0.0f && canExplode )
			DoExplosion();
	}

	async void DoBleep()
	{
		if ( !IsValid ) return;

		PlaySound( "hl2_grenade.tick_new" );
		bleepLight.Enabled = true;

		await Task.DelaySeconds( 0.1f );

		if ( !IsValid ) return;

		bleepLight.Enabled = false;
	}

	public void ResetTimerGrav()
	{
		if ( !IsValid || gravGrabbed ) return;

		gravGrabbed = true;
		TimeToExplode = baseTimer;
	}

	public void DoExplosion()
	{
		DeathmatchGame.Explosion( this, Owner, Position, 400, 100, 1.0f );
		Delete();
	}
}
