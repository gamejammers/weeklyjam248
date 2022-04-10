using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform Camera;
    public Rigidbody rb;
    public float Speed;
    public float sensitivity;
    public float JumpPower;
    public PlayerVisualController visuals;

    private float MouseX;
    private float MouseY;

    private float MaxSpeed;
    private bool Grounded;
    private bool Jumping;
    private float JumpCool;
    public bool Dashing;
    public float DashCool;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks for ground and than if it was found allow the player to try to jump
        GroundCheck();

        //Checks if the player is dashing and if dash cool is less than 0 than it dashes
        if (Input.GetButton("Dash") && DashCool < 0)
            StartCoroutine(Dash());
        else if (Dashing == false) //The player can't look around or move well dashing
        {
            //Calls the Move Function
            Move();
            //Calls the CameraMove FUnction
            RotatePlayer();
        }
    }

    void Move()
    {
        //Intakes the horizontal and vertical axis to move the player and sets those vaules to a vector3 that points where the player is facing
        float Forward = Input.GetAxisRaw("Vertical");
        float Horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 Direction = (transform.forward * Forward) + (transform.right * Horizontal);
        visuals.SetMoveDir(Direction);

        //Adds the direction in diffrent ways based on weather the player is falling or not
        if (Input.GetButton("Run"))
            MaxSpeed = Speed * 1.5f;
        else
            MaxSpeed = Speed;

        if (Mathf.Abs(rb.velocity.x) > MaxSpeed / 4 && Mathf.Abs(rb.velocity.z) > MaxSpeed / 4)
            rb.AddForce(Direction * MaxSpeed * 2);
        else
            rb.AddForce(Direction * MaxSpeed * 2);

        rb.velocity = new Vector3(rb.velocity.x * 0.98f, rb.velocity.y, rb.velocity.z * 0.98f);
    }

    //Moves the camera and is called in update
    void RotatePlayer()
    {
        //Intakes mouse x and mouse y than rotates the camera based on how it moved
        if (Input.GetAxis("Mouse X") > 0)
            transform.Rotate(0, sensitivity * Time.deltaTime * 50, 0);
        else if (Input.GetAxis("Mouse X") < 0)
            transform.Rotate(0, -sensitivity * Time.deltaTime * 50, 0);

        //Rotates the camera up and down 
        if (Input.GetAxis("Mouse Y") < 0 && MouseY < 80)
            MouseY += sensitivity * Time.deltaTime * 50;
        else if (Input.GetAxis("Mouse Y") > 0 && MouseY > -80)
            MouseY -= sensitivity * Time.deltaTime * 50;

        //Sets the camera after all the movement has been done
        Camera.position = transform.position;
        Camera.eulerAngles = new Vector3(MouseY, transform.eulerAngles.y, 0);
    }

    //Checks the ground and checks if you can jump
    void GroundCheck()
    {
        //Checks the ground by having a collider array and checking the tag of each collider
        Collider[] GroundCheck = Physics.OverlapSphere(transform.position - new Vector3(0, 1f, 0), 0.25f, LayerMask.GetMask("Default"));

        //Checks if any of the colliders was the ground 
        Grounded = false;
        for (int i = 0; i < GroundCheck.Length; i++)
        {
            if (GroundCheck[i].gameObject.name != gameObject.name)
                Grounded = true;
        }

        if (Grounded == true)
        {
            //Subtracts jump cooldown
            if (JumpCool >= 0)
                JumpCool -= Time.deltaTime;
            //Subtracts dash cooldown
            if (DashCool >= 0)
                DashCool -= Time.deltaTime;
        }

        //Jumps the player if they can 
        if (Input.GetButton("Jump") && JumpCool < 0)
            StartCoroutine(Jump());
    }

    //Jumps 
    IEnumerator Jump()
    {
        JumpCool = 0.25f;
        //Adds force 10 times quickly for a clean jump
        for (int i = 0; i < 5; i++)
        {
            rb.AddForce(new Vector3(0, JumpPower * 10, 0));
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Dash()
    {
        Dashing = true;
        DashCool = 1;
        float Forward = Input.GetAxisRaw("Vertical");
        Vector3 DashDirection;
        if (Forward >= 0)
            DashDirection = transform.forward * Speed * 3;
        else
            DashDirection = -transform.forward * Speed * 3;

        rb.velocity = new Vector3(DashDirection.x, -1, DashDirection.z);
        yield return new WaitForSeconds(0.25f);
        Dashing = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 1f, 0), 0.25f);
    }
}
