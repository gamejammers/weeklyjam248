//
// (c) BLACKTRIANGLES 2020
// http://www.blacktriangles.com
//

using System.Collections;
using UnityEngine;

namespace blacktriangles
{
    public class PulseLight
        : MonoBehaviour
    {
        //
        // members ////////////////////////////////////////////////////////////
        //

        public Light target;
        public AnimationCurve curve;
        public bool resetOnEnable;
        public WrapMode wrapMode;

        private float baseIntensity = 1f;
        private float startTime = 0f;

        //
        // unity callbacks ////////////////////////////////////////////////////
        //

        protected virtual void Awake()
        {
            if(target == null)
                target = GetComponent<Light>();

            baseIntensity = target.intensity;
            startTime = Time.time;
        }

        //
        // --------------------------------------------------------------------
        //

        protected virtual void OnEnable()
        {
            if(resetOnEnable)
            {
                startTime = Time.time;
            }
        }
        
        //
        // --------------------------------------------------------------------
        //
        
        protected virtual void Update()
        {
            float elapsed = Time.time - startTime;
            float duration = curve.GetDuration();
            switch(wrapMode)
            {
                case WrapMode.ClampForever: break;
                case WrapMode.Default: break;
                case WrapMode.Once: break;
                case WrapMode.Loop: elapsed = elapsed % duration; break;
                case WrapMode.PingPong: elapsed = Mathf.PingPong(elapsed, duration); break;
            }

            target.intensity = baseIntensity * curve.Evaluate(elapsed);
        }
    }
}
