using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeWeapon : MonoBehaviour
{
    public Attacking attacking;
    public int Damage;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyHealth>().TakeDamage(Damage);
        }
        attacking.ThrowTime = 3;
        Debug.Log("Collided");
    }
}
