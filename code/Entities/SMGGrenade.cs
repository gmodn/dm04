﻿partial class SMGGrenade : BasePhysics
{
	public SMGGrenade()
	{
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		Tags.Add( "projectile" );
	}

	public override void Spawn()
	{
		base.Spawn();

		//SetModel( "models/weapons/hl2_grenade/w_hl2_grenade.vmdl" );

		//Use temporary model until we get the proper smg nade in
		SetModel( "models/weapons/hl2_smg1/hl2_smg1_grenade.vmdl" );
	}

	[GameEvent.Tick.Server]
	public void Simulate()
	{
		var trace = Trace.Ray( Position, Position )
			.Size( 24 )
			.Ignore( this )
			.Ignore( Owner )
			.Run();

		Position = trace.EndPosition;

		if ( trace.Hit == true )
			BlowUp();
	}
	public void BlowUp()
	{
		DeathmatchGame.Explosion( this, Owner, Position, 250, 100, 1.0f );
		Delete();
	}
}
