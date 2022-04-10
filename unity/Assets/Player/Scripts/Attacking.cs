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
    public Transform BulletSpawn;

    //Animators for the weapons
    public PlayerVisualController visuals;

	// TRUE if we are actively attacking
	bool InAttack = false;

	// axe throwing has several different stages
	private enum AxeState
	{
		Ready,  // we are ready to throw the axe
		CheckSwing, // we delay a bit to see if this is a swing or a throw
		SwingAxe, // it was a swing, do the swing
		StartThrowing, // spawn the objects, let loose!
		Throwing, // the axe is thrown, and is going away from us
		Returning, // the axe is returning to us
		Catch, // finish the catch animation before we do more stuff
	}
	private AxeState axeState = AxeState.Ready;

	//Variables for throwing time and weather or not you are throwing
    public float ThrowTime = 0f;

	// This object represents where the axe is in space.  We pass it to the
	// visuals, so it can draw the axe and the chain.  We can also use it to
	// detect collisions and deal damage.
	private GameObject axeTracker = null;

    // Update is called once per frame
    void Update()
    {
        //Shot gun shooting detection
        if (Input.GetButtonDown("Fire2") && InAttack == false)
            StartCoroutine(ShotGunShot());

		UpdateAxe();
    }

	private void UpdateAxe()
	{
		ThrowTime += Time.deltaTime;
		switch(axeState)
		{
			//
			// we are not using the axe.  start swinging logic
			// 
			case AxeState.Ready:
				if(Input.GetButton("Fire1"))
					axeState = AxeState.CheckSwing;
			break;

			//
			// we ar swinging, if the player releases teh buttong, swing!
			// if they keep it held, we are throwing
			//
			case AxeState.CheckSwing:
				if(Input.GetButtonUp("Fire1"))
				{
					ThrowTime = 0f;
					axeState = AxeState.SwingAxe;
				}
				else if(ThrowTime > 0.2f)
				{
					ThrowTime = 0f;
					axeState = AxeState.StartThrowing;
				}
			break;

			//
			// if it was a swing, update it here
			//
			case AxeState.SwingAxe:
				InAttack = true;
				//visuals.SwingAxe(); // todo swing axe visuals
				const float SWING_AXE_DELAY = 2f; // todo make this a variable
				if(ThrowTime > SWING_AXE_DELAY)
				{
					InAttack = false;
					axeState = AxeState.Ready;
				}
			break;

			//
			// if it was a throw, start throwing
			//
			case AxeState.StartThrowing:
				InAttack = true;
				if(axeTracker == null)
				{
					axeTracker = new GameObject("This Empty Game Object Represents the Position of the Axe");
					visuals.StartThrow(axeTracker.transform);
				}

				if(ThrowTime > 1f)
				{
					ThrowTime = 0f;
					axeTracker.transform.position = transform.position + Vector3.up + transform.forward;
					axeTracker.transform.rotation = transform.rotation;
					axeState = AxeState.Throwing;
				}
			break;

			//
			// the axe is in flight, send it away until it reaches the furthest distance
			// or we release the button
			//
			case AxeState.Throwing:
				const float MAX_THROW_TIME = 2f; // todo throw variable
				if(ThrowTime > MAX_THROW_TIME || !Input.GetButton("Fire1"))
				{
					ThrowTime = 0f;
					visuals.FinishThrow();
					axeState = AxeState.Returning;
				}
				else
				{
					const float AXE_TRAVEL_SPEED = 10f;
					axeTracker.transform.position += axeTracker.transform.forward * AXE_TRAVEL_SPEED * Time.deltaTime;
				}
			break;

			//
			// the axe is returning to us, wait until it reaches us, then trigger the catch
			//
			case AxeState.Returning:
				Vector3 diff = transform.position - axeTracker.transform.position;
				float dist2 = diff.sqrMagnitude;
				if(dist2 < 1f)
				{
					ThrowTime = 0;
					visuals.Catch();
					Destroy(axeTracker);
					axeState = AxeState.Catch;
				}
				else
				{
					const float AXE_RETURN_SPEED = 10f;
					axeTracker.transform.position += diff.normalized * AXE_RETURN_SPEED * Time.deltaTime;
				}
			break;

			case AxeState.Catch:
				const float CATCH_DELAY_TIME = 2f;
				if(ThrowTime > CATCH_DELAY_TIME)
				{
					axeState = AxeState.Ready;
					InAttack = false;
				}
			break;
		}
	}

    //Shoots the shotgun
    IEnumerator ShotGunShot()
    {
        visuals.Fire();
        InAttack = true;
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
        yield return new WaitForSeconds(0.5f);
        InAttack = false;
    }
}
