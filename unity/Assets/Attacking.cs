using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public GameObject ShotGunBullet;
    public float BulletSpeed;
    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
            ShotGunShot();
    }

    void ThrowAxe()
    {

    }

    void ShotGunShot()
    {
        Vector3 BulletAverage = firePoint.eulerAngles;
        for (int i = 0; i < 20; i++)
        {
            GameObject NewBullet = Instantiate(ShotGunBullet, firePoint);
            NewBullet.transform.position += transform.forward * 0.15f;
            NewBullet.transform.SetParent(transform.parent);
            Vector3 bulletAngles = BulletAverage + new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
            Debug.Log(bulletAngles);
            NewBullet.transform.eulerAngles = bulletAngles;
            NewBullet.GetComponent<Rigidbody>().AddForce(NewBullet.transform.forward * 10 * BulletSpeed);
        }
    }


}
