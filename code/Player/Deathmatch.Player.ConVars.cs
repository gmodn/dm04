
// This file is used for player settings

public partial class DeathmatchPlayer
{
	[ConVar.Replicated] public bool dm04_goldsrcmovement { get; set; } = true;
	[ConVar.Replicated] public bool dm04_enableclothing { get; set; } = false;
}
