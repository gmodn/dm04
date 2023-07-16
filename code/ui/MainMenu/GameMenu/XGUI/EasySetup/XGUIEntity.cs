using Sandbox;
namespace XGUI;
public partial class XGUIEntity : HudEntity<XGUIRootPanel>
{
	public static XGUIEntity Current;

	public XGUIEntity()
	{
		Current = this;
	}
}
