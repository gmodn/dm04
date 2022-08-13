using Sandbox;
using Sandbox.UI;

public class DeathmatchHud : RootPanel
{
	public static DeathmatchHud Current;

	public Scoreboard Scoreboard { get; set; }

	public DeathmatchHud()
	{
		//If we already have a hud, delete and nullify the old hud
		//so we can start a new one
		if(Current != null)
		{
			Current?.Delete();
			Current = null;
		}

		Current = this;

		StyleSheet.Load( "/resource/styles/hud.scss" );
		SetTemplate( "/resource/templates/hud.html" );

		AddChild<DamageIndicator>();
		AddChild<HitIndicator>();

		AddChild<InventoryBar>();
		AddChild<PickupFeed>();

		AddChild<ChatBox>();
		AddChild<Ammo>();
		AddChild<Health>();
		AddChild<KillFeed>();
		AddChild<Crosshair>();
		Scoreboard = AddChild<Scoreboard>();
		AddChild<VoiceList>();
	}

	public override void Tick()
	{
		base.Tick();

		SetClass( "game-end", DeathmatchGame.CurrentState == DeathmatchGame.GameStates.GameEnd );
		SetClass( "game-warmup", DeathmatchGame.CurrentState == DeathmatchGame.GameStates.Warmup );
	}

	protected override void UpdateScale( Rect screenSize )
	{
		base.UpdateScale( screenSize );
	}
}
