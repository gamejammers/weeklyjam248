using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public GameObject ShotGunBullet;
    public float BulletSpeed;
    public Transform ShotGunPoint;
    public Transform AxePoint;

    private float ThrowTime;
    private bool Throwing;
    private Animator AxeAnimator;
    private Animator ShotGunAnimator;

    private LineRenderer Chain;
    private Vector3[] ChainPoints = new Vector3[2];

    private void Awake()
    {
        ShotGunAnimator = ShotGunPoint.GetComponent<Animator>();
        AxeAnimator = AxePoint.GetComponent<Animator>();
        Chain = AxePoint.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
            ShotGunShot();

        if (Input.GetButtonUp("Fire1") && Throwing == false)
            SwingAxe();

        if (Input.GetButton("Fire1"))
        {
            ThrowTime += Time.deltaTime;
            if (ThrowTime > 0.5)
                ThrowAxe();
        }
        if (Throwing == true)
            DrawChain();
        else
            AxePoint.localPosition = new Vector3(0.3f, -0.3f, 0.3f);
    }

    void SwingAxe()
    {
        AxeAnimator.Play("AxeSwing");
    }


    void ThrowAxe()
    {
        AxeAnimator.Play("AxeThrow");
        AxePoint.GetComponent<Rigidbody>().velocity = transform.forward * 8;
    }

    void DrawChain()
    {
        ChainPoints[0] = transform.position;
        ChainPoints[1] = AxePoint.position;

        Chain.SetPositions(ChainPoints);
        Vector3 Distance = transform.position - AxePoint.position;
        float TotalDistance = Mathf.Abs(Distance.x) + Mathf.Abs(Distance.y) + Mathf.Abs(Distance.z);
    }

    void ShotGunShot()
    {
        Vector3 BulletAverage = ShotGunPoint.eulerAngles;
        for (int i = 0; i < 20; i++)
        {
            GameObject NewBullet = Instantiate(ShotGunBullet, ShotGunPoint);
            NewBullet.transform.position += transform.forward * 0.15f;
            NewBullet.transform.SetParent(transform.parent);
            Vector3 bulletAngles = BulletAverage + new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
            Debug.Log(bulletAngles);
            NewBullet.transform.eulerAngles = bulletAngles;
            NewBullet.GetComponent<Rigidbody>().AddForce(NewBullet.transform.forward * 10 * BulletSpeed);
        }
    }
}
