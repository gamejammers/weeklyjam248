//
// (c) GameJammers 2022
// http://www.jamming.games
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroPlayer
	: MonoBehaviour
{
	//
	// fields /////////////////////////////////////////////////////////////////
	//

	public CanvasGroup titleScreen								= null;
	public CanvasGroup fadeToBlack								= null;
	public CanvasGroup[] story									= null;
	public AudioSource musicSource								= null;
	public AudioClip introMusic									= null;

	private bool next 											= false;

	//
	// callback methods ///////////////////////////////////////////////////////
	//

	public void StartCutscene()
	{
		musicSource.clip = introMusic;
		musicSource.Play();
		StartCoroutine(RunCutscene());
	}

	//
	// ----------------------------------------------------------------------------
	//

	protected IEnumerator RunCutscene()
	{
		const float WAIT_TIME = 0.05f;
		const float FADE_TIME = 1.0f;
		const float DELAY = 23.0f / 8f;

		var wait = new WaitForSeconds(WAIT_TIME);
		var delay = new WaitForSeconds(DELAY);

		CanvasGroup current = titleScreen;
		
		foreach(CanvasGroup cg in story)
		{
			//
			// fade out
			//
			float elapsed = 0f;
			while(elapsed < FADE_TIME)
			{
				float perc = elapsed / FADE_TIME;
				current.alpha = 1.0f - perc;
				yield return wait;
				elapsed += WAIT_TIME;
			}
			current.alpha = 0;
			current.interactable = false;
			current.blocksRaycasts = false;

			//
			// fade in
			//
			current = cg;
			elapsed = 0f;
			while(elapsed < FADE_TIME)
			{
				float perc = elapsed / FADE_TIME;
				current.alpha = perc;
				yield return wait;
				elapsed += WAIT_TIME;
			}
			current.alpha = 1;
			current.interactable = true;
			current.blocksRaycasts = true;

			//
			// wait
			//
			elapsed = 0f;
			while(!next && elapsed < DELAY) 
			{
				yield return wait;
				elapsed += WAIT_TIME;
			}

			next = false;
		}

		//
		// fade in
		//
		const float FADE_TO_BLACK = 7f;
		float el = 0f;
		while(el < FADE_TO_BLACK)
		{
			float perc = el / FADE_TO_BLACK;
			fadeToBlack.alpha = perc;
			yield return wait;
			el += WAIT_TIME;
		}
		

		yield return SceneManager.LoadSceneAsync(1);
	}

	//
	// ------------------------------------------------------------------------
	//

	public virtual void Next()
	{
		next = true;
	}

	//
	// unity callbacks ////////////////////////////////////////////////////////
	//

	protected virtual void Awake()
	{
		Cursor.lockState = CursorLockMode.None;

		foreach(CanvasGroup cg in story)
		{
			cg.alpha = 0f;
			cg.interactable = false;
			cg.blocksRaycasts = false;
		}
		titleScreen.alpha = 1f;
		titleScreen.interactable = true;
		titleScreen.blocksRaycasts = true;
	}
	
}
