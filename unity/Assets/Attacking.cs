
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    //The bullets made and speed of the bullets shot
    public GameObject ShotGunBullet;
    public float BulletSpeed;

    //Spawn position of the shot gun and axe
    public Transform ShotGunPoint;
    public Transform AxePoint;
    private AxeThrow AxeThrow;

    //Animators for the weapons
    private Animator ShotGunAnimator;
    private Animator AxeAnimator;

    //Variables for throwing time and weather or not you are throwing
    public float ThrowTime;
    public bool Throwing;

    //Called to set all the componets that are used
    private void Awake()
    {
        ShotGunAnimator = ShotGunPoint.GetComponent<Animator>();
        AxeAnimator = AxePoint.GetComponent<Animator>();
        AxeThrow = AxePoint.GetComponent<AxeThrow>();
    }

    // Update is called once per frame
    void Update()
    {
        //Shot gun shooting detection
        if (Input.GetButtonDown("Fire2"))
            ShotGunShot();

        //The attacking fot the axe
        //Clicking the button swings it
        if (Input.GetButtonUp("Fire1") && Throwing == false)
            SwingAxe();

        //Holding the button throws the axe
        if (Input.GetButton("Fire1") && ThrowTime < 1.2f)
        {
            //A timer to confirm throwing
            ThrowTime += Time.deltaTime;
            if (ThrowTime > 0.2f)
                if (Throwing == false)
                    ThrowAxe();
        }
        else if (Throwing == true) //if the player lets go of the throw button and the axe comes back
            AxeThrow.AxeBack(transform.position);

        //checks if the axes is not hel by the player and than if it is held by the player it sets the local position and if not draws the line for the chain
        if (Throwing == true)
            AxeThrow.DrawChain();
        else
            AxePoint.localPosition = new Vector3(0.3f, -0.3f, 0.3f);
    }

    //Function for doing multiple things for thorwing the axe
    void ThrowAxe()
    {
        Throwing = true;
        AxeAnimator.SetBool("Held", false);
        AxePoint.parent = transform.parent;
        AxeAnimator.Play("AxeThrow");
        AxeThrow.AxeForward(AxePoint.forward);
    }

    //After throwing it sets the axe to it's help mode
    public void AxeToHold()
    {
        AxeAnimator.SetBool("Held", true);
        AxePoint.parent = ShotGunPoint.parent;
        ThrowTime = 0;
        Throwing = false;
    }

    //Swings the axe needs more work
    void SwingAxe()
    {
        AxeToHold();
        AxeAnimator.Play("AxeSwing");
    }

    //Shoots the shotgun
    void ShotGunShot()
    {
        //Sets the average angle of the bullets
        Vector3 BulletAverage = ShotGunPoint.eulerAngles;
        for (int i = 0; i < 20; i++)
        {
            //Shoots multiple bullets and set the angle of each bullet and send them forward
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
