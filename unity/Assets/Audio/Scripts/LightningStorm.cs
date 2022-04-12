//
// (c) GameJammers 2020
// http://www.jamming.games
//

using blacktriangles;
using System.Collections;
using UnityEngine;

namespace Adrift
{
    public class LightningStorm
        : MonoBehaviour
    {
        //
        // members ////////////////////////////////////////////////////////////
        //

        public float flashDuration                              = 0.5f;
        public Vector2 flashDelay                               = new Vector2(1f, 10f);

        public GameObject effect                                = null;
        public GameObject thunderAudioPrefab                    = null;
        public PulseLight bolt                                  = null;
        public Vector2 extents                                  = new Vector2(100f,100f);

		public AudioSource sfxSource;

        public AnimationCurve[] flashVariations                 = null;

        //
        // private methods ////////////////////////////////////////////////////
        //
        
        private IEnumerator UpdateStorm()
        {
            var wait = new WaitForSeconds(flashDuration);
            while(true)
            {
                yield return new WaitForSeconds(btRandom.Range(flashDelay.x, flashDelay.y));

                effect.transform.localPosition = 
                    new Vector3(
                        btRandom.Range(-extents.x, extents.x), 
                        transform.position.y,
                        btRandom.Range(-extents.y, extents.y));

                effect.transform.LookAt(Vector3.zero);

                bolt.curve = flashVariations.Random();
                effect.SetActive(true);

				Instantiate(thunderAudioPrefab, transform.position, transform.rotation);

                yield return wait;

                effect.SetActive(false);
            }
        }

        //
        // unity callbacks ////////////////////////////////////////////////////
        //

        protected virtual void Awake()
        {
            effect.SetActive(false);
        }

        //
        // --------------------------------------------------------------------
        //

        protected virtual void Start()
        {
            StartCoroutine(UpdateStorm());
        }
        
    }
}
