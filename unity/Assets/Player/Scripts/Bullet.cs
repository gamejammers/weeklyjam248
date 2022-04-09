using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Hit " + col.gameObject.name);
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyHealth>().TakeDamage(Damage);
        }
        Destroy(gameObject);
    }
}
