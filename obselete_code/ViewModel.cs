using Sandbox;
using System;
using System.Linq;

partial class DmViewModel : BaseViewModel
{
	float walkBob = 0;

	public override void PostCameraSetup( ref CameraSetup camSetup )
	{
		base.PostCameraSetup( ref camSetup );

		// camSetup.ViewModelFieldOfView = camSetup.FieldOfView + (FieldOfView - 80);

		AddCameraEffects( ref camSetup );
	}

	private void AddCameraEffects( ref CameraSetup camSetup )
	{
		//Disabled for now because it kinda looks funky
		//Rotation = Local.Pawn.EyeRotation;
		//
		//
		// Bob up and down based on our walk movement
		//
		//var speed = Owner.Velocity.Length.LerpInverse( 0, 320 );
		//var left = camSetup.Rotation.Left;
		//var up = camSetup.Rotation.Up;

		//if ( Owner.GroundEntity != null )
		//{
		//	walkBob += Time.Delta * 25.0f * speed;
		//}
		//
		//Position += left * MathF.Sin( walkBob * 0.4f ) * speed * 1f;
	}
}
