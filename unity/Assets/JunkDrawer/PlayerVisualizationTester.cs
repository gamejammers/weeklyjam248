//
// (c) GameJammers 2022
// http://www.jamming.games
//

using UnityEngine;

namespace Metal
{

public class PlayerVisualizationTester
	: MonoBehaviour
{
	//
	// fields /////////////////////////////////////////////////////////////////
	//

	public PlayerVisualController player; 
	public Camera cam											= null;
	public Transform cameraBoom									= null;
	public float moveSpeed 										= 10.0f;
	public float lookSpeed										= 10.0f;
	
	private CharacterController character;
	private Vector3 lookDir										= new Vector3(0f, 0f, 0f);

	private Vector3 throwPosition								= Vector3.zero;
	private Transform throwTarget								= null;

	private bool isJumping										= false;
	private float jumpElapsed									= 0f;

	//
	// unity callbacks ////////////////////////////////////////////////////////
	//

	protected virtual void Awake()
	{
		character = GetComponent<CharacterController>();
		throwTarget = null;
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
	}
	
	//
	// ------------------------------------------------------------------------
	//

	protected virtual void Update()
	{
		//
		// look
		//
        Vector3 deltaLook = new Vector3(
            -Input.GetAxis("Mouse Y") * lookSpeed,
            Input.GetAxis("Mouse X") * lookSpeed,
            0f
        );

        lookDir += deltaLook * Time.deltaTime;
        lookDir.x = Mathf.Clamp(lookDir.x, -90f, 90f);
        lookDir.y = lookDir.y % 360f;
        cameraBoom.transform.rotation = Quaternion.Euler(lookDir.x, lookDir.y, 0f);

		//
		// movement
		//
		Vector3 deltaMove = new Vector3(
			Input.GetAxisRaw("Horizontal"),
			0f,
			Input.GetAxisRaw("Vertical")
		).normalized;

		player.SetMoveDir(deltaMove);

		deltaMove = cam.transform.TransformDirection(deltaMove);
		deltaMove *= moveSpeed;

		if(Input.GetButtonDown("Jump") && !isJumping && character.isGrounded)
		{
			isJumping = true;
			jumpElapsed = 0f;
		}

		if(isJumping)
		{
			const float JUMP_AIR_TIME = 1f;
			const float JUMP_SPEED = 9.8f;
			float perc = jumpElapsed / JUMP_AIR_TIME;
			deltaMove += Vector3.up * JUMP_SPEED * (1.0f - perc);
			if(
				perc >= 2.0f ||
				(character.isGrounded && jumpElapsed > 0.1f)
			)
			{
				isJumping = false;
			}

			jumpElapsed += Time.deltaTime;
		}
		else
		{
			deltaMove += Physics.gravity;
		}

		//
		// shooty
		//
		if(Input.GetButtonDown("Fire1"))
			player.Fire();

		//
		// chain hook
		//
		if(Input.GetButton("Fire2"))
		{
			if(throwTarget == null)
			{
				GameObject go = new GameObject("ThrowTarget");
				go.transform.position = transform.position + cam.transform.forward + Vector3.up;
				player.StartThrow(go.transform);
				throwTarget = go.transform;
			}
		}
		else if(throwTarget != null)
		{
			player.FinishThrow();
			player.Catch();
			Destroy(throwTarget.gameObject);
			throwTarget = null;
		}

		character.Move(deltaMove * Time.deltaTime);
	}
}

}
