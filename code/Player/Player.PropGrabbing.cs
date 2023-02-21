using Sandbox;
using System;
using System.Linq;

public partial class DeathmatchPlayer
{
	private PhysicsBody holdBody;

	public PhysicsBody HeldBody { get; private set; }
	public Rotation HeldRot { get; private set; }
	public ModelEntity HeldEntity { get; private set; }
	public Vector3 HeldPos { get; private set; }
	public Vector3 HoldPos { get; private set; }
	public Rotation HoldRot { get; private set; }

	private float CarryLimit = 2200.0f;

	private TimeSince timeSinceDrop;

	private Entity lastActiveChild;

	public void SimulateGrabbing()
	{
		if ( !Game.IsServer )
			return;

		if ( Input.Pressed( InputButton.Use ) && !HeldBody.IsValid())
		{
			if( HeldBody.IsValid() )
				GrabEnd();
			
			using ( Prediction.Off() )
			{
				if ( timeSinceDrop < 0.5f )
					return;

				var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 80 )
					.UseHitboxes()
					.Ignore( this, false )
					.Radius( 4.0f )
					.Run();

				if ( !tr.Entity.IsValid() )
					return;

				if ( !tr.Body.IsValid() )
					return;

				if ( tr.Entity.IsWorld )
					return;

				if ( tr.Entity.PhysicsGroup == null )
					return;

				if ( tr.Body.Mass >= CarryLimit )
					return;

				if ( tr.Entity is Prop prop )
				{
					lastActiveChild = ActiveChild;
					ActiveChild = null;
					GrabStart( prop, tr.Body, EyePosition + EyeRotation.Forward * 80, EyeRotation );
				}
			}
		}

		if( HeldBody.IsValid() || HeldEntity.IsValid())
		{
			if(GroundEntity == HeldEntity)
			{
				GrabEnd();
				ActiveChild = lastActiveChild;
			}

			if ( Input.Pressed( InputButton.PrimaryAttack ) && HeldBody.IsValid() )
			{
				timeSinceDrop = 0;
				HeldBody.ApplyImpulse( EyeRotation.Forward * (HeldBody.Mass * 250.0f) );
				GrabEnd();

				ActiveChild = lastActiveChild;

			}
			else if ( Input.Pressed( InputButton.SecondaryAttack ) )
			{
				timeSinceDrop = 0;
				GrabEnd();

				ActiveChild = lastActiveChild;
			}

			GrabMove( EyePosition, EyeRotation.Forward, EyeRotation);
		} 
		else
		{
			if ( lastActiveChild != null )
			{
				ActiveChild = lastActiveChild;
				lastActiveChild = null;
			}

			GrabEnd();
		}
	}

	private void GrabStart( ModelEntity entity, PhysicsBody body, Vector3 grabPos, Rotation grabRot )
	{
		if ( !body.IsValid() )
			return;

		if ( body.PhysicsGroup == null )
			return;

		GrabEnd();

		HeldBody = body;
		HeldPos = HeldBody.LocalMassCenter;
		HeldRot = grabRot.Inverse * HeldBody.Rotation;

		HoldPos = HeldBody.Position;
		HoldRot = HeldBody.Rotation;

		HeldBody.Sleeping = false;
		HeldBody.AutoSleep = false;

		HeldEntity = entity;

		Client?.Pvs.Add( HeldEntity );
	}

	private void GrabEnd()
	{
		holdBody?.Remove();
		holdBody = null;

		if ( HeldBody.IsValid() )
		{
			HeldBody.AutoSleep = true;
		}

		if ( HeldEntity.IsValid() )
		{
			Client?.Pvs.Remove( HeldEntity );
		}

		HeldBody = null;
		HeldRot = Rotation.Identity;
		HeldEntity = null;
	}

	private void GrabMove( Vector3 startPos, Vector3 dir, Rotation rot )
	{
		if ( !HeldBody.IsValid() )
			return;

		var attachPos = HeldBody.FindClosestPoint( startPos );
		var holdDistance = 50.0f + attachPos.Distance( HeldBody.MassCenter );

		holdBody.Position = startPos + dir * holdDistance;
		holdBody.Rotation = rot * HeldRot;
	}
}
