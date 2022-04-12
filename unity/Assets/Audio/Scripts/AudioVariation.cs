//
// (c) BLACKTRIANGLES 2020
// http://www.blacktriangles.com
//

using UnityEngine;

namespace blacktriangles
{
    public class AudioVariation
        : MonoBehaviour
    {
        //
        // types //////////////////////////////////////////////////////////////
        //

        public enum PlayCondition
        {
            Manual,
            Awake,
            Enable
        }
        
        //
        // members ////////////////////////////////////////////////////////////
        //

        public AudioSource source                               = null;
        public AudioClip[] clips                                = null;
        public PlayCondition playCondition                      = PlayCondition.Manual;

        //
        // public methods /////////////////////////////////////////////////////
        //

        public virtual void Play()
        {
            source.clip = clips.Random();
            source.Play();
        }

        //
        // unity callbacks ////////////////////////////////////////////////////
        //

        protected virtual void Awake()
        {
            if(playCondition == PlayCondition.Awake)
            {
                Play();
            }
        }
        
        //
        // --------------------------------------------------------------------
        //

        protected virtual void OnEnable()
        {
            if(playCondition == PlayCondition.Enable)
            {
                Play();
            }
        }
    }
}
