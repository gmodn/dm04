using Sandbox;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class DeathmatchBot : AnimatedEntity
{
	// it's nothing lol
	// nvm not anymore

	DamageInfo LastDamage;

	/// <summary>
	/// A generic corpse entity
	/// </summary>
	public ModelEntity Corpse { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		Tags.Add( "npc" );
	}

	public override void TakeDamage( DamageInfo info )
	{
		LastDamage = info;

		// hack - hitbox group 1 is head
		// we should be able to get this from somewhere (it's pretty specific to citizen though?)
		if ( info.Hitbox.HasTag( "head" ) )
		{
			info.Damage *= 2.0f;
		}

		this.ProceduralHitReaction( info );

		base.TakeDamage( info );
	}

	public override void OnKilled()
	{
		base.OnKilled();

		if ( LastDamage.HasTag( "blast" ) )
		{
			using ( Prediction.Off() )
			{
				var particles = Particles.Create( "particles/gib.vpcf" );
				if ( particles != null )
				{
					particles.SetPosition( 0, Position + Vector3.Up * 40 );
				}
			}
		}
		else
		{
			BecomeRagdollOnClient( LastDamage.Force, LastDamage.BoneIndex );
		}
	}

	[ClientRpc]
	void BecomeRagdollOnClient( Vector3 force, int forceBone )
	{
		var ent = new ModelEntity();
		ent.Tags.Add( "ragdoll", "solid", "debris" );
		ent.Position = Position;
		ent.Rotation = Rotation;
		ent.Scale = Scale;
		ent.UsePhysicsCollision = true;
		ent.EnableAllCollisions = true;
		ent.SetModel( GetModelName() );
		ent.CopyBonesFrom( this );
		ent.CopyBodyGroups( this );
		ent.CopyMaterialGroup( this );
		ent.CopyMaterialOverrides( this );
		ent.TakeDecalsFrom( this );
		ent.EnableAllCollisions = true;
		ent.SurroundingBoundsMode = SurroundingBoundsType.Physics;
		ent.RenderColor = RenderColor;
		ent.PhysicsGroup.Velocity = force;
		ent.PhysicsEnabled = true;

		foreach ( var child in Children )
		{
			if ( !child.Tags.Has( "clothes" ) ) continue;
			if ( child is not ModelEntity e ) continue;

			var model = e.GetModelName();

			var clothing = new ModelEntity();
			clothing.SetModel( model );
			clothing.SetParent( ent, true );
			clothing.RenderColor = e.RenderColor;
			clothing.CopyBodyGroups( e );
			clothing.CopyMaterialGroup( e );
		}

		Corpse = ent;

		if ( IsLocalPawn )
			Corpse.EnableDrawing = false;

		ent.DeleteAsync( 10.0f );
	}

	[ClientRpc]
	public virtual void PlaySoundOnClient( string sound )
	{
		//PlaySound( "zombie.hurt" );
		Sound.FromWorld( sound, Position + Vector3.Up * 60 );
		//SetAnimParameter( "b_talking", true );
	}

	public virtual void DamagedEffects()
	{
		Velocity *= 0.1f;
		if ( Health > 0 )
			PlaySoundOnClient( "zombie.hurt" );
	}
}
