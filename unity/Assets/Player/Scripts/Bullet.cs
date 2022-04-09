using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;
    public float BulletDuration;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("BulletEnd", BulletDuration);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyHealth>().TakeDamage(Damage);
        }
        Destroy(gameObject);
    }

    void BulletEnd()
    {
        Destroy(gameObject);
    }
}
