using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    //The bullets made and speed of the bullets shot
    public GameObject ShotGunBullet;
    public AxeWeapon AxePrefab;
    public float BulletSpeed;
    public float BulletSpread;
    public int SwingDamage;
    public Transform bulletSpawn;

    //Spawn position of the shot gun and axe
    public Transform SightPos;

    //Animators for the weapons
    public PlayerVisualController visuals;
    private PlayerSounds Sounds;

    // TRUE if we are actively attacking
    bool CanAttack = true;

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
    private AxeWeapon axeTracker = null;

    void Awake()
    {
        Sounds = GetComponent<PlayerSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        //Axe state Updater
        UpdateAxe();

        //Shot gun shooting detection
        if (Input.GetButton("Fire1") && CanAttack)
            StartCoroutine(ShotGunShot());
    }

    private void UpdateAxe()
    {
        switch (axeState)
        {
            //
            // we are not using the axe.  start swinging logic
            // 
            case AxeState.Ready:
                if (Input.GetButton("Fire2") && CanAttack)
                {
                    CanAttack = false;
                    ThrowTime = 0f;
                    axeState = AxeState.CheckSwing;
                }
                break;

            //
            // we ar swinging, if the player releases teh buttong, swing!
            // if they keep it held, we are throwing
            //
            case AxeState.CheckSwing:
                if (!Input.GetButton("Fire2"))
                {
                    ThrowTime = 0f;
                    axeState = AxeState.SwingAxe;
                    visuals.SwingAxe(); // todo swing axe visuals

                    if (!CanAttack)
                        AxeHit();
                }
                else if (ThrowTime > 0.2f)
                {
                    ThrowTime = 0f;
                    axeTracker = null;
                    axeState = AxeState.StartThrowing;
                }
                break;

            //
            // if it was a swing, update it here
            //
            case AxeState.SwingAxe:
                const float SWING_AXE_DELAY = 1.2f; // todo make this a variable
                if (ThrowTime > SWING_AXE_DELAY)
                {
                    CanAttack = true;
                    axeState = AxeState.Ready;
                }
                else
                    CanAttack = false;
                break;

            //
            // if it was a throw, start throwing
            //
            case AxeState.StartThrowing:
                CanAttack = false;
                if (axeTracker == null)
                {
                    ThrowTime = 0f;
                    axeTracker = Instantiate(AxePrefab, SightPos.position, SightPos.rotation) as AxeWeapon;
                    axeTracker.attacking = this;
                    axeTracker.Hit = false;
                    visuals.StartThrow(axeTracker.transform);
                    Sounds.Play_AxeSwing();
                }

                if (ThrowTime > 1f)
                {
                    Sounds.Play_AxeChains();
                    Debug.Log("Axe Spawned at" + SightPos.position);
                    ThrowTime = 0f;
                    axeTracker.transform.position = SightPos.position;
                    axeTracker.transform.rotation = SightPos.rotation;
                    axeState = AxeState.Throwing;
                }
                break;

            //
            // the axe is in flight, send it away until it reaches the furthest distance
            // or we release the button
            //
            case AxeState.Throwing:
                const float MAX_THROW_TIME = 2f; // todo throw variable
                if (ThrowTime > MAX_THROW_TIME || !Input.GetButton("Fire2"))
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
                Vector3 diff = SightPos.position - axeTracker.transform.position;
                float dist2 = (Mathf.Abs(diff.x) + Mathf.Abs(diff.y) + Mathf.Abs(diff.z));// orginal was diff.sqrMagnitude;
                if (dist2 < 0.07f)
                {
                    ThrowTime = 0;
                    visuals.Catch();
                    Destroy(axeTracker.gameObject);
                    axeTracker = null;
                    axeState = AxeState.Catch;
                    Sounds.Play_AxeChains();
                }
                else
                {
                    const float AXE_RETURN_SPEED = 10f;
                    axeTracker.transform.position += diff.normalized * AXE_RETURN_SPEED * Time.deltaTime;
                }
                break;

            case AxeState.Catch:
                const float CATCH_DELAY_TIME = 1.5f;
                if (ThrowTime > CATCH_DELAY_TIME)
                {
                    axeState = AxeState.Ready;
                    CanAttack = true;
                }
                break;
        }
        ThrowTime += Time.deltaTime;
    }

    void AxeHit()
    {
        Sounds.Play_AxeSwing();
        Collider[] EnemyCheck = Physics.OverlapBox(transform.position + transform.forward * 0.5f, new Vector3(0.3f, 0.7f, 1), Quaternion.identity, LayerMask.GetMask("Enemy"));

        foreach (Collider Check in EnemyCheck)
        {
            Check.gameObject.GetComponent<EnemyHealth>().TakeDamage(SwingDamage);
        }
    }

    //Shoots the shotgun
    IEnumerator ShotGunShot()
    {
        visuals.Fire();
        CanAttack = false;
        //Sets the average angle of the bullets
        Vector3 BulletAverage = SightPos.eulerAngles;
        for (int i = 0; i < 20; i++)
        {
            Sounds.Play_ShotGunShot();
            //Shoots multiple bullets and set the angle of each bullet and send them forward
            GameObject NewBullet = Instantiate(ShotGunBullet, bulletSpawn);
            NewBullet.transform.SetParent(transform.parent);
            Vector3 bulletAngles = BulletAverage + new Vector3(Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread), Random.Range(-BulletSpread, BulletSpread));
            NewBullet.transform.eulerAngles = bulletAngles;
            NewBullet.GetComponent<Rigidbody>().AddForce(NewBullet.transform.forward * 10 * BulletSpeed);
        }
        yield return new WaitForSeconds(1.6f);
        CanAttack = true;
    }
}
