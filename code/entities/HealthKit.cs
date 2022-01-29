using Sandbox;
using System;

[Library( "dm08_healthkit", Title = "Health Kit" )]
public partial class HealthKit: Prop, IUse, IRespawnableEntity
{
	public PickupTrigger PickupTrigger { get; protected set; }

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/citizen_props/recyclingbin01.vmdl" );
		Scale = 0.3f;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
	public bool OnUse( Entity user )
	{
		if ( user is Player player )
		{
			player.Health += 10;

			Delete();
		}

		Sound.FromScreen( "ui.button.press" );

		return false;
	}

}
