using Sandbox;
using System;

[Library( "dm08_healthvial", Title = "Health Kit" )]
[Hammer.EditorModel( "models/healthvial.vmdl" )]
[Hammer.EntityTool( "Health Vial", "DM:04" )]
public partial class HealthVial : Prop, IUse, IRespawnableEntity
{
	public PickupTrigger PickupTrigger { get; protected set; }

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/healthvial.vmdl" );
		PickupTrigger = new PickupTrigger();
		PickupTrigger.Parent = this;
		PickupTrigger.Position = Position;
	}

	public bool IsUsable( Entity user )
	{
		if ( user.Health > 100 )
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	public bool OnUse( Entity user )
	{
		if ( user is Player player )
		{
			player.Health += 10;

			Delete();
			Sound.FromScreen( "smallmedkit1" );
		}

		if ( user.Health > 100 )
		{
			
		}

		return false;
	}

}
