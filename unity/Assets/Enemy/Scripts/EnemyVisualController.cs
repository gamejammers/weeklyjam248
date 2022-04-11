//
// (c) GameJammers 2022
// http://www.jamming.games
//

using UnityEngine;
using System.Collections.Generic;

public abstract class EnemyVisualController
	: MonoBehaviour
{

	//
	// interface methods ////////////////////////////////////////////////////////////////////////////
	//
	

	virtual public void SetMoveDir(Vector3 dir)
	{
	}

	//
	// ------------------------------------------------------------------------
	//
	
	virtual public void Attack()
	{
	}
	
	//
	// ------------------------------------------------------------------------
	//

	virtual public void TakeDamage()
	{
	}

	//
	// ------------------------------------------------------------------------
	//
	
	virtual public void SetDead()
	{
	}
}
