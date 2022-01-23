using Sandbox;

public partial class Carriable : BaseCarriable, IUse
{
	public PickupTrigger PickupTrigger { get; protected set; }

	public virtual int Bucket => 1;

	public override void Spawn()
	{
		base.Spawn();

		SetModel("weapons/rust_pistol/rust_pistol.vmdl");

		PickupTrigger = new PickupTrigger();
		PickupTrigger.Parent = this;
		PickupTrigger.Position = Position;
	}

	public override void CreateViewModel()
	{
		Host.AssertClient();

		if ( string.IsNullOrEmpty( ViewModelPath ) )
			return;

		ViewModelEntity = new ViewModel
		{
			Position = Position,
			Owner = Owner,
			EnableViewmodelRendering = true
		};

		ViewModelEntity.SetModel( ViewModelPath );
	}

	public bool OnUse( Entity user )
	{
		return false;
    }

    public virtual bool IsUsable(Entity user)
    {
        return Owner == null;
    }

	public override void OnCarryStart(Entity carrier)
	{
		base.OnCarryStart(carrier);

		if (PickupTrigger.IsValid())
		{
			PickupTrigger.EnableTouch = false;
		}
	}

	public override void OnCarryDrop(Entity dropper)
	{
		base.OnCarryDrop(dropper);

		if (PickupTrigger.IsValid())
		{
			PickupTrigger.EnableTouch = true;
		}
	}
}
