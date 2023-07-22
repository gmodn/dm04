partial class Coffin : ModelEntity
{
	public static readonly Model WorldModel = Model.Load( "models/player/hev_coffin.vmdl" );

	public List<string> Weapons = new List<string>();
	public List<int> Ammos = new List<int>();

	public bool CoffinDebug { get; set; } = false;

	public override void Spawn()
	{
		base.Spawn();

		Model = WorldModel;
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		Tags.Add( "item" );

		if ( CoffinDebug == true )
		{
			Log.Info( "Coffin Debug Enabled" );
			Log.Info( $"Coffin ID: {this.NetworkIdent}" );
			Log.Info( $"Coffin Contents: {this.Weapons} {this.Ammos}" );
		}
	}

	public void Populate( DeathmatchPlayer player )
	{
		Ammos.AddRange( player.Ammo );

		foreach ( var child in player.Children.ToArray() )
		{
			if ( child is HLDMWeapon weapon )
			{
				Weapons.Add( weapon.ClassName );
			}
		}
	}

	public override void StartTouch( Entity other )
	{
		base.StartTouch( other );

		if ( Game.IsClient )
			return;

		if ( other is not DeathmatchPlayer player )
			return;

		if ( player.LifeState == LifeState.Dead )
			return;

		Sound.FromWorld( "dm.pickup_ammo", Position );

		foreach ( var weapon in Weapons )
		{
			player.Give( weapon );
		}

		for ( int i = 0; i < Ammos.Count; i++ )
		{
			int taken = player.GiveAmmo( (AmmoType)i, Ammos[i] );
			if ( taken > 0 )
			{
				PickupFeed.OnPickup( To.Single( player ), $"+{taken} {((AmmoType)i)}" );
			}
		}

		Delete();
	}

}
