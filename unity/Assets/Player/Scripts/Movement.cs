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
    private PlayerSounds Sounds;

    private float MouseX;
    private float MouseY;

    private bool Grounded;
    private bool Jumping;
    private float JumpCool;
    public bool Dashing;
    public float DashCool;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Sounds = GetComponent<PlayerSounds>();
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
        if (Direction.x > 1)
            Direction.x = 1;

        if (Direction.z > 1)
            Direction.z = 1;
        visuals.SetMoveDir(Direction);

        //The speed used for the player when moving
        float MaxSpeed;

        ///Adds the direction in diffrent ways based on weather the player is falling or not
        if (Input.GetButton("Run"))
            MaxSpeed = Speed * 1.5f;
        else
            MaxSpeed = Speed;


        rb.AddForce(Direction * Speed * 4f);
        if (Mathf.Abs(rb.velocity.x) > MaxSpeed || Mathf.Abs(rb.velocity.z) > MaxSpeed)
            rb.velocity = new Vector3(Direction.x * MaxSpeed, rb.velocity.y, Direction.z * MaxSpeed);

        rb.velocity = new Vector3(rb.velocity.x * 0.97f, rb.velocity.y, rb.velocity.z * 0.97f);
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
        Camera.position = transform.position + Vector3.up * 2.0f;
        Camera.eulerAngles = new Vector3(MouseY, transform.eulerAngles.y, 0);
    }

    //Checks the ground and checks if you can jump
    void GroundCheck()
    {
        //Checks the ground by having a collider array and checking the tag of each collider
        Collider[] GroundCheck = Physics.OverlapSphere(transform.position - new Vector3(0, 1f, 0), 0.25f, LayerMask.GetMask("Default"));

        //Checks if any of the colliders was the ground 
        Grounded = false;
        foreach (Collider Check in GroundCheck)
        {
            if (Check.gameObject.name != gameObject.name && !Check.isTrigger)
            {
                Grounded = true;
                Debug.Log(Check.gameObject.name);
            }
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
        Sounds.Play_Jump();
    }

    IEnumerator Dash()
    {
        Dashing = true;
        DashCool = 1;
        float Forward = Input.GetAxisRaw("Vertical");
        float Horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 DashDirection = (transform.forward * Forward * Speed * 3) + (transform.right * Horizontal * Speed * 3);

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
