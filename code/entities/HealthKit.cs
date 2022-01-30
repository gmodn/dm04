﻿using Sandbox;
using System;

[Library( "dm08_healthkit", Title = "Health Kit" )]
[Hammer.EditorModel( "models/items/healthkit.vmdl" )]
[Hammer.EntityTool( "Health Kit", "DM:04" )]
public partial class HealthKit: Prop, IUse, IRespawnableEntity
{
	public PickupTrigger PickupTrigger { get; protected set; }

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/items/healthkit.vmdl" );
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}
	public bool OnUse( Entity user )
	{
		if ( user is Player player )
		{
			player.Health += 25;

			Delete();
		}

		Sound.FromScreen( "ui.button.press" );

		return false;
	}

}
