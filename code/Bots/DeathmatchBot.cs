public class DeathmatchBot : Bot
{
	[ConCmd.Admin( "HLDM_CreateBot", Help = "Spawn A Deathmatch Bot." )]
	internal static void SpawnDMBot()
	{
		Game.AssertServer();

		// Create an instance of your custom bot.
		_ = new DeathmatchBot();
	}

	public override void BuildInput()
	{
		Input.AnalogMove = Vector3.Forward;
		Input.AnalogLook = new Angles( 0, 30 * Time.Delta, 0 );

		(Client.Pawn as Entity).BuildInput();
		// Make this actually do something at some point in time okay thanks
	}

	public override void Tick()
	{

	}
}
