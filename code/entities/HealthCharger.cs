using Sandbox;
using System;

[Library( "dm08_healthcharger", Title = "Health Charger" )]
[Hammer.EditorModel( "models/props_combine/suit_charger001.vmdl" )]
[Hammer.EntityTool( "Health Charger", "DM:04" )]
public partial class HealthCharger: ModelEntity, IUse
{
	private Sound healthBeat;
	int storedHealth = 75;

	public PickupTrigger PickupTrigger { get; protected set; }

	public override void Spawn()
	{
		base.Spawn();

		MoveType = MoveType.Physics;
		PhysicsEnabled = false;
		UsePhysicsCollision = true;
		SetModel( "models/props_combine/suit_charger001.vmdl" );
	}

	public bool IsUsable( Entity user )
	{
		if (user is Player player)
		{
			if ((player.Health < 100) & (storedHealth > 0))
			{
				return true;
			}
			else
				healthBeat.Stop();
				Sound.FromScreen( "medshotno1" );
				return false;
		}
		else
		{
			return false;
		}
	}
	public bool OnUse( Entity user )
	{
		if ( user is Player player )
		{
			player.Health += 1;
			storedHealth -= 1;

			Sound.FromScreen( "medshot4" );
			//healthBeat = Sound.FromScreen("medcharge4");
		}
		return false;
	}
}
