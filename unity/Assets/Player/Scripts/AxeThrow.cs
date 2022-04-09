using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeThrow : MonoBehaviour
{
    public float AxeSpeed;
    public int Damage;
    public Attacking attacking;

    private LineRenderer Chain;
    private Vector3[] ChainPoints = new Vector3[2];
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Chain = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    //Adds a forward force on the axe
    public void AxeForward(Vector3 Direction)
    {
        rb.velocity = Direction * AxeSpeed;
    }

    //Function for when the axe returns and is called as many times as needed before the axe is back
    public void AxeBack(Vector3 player)
    {
        GetComponent<Animator>().SetBool("Returning", true);
        attacking.ThrowTime = 4;
        Vector3 Direction = transform.position - player;
        float TotalDirection = Mathf.Abs(Direction.x) + Mathf.Abs(Direction.y) + Mathf.Abs(Direction.z);
        if (TotalDirection < 2f)
            attacking.AxeToHold();
        Direction = new Vector3(Direction.x / TotalDirection, Direction.y / TotalDirection, Direction.z / TotalDirection);
        rb.velocity = -Direction * AxeSpeed;
    }

    //Draws the chain line 
    public void DrawChain()
    {
        ChainPoints[0] = transform.position;
        ChainPoints[1] = transform.position;

        Chain.SetPositions(ChainPoints);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyHealth>().TakeDamage(Damage);
        }
        AxeBack(attacking.transform.position);
    }
}