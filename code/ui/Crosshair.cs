
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

public class Crosshair : Panel
{
	int fireCounter;

	// Quick info crosshair
	public Label LeftFull;
	public Label centre;
	public Label RightFull;

	public Crosshair()
	{
		StyleSheet.Load( "ui/Crosshair.scss" );
		centre = Add.Label( "{", "crosshairBracket" );
		LeftFull = Add.Label( "Q", "crosshair" );
		RightFull = Add.Label( "}", "crosshairBracket" );
	}

	public override void Tick()
	{
		base.Tick();

		SetClass( "fire", fireCounter > 0 );

		if ( fireCounter > 0 )
			fireCounter--;
	}

	[PanelEvent]
	public void FireEvent()
	{
		fireCounter += 2;
	}
}
