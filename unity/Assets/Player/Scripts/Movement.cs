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

    private float MouseX;
    private float MouseY;

    private float MaxSpeed;
    private bool Grounded;
    private bool Jumping;
    private bool CanJump;
    private float JumpCool;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks for ground and than if it was found reduce the JumpCool
        GroundCheck();

        //Calls the Move Function
        Move();

        //Calls the CameraMove FUnction
        RotatePlayer();
    }

    void Move()
    {
        //Intakes the horizontal and vertical axis to move the player and sets those vaules to a vector3 that points where the player is facing
        float Forward = Input.GetAxisRaw("Vertical");
        float Horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 Direction = (transform.forward * Forward) + (transform.right * Horizontal);

        //Adds the direction in diffrent ways based on weather the player is falling or not
        if (Input.GetButton("Run"))
            MaxSpeed = Speed * 3;
        else
            MaxSpeed = Speed * 2;

        if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(Direction.x) * Speed && Mathf.Abs(rb.velocity.z) < Mathf.Abs(Direction.z) * Speed)
            rb.velocity = new Vector3(Direction.x * Speed, rb.velocity.y, Direction.z * Speed);



        rb.AddForce(Direction * MaxSpeed);
        if (Grounded == true)
        {
            if (Mathf.Abs(rb.velocity.x) > MaxSpeed || Mathf.Abs(rb.velocity.z) > MaxSpeed)
            {
                float XZ = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);
                float XVelocity = Mathf.Abs(rb.velocity.x) / XZ;
                float ZVelocity = Mathf.Abs(rb.velocity.z) / XZ;

                rb.velocity = new Vector3(Direction.x * XVelocity * MaxSpeed, rb.velocity.y, Direction.z * ZVelocity * MaxSpeed);
            }
        }
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
        Collider[] GroundCheck = Physics.OverlapSphere(transform.position - new Vector3(0, 1f, 0), 0.25f);

        //Checks if any of the colliders was the ground 
        Grounded = false;
        for (int i = 0; i < GroundCheck.Length; i++)
        {
            if (GroundCheck[i].gameObject.name != gameObject.name)
                Grounded = true;
        }

        if (Grounded == true)
            if (JumpCool > 0)
            {
                JumpCool -= Time.deltaTime;
                CanJump = false;
            }
            else
                CanJump = true;

        //Jumps the player if they can 
        if (Input.GetButton("Jump") && CanJump == true)
        {
            CanJump = false;
            StartCoroutine(Jump());
        }
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 1f, 0), 0.25f);
    }
}
