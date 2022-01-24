using Sandbox;

[Library("weapon_crowbar", Title = "Crowbar", Spawnable = true)]
partial class Crowbar : BaseDmWeapon
{
	public override string ViewModelPath => "models/crowbar/crowbar_reference.vmdl";

	public override int Bucket => 0;
	public override float PrimaryRate => 2.5f;
	public override float SecondaryRate => 2.0f;

	public override void Spawn()
	{
		base.Spawn();
		SetModel("models/citizen_props/crowbar01.vmdl");
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if (MeleeAttack())
		{
			OnMeleeHit();
		}
		else
		{
			OnMeleeMiss();
		}

		PlaySound( "iceaxe_swing1" );
	}

	public override void Simulate(Client cl)
	{
		if (cl == null)
			return;

		base.Simulate(cl);
	}

	public override bool CanReload()
	{
		return false;
	}

	private bool MeleeAttack()
	{
		var forward = Owner.EyeRot.Forward;
		forward = forward.Normal;

		bool hit = false;

		foreach (var tr in TraceBullet(Owner.EyePos, Owner.EyePos + forward * 80, 20.0f))
		{
			if (!tr.Entity.IsValid()) continue;

			tr.Surface.DoBulletImpact(tr);

			hit = true;

			if (!IsServer) continue;

			using (Prediction.Off())
			{
				var damageInfo = DamageInfo.FromBullet(tr.EndPos, forward * 100, 25)
					.UsingTraceResult(tr)
					.WithAttacker(Owner)
					.WithWeapon(this);

				tr.Entity.TakeDamage(damageInfo);
			}
		}

		return hit;
	}

	[ClientRpc]
	private void OnMeleeMiss()
	{
		Host.AssertClient();

		if (IsLocalPawn)
		{
			_ = new Sandbox.ScreenShake.Perlin();
		}

		ViewModelEntity?.SetAnimBool("attack", true);
	}

	[ClientRpc]
	private void OnMeleeHit()
	{
		Host.AssertClient();

		if (IsLocalPawn)
		{
			_ = new Sandbox.ScreenShake.Perlin(1.0f, 1.0f, 3.0f);
		}
		ViewModelEntity?.SetAnimBool("attack_hit", true);
	}

	public override void SimulateAnimator(PawnAnimator anim)
	{
		anim.SetParam("holdtype", 4);
		anim.SetParam("aimat_weight", 1.0f);
		anim.SetParam("holdtype_handedness", 1);
	}
}
