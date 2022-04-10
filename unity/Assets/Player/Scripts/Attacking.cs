using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    //The bullets made and speed of the bullets shot
    public GameObject ShotGunBullet;
    public float BulletSpeed;
    public float BulletSpread;

    //Spawn position of the shot gun and axe
    public Transform AxePoint;
    public Transform ThrowPoint;
    public Transform BulletSpawn;
    private AxeThrow AxeThrow;

    //Animators for the weapons
    public PlayerVisualController visuals;

    //Variables for throwing time and weather or not you are throwing
    public float ThrowTime;
    public bool CanAttack;
    public bool Throwing;

    void Awake()
    {
        AxeThrow = AxePoint.GetComponent<AxeThrow>();
    }

    // Update is called once per frame
    void Update()
    {
        //Shot gun shooting detection
        if (Input.GetButtonDown("Fire2") && CanAttack == false)
            StartCoroutine(ShotGunShot());

        //The attacking fot the axe
        //Clicking the button swings it
        if (Input.GetButtonUp("Fire1") && CanAttack == false)
            SwingAxe();

        //Holding the button throws the axe
        if (Input.GetButton("Fire1") && CanAttack == true)
        {
            //A timer to confirm throwing
            ThrowTime += Time.deltaTime;
            if (ThrowTime > 0.2f)
            {
                visuals.StartThrow(transform);
                if (Throwing == false && ThrowTime > 0.8f)
                    ThrowAxe();
            }

            if (ThrowTime > 3)
                AxeThrow.AxeBack(ThrowPoint.position);
        }
        else if (Throwing == true) //if the player lets go of the throw button and the axe comes back
            AxeThrow.AxeBack(ThrowPoint.position);

        //checks if the axes is not hel by the player and than if it is held by the player it sets the local position and if not draws the line for the chain
        if (Throwing == true)
            AxeThrow.DrawChain();
    }

    //Function for doing multiple things for thorwing the axe
    void ThrowAxe()
    {
        CanAttack = true;
        Throwing = true;
        visuals.FinishThrow();
        AxePoint.GetChild(0).gameObject.SetActive(true);
        //throw axe animation
        AxePoint.parent = transform.parent;
        AxeThrow.AxeForward(ThrowPoint.forward);
    }

    //After throwing it sets the axe to it's help mode
    public void AxeToHold()
    {
        Throwing = false;
        CanAttack = false;
        visuals.Catch();
        AxePoint.GetChild(0).gameObject.SetActive(false);
        AxePoint.parent = transform;
        ThrowTime = 0;
    }

    //Swings the axe needs more work
    void SwingAxe()
    {
        AxeToHold();
    }

    //Shoots the shotgun
    IEnumerator ShotGunShot()
    {
        visuals.Fire();
        CanAttack = true;
        //Sets the average angle of the bullets
        Vector3 BulletAverage = BulletSpawn.eulerAngles;
        for (int i = 0; i < 20; i++)
        {
            //Shoots multiple bullets and set the angle of each bullet and send them forward
            GameObject NewBullet = Instantiate(ShotGunBullet, BulletSpawn);
            NewBullet.transform.SetParent(transform.parent);
            Vector3 bulletAngles = BulletAverage + new Vector3(Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread));
            NewBullet.transform.eulerAngles = bulletAngles;
            NewBullet.GetComponent<Rigidbody>().AddForce(NewBullet.transform.forward * 10 * BulletSpeed);
        }
        yield return new WaitForSeconds(0.3f);
        CanAttack = false;
    }

    public override bool Equals(object obj)
    {
        return obj is Attacking attacking &&
               base.Equals(obj) &&
               ThrowTime == attacking.ThrowTime;
    }
}
