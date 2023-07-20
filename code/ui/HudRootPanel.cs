using Sandbox.UI;

public class DMHud : RootPanel
{
	public static DMHud Current;

	public Scoreboard Scoreboard { get; set; }

	public DMHud()
	{
		Current?.Delete();
		Current = null;

		StyleSheet.Load( "/resource/styles/hud.scss" );
		SetTemplate( "/resource/templates/hud.html" );

		AddChild<DamageIndicator>();
		AddChild<HitIndicator>();

		AddChild<InventoryBar>();
		AddChild<PickupFeed>();

		AddChild<Crosshair>();
		AddChild<HLDMChatBox>();
		AddChild<HLDMAlertBox>();
		AddChild<KillFeed>();
		Scoreboard = AddChild<Scoreboard>();
		AddChild<VoiceList>();

		Current = this;
	}

	public override void Tick()
	{
		base.Tick();
	}

	protected override void UpdateScale( Rect screenSize )
	{
		base.UpdateScale( screenSize );
	}
}
