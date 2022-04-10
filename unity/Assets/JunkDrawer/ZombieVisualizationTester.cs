//
// (c) GameJammers 2022
// http://www.jamming.games
//

using UnityEngine;
using System.Collections.Generic;

namespace Metal
{

public class ZombieVisualizationTester
	: MonoBehaviour
{

	public ZombieVisualController zombie;
	private bool isMoving = false;

	protected virtual void OnGUI()
	{
		if(GUILayout.Button("Toggle Move"))
		{
			isMoving = !isMoving;
			zombie.SetMoveDir( isMoving ? Vector3.one : Vector3.zero );
		}

		if(GUILayout.Button("Zombie Attack"))
		{
			zombie.Attack();
		}
		
		if(GUILayout.Button("Take Damage"))
		{
			zombie.TakeDamage();
		}

		if(GUILayout.Button("Dead"))
		{
			zombie.SetDead();
		}
	}
}

}
