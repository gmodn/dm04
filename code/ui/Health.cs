using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class Health : Panel
{
	public Label Label;
	public Label Label2;
	public float prevHealth = 0;
	public Health()
	{
		Label2 = Add.Label( "HEALTH", "text" );
		//Label2.Text = "HEALTH";
		Label = Add.Label( "100", "value" );
		StyleSheet.Load( "/ui/Health.scss" );
	}

	public override void Tick()
	{

		var player = Local.Pawn;
		if ( player == null ) return;




		Label.Text = $"{player.Health.CeilToInt()}";
		Label.SetClass( "danger", (player.Health <= 20.0f) );	
		Label.SetClass( "value", player.Health >= 20.0f );

		Label2.SetClass( "dangerMini", (player.Health <= 20.0f) );
		Label2.SetClass( "text", player.Health >= 20.0f );

		Label.SetClass( "glow", player.Health != prevHealth );
		Label.SetClass( "value", player.Health == prevHealth );
		prevHealth = player.Health;
	}
}
