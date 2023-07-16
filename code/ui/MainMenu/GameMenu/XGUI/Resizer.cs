using Sandbox.UI;
namespace XGUI;
public class Resizer : Panel
{
	public Resizer()
	{

		AddClass( "Resizer" );
	}
	bool draggingR = false;
	bool draggingL = false;
	bool draggingT = false;
	bool draggingB = false;

	protected override void OnMouseDown( MousePanelEvent e )
	{
		base.OnMouseDown( e );

		draggingB = true;
		draggingR = true;
		//draggingT = true;
		//draggingL = true;
		xoff1 = (float)((FindRootPanel().MousePosition.x) - Parent.Box.Rect.Right);
		yoff1 = (float)((FindRootPanel().MousePosition.y) - Parent.Box.Rect.Bottom);
		xoff2 = (float)((FindRootPanel().MousePosition.x) - Parent.Box.Rect.Left);
		yoff2 = (float)((FindRootPanel().MousePosition.y) - Parent.Box.Rect.Top);
	}
	protected override void OnMouseUp( MousePanelEvent e )
	{
		base.OnMouseUp( e );
		draggingB = false;
		draggingR = false;
		draggingT = false;
		draggingL = false;
	}
	float xoff1 = 0;
	float yoff1 = 0;
	float xoff2 = 0;
	float yoff2 = 0;
	protected override void OnMouseMove( MousePanelEvent e )
	{
		base.OnMouseMove( e );

		if ( draggingB )
		{
			//Parent.Style.Width = (FindRootPanel().MousePosition.x - Parent.Box.Rect.Left) - xoff;
			Parent.Style.Height = (FindRootPanel().MousePosition.y - Parent.Box.Rect.Top) - yoff1;
		}

		if ( draggingR )
		{
			Parent.Style.Width = (FindRootPanel().MousePosition.x - Parent.Box.Rect.Left) - xoff1;
			//Parent.Style.Height = (FindRootPanel().MousePosition.y - Parent.Box.Rect.Top) - yoff;
		}
		if ( draggingT )
		{
			//Parent.Style.Width = (FindRootPanel().MousePosition.x - Parent.Box.Rect.Left) - xoff;
			Parent.Style.Height = (FindRootPanel().MousePosition.y - Parent.Box.Rect.Top) - yoff2;
		}

		if ( draggingL )
		{
			Parent.Style.Width = (FindRootPanel().MousePosition.x - Parent.Box.Rect.Left) - xoff2;
			(Parent as Window).Position.x = (FindRootPanel().MousePosition.x - Parent.Box.Rect.Left) - xoff2;
			//Parent.Style.Height = (FindRootPanel().MousePosition.y - Parent.Box.Rect.Top) - yoff2;
		}
	}
}
