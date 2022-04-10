//
// (c) GameJammers 2022
// http://www.jamming.games
//

using UnityEngine;
using System.Collections;

public class ZombieVisualController
	: MonoBehaviour
{
	//
	// fields /////////////////////////////////////////////////////////////////
	//

	public Animator animator									{ get; private set; }

	//
	// public methods /////////////////////////////////////////////////////////
	//

	public void SetMoveDir(Vector3 dir)
	{
		animator.SetBool("isMoving", dir.sqrMagnitude > 0.01);
	}

	//
	// ------------------------------------------------------------------------
	//
	
	public void Attack()
	{
		animator.SetTrigger("attack");
	}
	
	//
	// ------------------------------------------------------------------------
	//

	public void TakeDamage()
	{
		animator.SetTrigger("damage");
	}

	//
	// ------------------------------------------------------------------------
	//
	
	public void SetDead()
	{
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
