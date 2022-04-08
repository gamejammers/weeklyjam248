using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float sensitivity;
    public float Speed;
    public Transform Camera;
    private Rigidbody rb;

    private float MouseY;
    private float MaxSpeed;
    private bool Grounded;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        RotatePlayer();
    }

    //Movement function
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
            rb.velocity = Direction * Speed;



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
        if (Input.GetAxis("Mouse Y") < 0 && MouseY < 45)
            MouseY += sensitivity * Time.deltaTime * 50;
        else if (Input.GetAxis("Mouse Y") > 0 && MouseY > -45)
            MouseY -= sensitivity * Time.deltaTime * 50;

        //Sets the camera after all the movement has been done
        Camera.position = transform.position;
        Camera.eulerAngles = new Vector3(MouseY, transform.eulerAngles.y, 0);
    }
}
