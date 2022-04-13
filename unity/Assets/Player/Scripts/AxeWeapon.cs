using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeWeapon : MonoBehaviour
{
    public Attacking attacking;
    public LayerMask HitableMask;
    public int Damage;
    public bool Hit;
    public Vector3 AxeHitBox;
    public float OffSet;

    void Update()
    {
        if (Hit == false)
        {
            CollisionCheck();
        }
    }

    void CollisionCheck()
    {
        Collider[] EnemyCheck = Physics.OverlapBox(transform.position - (transform.forward * OffSet), AxeHitBox, Quaternion.identity, HitableMask);
        foreach (Collider Check in EnemyCheck)
        {
            if (Check.isTrigger == false)
            {
                Debug.Log("Returning from" + Check.gameObject.name);
                Hit = true;
                attacking.ThrowTime = 100;
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
        Gizmos.DrawWireCube(transform.position - (transform.forward * OffSet), AxeHitBox);
    }
}
