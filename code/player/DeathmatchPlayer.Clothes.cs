public partial class DeathmatchPlayer
{
	public ClothingContainer Clothing { get; protected set; }

	public string[] RestrictedClothing = new string[]
	{
		"models/citizen/skin/skeleton/skeleton_skin.vmdl",
		"models/citizen/skin/skeleton/skeleton_clean_skin.vmdl",
		"models/citizen/skin/citizen_skin_zombie.vmdl",
		"models/citizen/skin/citizen_skin_zombie_02.vmdl",
		"models/citizen/skin/muscley/muscley.vmdl",
		"models/citizen/skin/muscley/muscley_02.vmdl",
		"models/citizen/skin/muscley/muscley_03.vmdl",
		"models/citizen_clothes/hat/bucket_helmet/models/bucket_helmet.vmdl"
	};

	/// <summary>
	/// Set the clothes to whatever the player is wearing
	/// </summary>
	public void UpdateClothes( IClient cl )
	{
		Clothing ??= new();

		Clothing.LoadFromClient( cl );
		RemoveForbiddenClothing( cl );
	}

	public void RemoveForbiddenClothing(IClient cl)
	{
		foreach ( var piece in Clothing.Clothing.ToArray() )
		{
			if ( RestrictedClothing.Any( p => p == piece.Model ) || piece.SlotsUnder == Sandbox.Clothing.Slots.Skin )
				Clothing.Clothing.Remove( piece );
		}
	}
}
