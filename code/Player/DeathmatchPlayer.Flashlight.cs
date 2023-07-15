using Sandbox;

public partial class DeathmatchPlayer
{
	protected virtual Vector3 LightOffset => Vector3.Forward * 10;

	private SpotLightEntity worldLight;
	private SpotLightEntity viewLight;

	[Net, Local, Predicted]
	private bool LightEnabled { get; set; } = true;

	TimeSince timeSinceLightToggled;

	public override void Spawn()
	{
		base.Spawn();
	}

	private SpotLightEntity CreateLight()
	{
		var light = new SpotLightEntity
		{
			Enabled = true,
			DynamicShadows = true,
			Range = 512,
			Falloff = 1.0f,
			LinearAttenuation = 0.0f,
			QuadraticAttenuation = 1.0f,
			Brightness = 2,
			Color = Color.White,
			InnerConeAngle = 20,
			OuterConeAngle = 40,
			FogStrength = 1.0f,
			Owner = Owner,
			LightCookie = Texture.Load( "materials/effects/lightcookie.vtex" )
		};

		return light;
	}

	public void Simulateflashlight()
	{

		bool toggle = Input.Pressed( "flashlight" );

		if ( worldLight == null )
		{
			worldLight = CreateLight();
			worldLight.SetParent( this );
			worldLight.Enabled = true;

		}

		if ( worldLight.IsValid() && Game.IsServer )
		{
			worldLight.Enabled = LightEnabled;
			worldLight.Position = EyePosition + EyeRotation.Forward * 25;
			worldLight.Rotation = EyeRotation;
		}

		if ( timeSinceLightToggled > 0.3f && toggle )
		{
			LightEnabled = !LightEnabled;

			PlaySound( LightEnabled ? "flashlight-on" : "flashlight-off" );

			timeSinceLightToggled = 0;
		}
	}

	private void Activate()
	{
		if ( worldLight.IsValid() )
		{
			worldLight.Enabled = LightEnabled;
			Log.Info( "flashlight is active." );
		}
	}

	private void Deactivate()
	{
		if ( worldLight.IsValid() )
		{
			worldLight.Enabled = false;
			Log.Info( "flashlight is inactive." );
		}
	}
}
