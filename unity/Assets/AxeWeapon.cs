using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeWeapon : MonoBehaviour
{
    public Attacking attacking;
    public int Damage;
    public bool Hit;

    void Update()
    {
        if (Hit == false)
        {
            CollisionCheck();
        }
    }

    void CollisionCheck()
    {
        Debug.Log("checking for collisions at " + transform.position);
        Collider[] EnemyCheck = Physics.OverlapBox(transform.position + transform.forward * 0.5f, new Vector3(0.3f, 0.7f, 1));
        foreach (Collider Check in EnemyCheck)
        {
			// don't collide with self or the player
			if(Check.gameObject != this.gameObject && Check.gameObject.tag != "Player")
			{
            	Debug.Log("COLLISION: " + Check.gameObject.name);
            	Hit = true;
            	attacking.ThrowTime = 3;
            	if (Check.gameObject.tag == "Enemy")
            	{
            	    Check.gameObject.GetComponent<EnemyHealth>().TakeDamage(Damage);
            	}
			}
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + transform.forward * 0.5f, new Vector3(0.3f, 0.7f, 1));
    }
}
