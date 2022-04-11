//
// (c) GameJammers 2022
// http://www.jamming.games
//

using UnityEngine;
using System.Collections;

public class ZombieVisualController
	: EnemyVisualController
{
	//
	// fields /////////////////////////////////////////////////////////////////
	//

	public Animator animator									{ get; private set; }

	//
	// public methods /////////////////////////////////////////////////////////
	//

	override public void SetMoveDir(Vector3 dir)
	{
		base.SetMoveDir(dir);
		animator.SetBool("isMoving", dir.sqrMagnitude > 0.01);
	}

	//
	// ------------------------------------------------------------------------
	//
	
	override public void Attack()
	{
		base.Attack();
		animator.SetTrigger("attack");
	}
	
	//
	// ------------------------------------------------------------------------
	//

	override public void TakeDamage()
	{
		base.TakeDamage();
		animator.SetTrigger("damage");
	}

	//
	// ------------------------------------------------------------------------
	//
	
	override public void SetDead()
	{
		base.SetDead();
		animator.SetTrigger("dead");
	}

	//
	// unity callbacks ////////////////////////////////////////////////////////
	//

	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
	}
	
}
