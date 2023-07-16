﻿/// <summary>
/// Gives 25 Armour
/// </summary>
[Library( "dm_battery" ), HammerEntity]
[EditorModel( "models/dm_ammo_9mmbox.vmdl" )]
[Title(  "Battery" )]
partial class Battery : ModelEntity, IRespawnableEntity
{
	public static readonly Model WorldModel = Model.Load( "models/dm_ammo_9mmbox.vmdl" );

	public override void Spawn()
	{
		base.Spawn();

		Model = WorldModel;

		PhysicsEnabled = true;
		UsePhysicsCollision = true;
		Tags.Add( "item" );
	}

	public override void StartTouch( Entity other )
	{
		base.StartTouch( other );

		if ( other is not DeathmatchPlayer player ) return;
		if ( player.Armour >= 100 ) return;

		var newhealth = player.Armour + 25;

		newhealth = newhealth.Clamp( 0, 100 );

		player.Armour = newhealth;

		Sound.FromWorld( "dm_item_battery", Position );
		PickupFeed.OnPickup( To.Single( player ), $"+25 Armour" );

		ItemRespawn.Taken( this );
		Delete();
	}
}
