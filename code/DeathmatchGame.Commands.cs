/// <summary>
/// This is used for console commands for development reasons, or otherwise. It should be useful.
/// </summary>


partial class DeathmatchGame : GameManager
{
	[ConCmd.Server( "ent_create" )]
	public static void SpawnEntity( string entName )
	{
		var owner = ConsoleSystem.Caller.Pawn as Player;

		if ( owner == null )
			return;

		var entityType = TypeLibrary.GetType<Entity>( entName )?.TargetType;
		if ( entityType == null )
			return;

		if ( !TypeLibrary.HasAttribute<SpawnableAttribute>( entityType ) )
			return;

		var tr = Trace.Ray( owner.EyePosition, owner.EyePosition + owner.EyeRotation.Forward * 200 )
			.UseHitboxes()
			.Ignore( owner )
			.Size( 2 )
			.Run();

		var ent = TypeLibrary.Create<Entity>( entityType );
		if ( ent is BaseCarriable && owner.Inventory != null )
		{
			if ( owner.Inventory.Add( ent, true ) )
				return;
		}

		ent.Position = tr.EndPosition;
		ent.Rotation = Rotation.From( new Angles( 0, owner.EyeRotation.Angles().yaw, 0 ) );

		//Log.Info( $"ent: {ent}" );
	}
}
