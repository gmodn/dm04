


partial class DeathmatchGame : GameManager
{
	public static GameStates CurrentState => (Current as DeathmatchGame)?.GameState ?? GameStates.Warmup;

	[Net]
	public RealTimeUntil StateTimer { get; set; } = 0f;

	[Net]
	public GameStates GameState { get; set; } = GameStates.Warmup;
	[Net]
	public string NextMap { get; set; } = "facepunch.datacore";

	[ConVar.Replicated]
	public static int hldm_gamelength { get; set; } = 10;

	[ConCmd.Admin]
	public static void SkipStage()
	{
		if ( Current is not DeathmatchGame dmg ) return;

		dmg.StateTimer = 1;
	}

	[ConCmd.Server( "hldm_setgamelength" )]
	public static void ChangeGameLength( int length )
	{
		hldm_gamelength = length;
	}

	private async Task WaitStateTimer()
	{
		while ( StateTimer > 0 )
		{
			await Task.DelayRealtimeSeconds( 1.0f );
		}

		// extra second for fun
		await Task.DelayRealtimeSeconds( 1.0f );
	}

	private async Task GameLoopAsync()
	{
		
		GameState = GameStates.Warmup;
		StateTimer = 10;
		await WaitStateTimer();

		GameState = GameStates.Live;
		StateTimer = hldm_gamelength * 60;
		FreshStart();
		await WaitStateTimer();

		GameState = GameStates.GameEnd;
		StateTimer = 10.0f;
		await WaitStateTimer();

		Game.ChangeLevel( "facepunch.crossfire" );
	}

	private bool HasEnoughPlayers()
	{
		if ( All.OfType<DeathmatchPlayer>().Count() < 2 )
			return false;

		return true;
	}

	private void FreshStart()
	{
		foreach ( var cl in Game.Clients )
		{
			cl.SetInt( "kills", 0 );
			cl.SetInt( "deaths", 0 );
		}

		All.OfType<DeathmatchPlayer>().ToList().ForEach( x =>
		{
			x.Respawn();
		} );
	}

	public enum GameStates
	{
		Warmup,
		Live,
		GameEnd,
		MapVote
	}

}
