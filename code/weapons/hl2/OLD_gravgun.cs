using Sandbox;


[Library( "OLD_gravgun", Title = "ZERO-POINT ENERGY GUN (GRAVITY GUN)" )]
[Hammer.EditorModel( "models/weapons/hl2_gravgun/w_hl2_gravgun.vmdl" )]
partial class OLD_gravgun : BaseDmWeapon
{
	public override string ViewModelPath => "models/weapons/hl2_gravgun/v_hl2_gravgun.vmdl";
	public override float PrimaryRate => 2.0f;
	public override AmmoType AmmoType => AmmoType.None;
	public override int Bucket => 0;


	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/weapons/hl2_gravgun/w_hl2_gravgun.vmdl" );
	}

	public override bool CanReload()
	{
		return false;
	}

	public override void AttackPrimary()
	{
		if ( MeleeAttack() )
		{
			OnMeleeHit();
		}
	}

	private bool MeleeAttack()
	{
		var forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;

		bool hit = false;

		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + forward * 150, 0.2f ) )
		{
			if ( (!tr.Entity.IsValid()) || (tr.Entity.IsWorld) )
			{
				PlaySound( "hl2_gravgun.dryfire" );
				ViewModelEntity?.SetAnimParameter( "dryfire", true );
				continue;
			}

			//tr.Surface.DoBulletImpact( tr );

			hit = true;

			if ( !IsServer ) continue;

			using ( Prediction.Off() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 300, 0 )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
			}
		}

		return hit;
	}

	[ClientRpc]
	private void OnMeleeHit()
	{
		Host.AssertClient();

		PlaySound( "hl2_gravgun.launch" );
		ViewModelEntity?.SetAnimParameter( "fire", true );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 2 ); // TODO this is shit
		//anim.SetAnimParameter( "aimat_weight", 1.0f );

		var forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;
		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + forward * 150, 0.2f ) )
		{
			ViewModelEntity?.SetAnimParameter( "prongs", ((!tr.Entity.IsValid()) || (tr.Entity.IsWorld)) );
		}
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		TimeSinceDeployed = 0;

		IsReloading = false;
	}
}
