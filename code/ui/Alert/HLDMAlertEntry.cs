using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;



	public partial class HLDMAlertEntry : Panel
	{
		public Label Message { get; internal set; }

		public RealTimeSince TimeSinceBorn = 0;

		public HLDMAlertEntry()
		{
			Message = Add.Label( "Message", "message" );
		}

		public override void Tick()
		{
			base.Tick();
		}
}
