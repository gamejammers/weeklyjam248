//
// (c) GameJammers 2022
// http://www.jamming.games
//

using UnityEngine;
using System.Collections;

public class PlayerVisualController
	: MonoBehaviour
{
	//
	// fields /////////////////////////////////////////////////////////////////
	//

	public Animator animator									{ get; private set; }
	public LineRenderer line									{ get; private set; }

	[Header("Shooting")]
	public Rigidbody shellCasing								= null;
	public Transform ejectionPort								= null;
	public Transform ejectionDirection							= null;
	public float ejectionSpeed									= 10f;

	[Header("Axe Throwing")]
	public GameObject thrownAxePrefab							= null;
	public Renderer[] axeModels									= null;
	public int segmentCount										= 10;
	public float updateHz										= 10;

	public Transform throwTarget								{ get; private set; }
	private bool hiddenAxe										= false;
	private GameObject thrownAxe								= null;
	public Vector3[] segments									= null;

	private LevelController level								= null;

	//
	// public methods /////////////////////////////////////////////////////////
	//

	public void SetThrowTarget(Transform target)
	{
		throwTarget = target;
	}

	//
	// ------------------------------------------------------------------------
	//

	public void SetMoveDir(Vector3 dir)
	{
		animator.SetBool("isMoving", dir.sqrMagnitude > 0.01);
	}

	//
	// ------------------------------------------------------------------------
	//

	public void Fire()
	{
		animator.SetTrigger("fire");
	}

	//
	// ------------------------------------------------------------------------
	//

	public void StartThrow(Transform target)
	{
		hiddenAxe = false;
		animator.SetBool("isThrowing", true);
		SetThrowTarget(target);
	}
	
	//
	// ------------------------------------------------------------------------
	//
	
	public void FinishThrow()
	{
		animator.SetBool("isThrowing", false);
	}

	//
	// ------------------------------------------------------------------------
	//

	public void Catch()
	{
		animator.SetTrigger("caught");
		SetThrowTarget(null);
	}

	//
	// ------------------------------------------------------------------------
	//
	
	public void SwingAxe()
	{
		animator.SetTrigger("swing");
	}

	//
	// animation events ///////////////////////////////////////////////////////
	//

	private void ShowAxe()
	{
		hiddenAxe = true;
		foreach(Renderer rend in axeModels)
		{
			rend.enabled = true;
		}
	}

	//
	// ------------------------------------------------------------------------
	//
	
	private void HideAxe()
	{
		hiddenAxe = true;
		foreach(Renderer rend in axeModels)
		{
			rend.enabled = false;
		}
	}

	//
	// ------------------------------------------------------------------------
	//
	
	private void EjectShell()
	{
		Rigidbody shell = Instantiate(shellCasing, ejectionPort.position, ejectionPort.rotation);
		shell.AddForce(ejectionDirection.forward * ejectionSpeed);
	}	

	//
	// private methods ////////////////////////////////////////////////////////
	//

	IEnumerator LineUpdate()
	{
		var wait = new WaitForSeconds(1.0f / updateHz);
		while(true)
		{
			if(throwTarget != null && hiddenAxe)
			{
				if(thrownAxe == null)
				{
					thrownAxe = Instantiate(thrownAxePrefab, throwTarget.position, throwTarget.rotation);
				}

				thrownAxe.transform.position = throwTarget.position;
				thrownAxe.transform.rotation = Quaternion.LookRotation(thrownAxe.transform.position - transform.position);

				line.enabled = true;
	
				// TODO: Curve
				//	Vector3 diff = throwTarget.position - transform.position;
				//	float segmentLength = diff.magnitude / segmentCount;
				//	Vector3 norm = diff.normalized;
				//	
				//	// todo add some curves
				//	for(int i = 0; i < segmentCount; ++i)
				//	{
				//		Debug.Log("SETTING POINT: " + i);
				//		segments[i] = norm * segmentLength * i;
				//	}

				line.SetPositions(new Vector3[] {
					transform.position,
					throwTarget.transform.position
				});
			}
			else if(thrownAxe != null)
			{
				Destroy(thrownAxe);
				line.enabled = false;
			}

			yield return wait;
		}
	}
	

	//
	// unity callbacks ////////////////////////////////////////////////////////
	//

	protected virtual void Awake()
	{
		line = GetComponent<LineRenderer>();
		animator = GetComponent<Animator>();
		segments = new Vector3[segmentCount];

		StartCoroutine(LineUpdate());
	}

	//
	// ------------------------------------------------------------------------
	//

	protected virtual void Start()
	{
		level = LevelController.instance;
		if(level)
			level.RegisterPlayer(this);
	}

	//
	// ------------------------------------------------------------------------
	//

	protected virtual void Update()
	{
		if(Input.GetKeyDown(KeyCode.E) && level)
		{
			level.Use();
		}
	}
}
