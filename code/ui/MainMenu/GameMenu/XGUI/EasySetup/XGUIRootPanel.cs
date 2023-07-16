using Sandbox.UI;
namespace XGUI;
public class XGUIRootPanel : RootPanel
{
	public static XGUIRootPanel Current;

	public XGUIRootPanel()
	{
		Current = this;
		Style.Width = Length.Percent( 100 );
		Style.Height = Length.Percent( 100 );
	}

	public override void Tick()
	{
		base.Tick();
	}

	protected override void UpdateScale( Rect screenSize )
	{
		Scale = 1;
		if ( screenSize.Width > 1920 ) Scale = 1.50f;
		if ( screenSize.Width > 2200 ) Scale = 2.00f;
	}
}
