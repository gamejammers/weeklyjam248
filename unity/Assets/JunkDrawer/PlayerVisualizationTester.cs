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
	private Vector3 lookDir;

	private Vector3 throwPosition								= Vector3.zero;
	private Transform throwTarget								= null;

	//
	// unity callbacks ////////////////////////////////////////////////////////
	//

	protected virtual void Awake()
	{
		character = GetComponent<CharacterController>();
		throwTarget = null;
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
		);

		player.SetMoveDir(deltaMove);

		deltaMove = cam.transform.TransformDirection(deltaMove);
		deltaMove *= moveSpeed;
		deltaMove += Physics.gravity;

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
