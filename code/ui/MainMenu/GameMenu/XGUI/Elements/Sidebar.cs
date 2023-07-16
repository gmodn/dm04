using Sandbox.UI;
public class Sidebar : TabContainer
{
	public Sidebar()
	{
		RemoveClass( "TabContainer" );
		AddClass( "Sidebar" );
	}
}
