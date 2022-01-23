using Sandbox;
using System;
using System.Linq;

public class ClothingEntity : ModelEntity
{

}

partial class DeathmatchPlayer
{
	public Clothing.Container Clothing { get; protected set; }

	/// <summary>
	/// Set the clothes to whatever the player is wearing
	/// </summary>
	public void UpdateClothes( Client cl )
	{
		Clothing ??= new();
		Clothing.LoadFromClient( cl );
	}
}
