using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;



	public partial class HLDMChatEntry : Panel
	{
		public Label NameLabel { get; internal set; }
		public Label Message { get; internal set; }

		public RealTimeSince TimeSinceBorn = 0;

		public HLDMChatEntry()
		{
			NameLabel = Add.Label( "Name", "name" );
			Message = Add.Label( "Message", "message" );
		}

		public override void Tick()
		{
			base.Tick();

			if ( TimeSinceBorn > 10 )
			{
				Delete();
			}
		}
}
