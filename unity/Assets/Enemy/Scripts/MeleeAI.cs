using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : MonoBehaviour
{
    //How fast the enemy moves
    public float Speed;
    //The distance before the enemy sees the player
    public float Sight;
    //The range before the enemy attacks
    public float AttackRange;
    //The speed at which the attack recharges
    public float AttackSpeed;

    private float AttackCool;
    private bool InRange;
    private Transform Target;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Sets down the attack cooldown if it's above 0
        if (AttackCool > 0)
            AttackCool -= Time.deltaTime * AttackSpeed;

        //Checks for the player so see if it is in Range and also if the enemy has a line of sight
        bool SeePlayer = PlayerCheck();

        //if the enemy has a line of sight it will move twoards the player and attempt to attack
        if (SeePlayer)
        {
            //checks to see if the target to attack is in range and if not it will move twoards them
            bool InRange = MoveTwoards(Target.position);

            //If the enemy is inrange and if the attack cool is less than 0
            if (InRange && AttackCool < 0)
                Attack();
        }

    }

    //Checks how close the enemy is to the player and if close enough returns true to ba able to attack else it returns false and moves twoard the target
    private bool MoveTwoards(Vector3 TargetPos)
    {

        Vector3 direction = Target.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newQuaternion = Quaternion.Euler(new Vector3(0, angle, 0));
        transform.rotation = Quaternion.Slerp(transform.rotation, newQuaternion, 4 * Time.deltaTime);

        Vector3 Direction = (transform.forward);
        float MaxSpeed = Speed * 2;

        //Gets the total dirction than checks if that direction is within the range of the target
        float TotalDirection = Mathf.Abs(Direction.x) + Mathf.Abs(Direction.z);
        if (TotalDirection < AttackRange)
        {
            //Sets the velocity to 0 well the enemy atempts to attack
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return true;
        }


        if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(Direction.x) * Speed && Mathf.Abs(rb.velocity.z) < Mathf.Abs(Direction.z) * Speed)
            rb.velocity = Direction * Speed;

        rb.AddForce(Direction * MaxSpeed);

        if (Mathf.Abs(rb.velocity.x) > MaxSpeed || Mathf.Abs(rb.velocity.z) > MaxSpeed)
        {
            float XZ = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);
            float XVelocity = Mathf.Abs(rb.velocity.x) / XZ;
            float ZVelocity = Mathf.Abs(rb.velocity.z) / XZ;

            rb.velocity = new Vector3(Direction.x * XVelocity * MaxSpeed, rb.velocity.y, Direction.z * ZVelocity * MaxSpeed);
        }
        rb.velocity = new Vector3(rb.velocity.x * 0.98f, rb.velocity.y, rb.velocity.z * 0.98f);
        return false;
    }

    //If the raycast hits the player and the enemy has a line of sight than the player will move twoards
    private bool PlayerCheck()
    {
        //Checks a sphere around the enemy for the player
        Collider[] PlayerCheck = Physics.OverlapSphere(transform.position, Sight);

        foreach (Collider collider in PlayerCheck)
        {
            if (collider.tag == "Player")
            {
                Target = collider.transform;
                Vector3 direction = (collider.transform.position - transform.position).normalized;
                Ray ray = new Ray(transform.position, direction);
                RaycastHit hit;

                Debug.DrawRay(transform.position, direction, Color.red);

                if (Physics.Raycast(ray, out hit))
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                        return true;
            }
        }
        Target = null;
        return false;
    }

    void Attack()
    {
        Debug.Log("Attacking player");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, Sight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 0.8f, 0.8f);
    }
}