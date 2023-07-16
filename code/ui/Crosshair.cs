
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

public class Crosshair : Panel
{
	int fireCounter;

	// Quick info crosshair
	public Label LeftFull;
	public Label Center;
	public Label RightFull;

	public Crosshair()
	{
		StyleSheet.Load( "ui/Crosshair.scss" );
		Center = Add.Label( "{", "crosshairBracketLeft" );
		LeftFull = Add.Label( "Q", "crosshairCenter" );
		RightFull = Add.Label( "}", "crosshairBracketRight" );
	}

	public override void Tick()
	{
		base.Tick();
		var player = Game.LocalPawn as DeathmatchPlayer;
		if ( player == null ) return;

		SetClass( "healthlow", player.Health < 40.0f );
		SetClass( "healthempty", player.Health <= 0.0f );

		SetClass( "ammolow", player.Health < 40.0f );
		SetClass( "ammoempty", player.Health < 40.0f );
	}

	[PanelEvent]
	public void FireEvent()
	{
		fireCounter += 2;
	}
}
