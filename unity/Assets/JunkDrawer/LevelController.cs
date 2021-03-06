//
// (c) GameJammers 2022
// http://www.jamming.games
//

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelController
	: MonoBehaviour
{
	//
	// fields /////////////////////////////////////////////////////////////////
	//

	static public LevelController instance						{ get; private set; }
	public PlayerVisualController player						{ get; private set; }
	
	public Transform teletarget									= null;
	public Transform elevatorButton								= null;
	public Transform button2									= null;
	public Animator elevatorDoors								= null;
	public Jukebox jukebox										= null;
	public AudioClip elevatorMusic								= null;
	public AudioClip heavyMusic									= null;
	public AudioClip elevatorDoorSfx							= null;
	public AudioClip elevatorDingSfx							= null;

	public CanvasGroup canvasGroup								= null;

	public Transform demon;
	public Transform demonStart;
	public Transform demonEnd;
	public AudioClip demonSfx;


	bool doorsOpen = false;
	bool elevatorDone = false;
	bool finished = false;
	bool restart = false;
	bool doDemonMotion = false;

	//
	// public methods /////////////////////////////////////////////////////////
	//

	public void RegisterPlayer(PlayerVisualController player)
	{
		this.player = player;
	}

	//
	// ------------------------------------------------------------------------
	//
	
	public void Use()
	{
		const float ACTIVATION_DISTANCE = 10f;
		float dist2 = (elevatorButton.transform.position - player.transform.position).sqrMagnitude;
		if( dist2 < ACTIVATION_DISTANCE && !doorsOpen)
		{
			jukebox.player.PlayOneShot(elevatorDoorSfx);
			elevatorDoors.SetTrigger("openElevatorDoor");
			doorsOpen = true;
		}

		dist2 = (button2.transform.position - player.transform.parent.position).sqrMagnitude;
		if(doorsOpen && dist2 < ACTIVATION_DISTANCE && !elevatorDone)
		{
			elevatorDoors.SetTrigger("closeElevatorDoor");
			StartCoroutine(ElevatorTrip());
		}
	}

	//
	// private methods ////////////////////////////////////////////////////////
	//

	private IEnumerator ElevatorTrip()
	{

		var controller = player.transform.parent.GetComponent<CharacterController>();
		var rb = player.transform.parent.GetComponent<Rigidbody>();
		Vector3 diff = button2.transform.position - player.transform.position;
		if(controller != null)
		{
			controller.enabled = false;
			controller.transform.position = teletarget.position - diff;
			controller.enabled = true;
		}
		else if(rb != null)
		{
			rb.isKinematic = true;
			rb.transform.position = teletarget.position - diff;
			rb.isKinematic = false;
		}
		else
		{
			player.transform.position = teletarget.position - diff;
		}

		elevatorDone = true;
		var wait = new WaitForSeconds(0.1f);

		const int FADEOUT_TIME = 10;

		float startVolume = jukebox.player.volume;
		for(int i = 0; i < FADEOUT_TIME; ++i)
		{
			float perc = 1 - (i / (float)FADEOUT_TIME);
			jukebox.player.volume = startVolume * perc;
			yield return wait;
		}

		jukebox.player.Stop();
		jukebox.player.clip = elevatorMusic;
		jukebox.player.Play();
		for(int i = 0; i < FADEOUT_TIME; ++i)
		{
			float perc = i / (float)FADEOUT_TIME;
			jukebox.player.volume = startVolume * perc;
			yield return wait;
		}

		// wait for elevator music
		const float ELEVATOR_WAIT_TIME = 28f;
		yield return new WaitForSeconds(ELEVATOR_WAIT_TIME);
		
		jukebox.player.volume = 0f;

		yield return new WaitForSeconds(0.5f);
		jukebox.player.PlayOneShot(elevatorDingSfx);
		yield return new WaitForSeconds(1f);

		jukebox.player.clip = heavyMusic;
		jukebox.player.volume = startVolume;
		jukebox.player.Play();

		jukebox.player.PlayOneShot(elevatorDoorSfx);
		elevatorDoors.SetTrigger("roofElevatorDoorOpen");

		yield break;
	}

	//
	// ------------------------------------------------------------------------
	//

	private IEnumerator EndGame()
	{
		const float FADE_IN_TIME = 5f;
		const float WAIT_TIME = 0.1f;
		var wait = new WaitForSeconds(WAIT_TIME);
		float elapsed = 0f;

		const float FLY_IN_TIME = 1.5f;
		while(elapsed < FLY_IN_TIME)
		{
			float perc = elapsed / FLY_IN_TIME;
			demon.position = Vector3.Lerp(demonStart.position, demonEnd.position, perc);
			demon.rotation = Quaternion.Lerp(demonStart.rotation, demonEnd.rotation, perc);
			yield return new WaitForSeconds(0f);
			elapsed += Time.deltaTime;
		}

		demon.position = demonEnd.position;
		demon.rotation = demonEnd.rotation;
		jukebox.player.PlayOneShot(demonSfx);

		doDemonMotion = true;

		yield return new WaitForSeconds(1f);

		while(elapsed < FADE_IN_TIME)
		{
			yield return wait;
			elapsed += WAIT_TIME;
			float perc = elapsed / FADE_IN_TIME;
			canvasGroup.alpha = perc;
		}

		finished = true;

		yield return new WaitForSeconds(5f);
		yield return SceneManager.LoadSceneAsync(0);
	}

	//
	// unity callbacks ////////////////////////////////////////////////////////
	//

	protected virtual void Awake()
	{
		instance = this;
	}

	//
	// ------------------------------------------------------------------------
	//

	protected virtual void Update()
	{
		if(doDemonMotion)
		{
			demon.position = demonEnd.position + new Vector3( 
				Mathf.Sin(Time.time)*10.0f,
				Mathf.Cos(Time.time)*10.0f,
				Mathf.Sin(Time.time)*10.0f
			);
		}
	}
	

	//
	// ------------------------------------------------------------------------
	//

	protected virtual void OnTriggerEnter(Collider other)
	{
		if(!finished && canvasGroup.alpha <= 0f)
		{
			StartCoroutine(EndGame());
		}
	}
	
}
