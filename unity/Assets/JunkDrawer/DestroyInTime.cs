//=============================================================================
//
// (C) BLACKTRIANGLES 2015
// http://www.blacktriangles.com
//
// Howard N Smith | hsmith | howard@blacktriangles.com
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace blacktriangles
{
    public class DestroyInTime
        : MonoBehaviour
    {
        // members ////////////////////////////////////////////////////////////
        public float destroyInSeconds                            = 1f;

        // unity callbacks ////////////////////////////////////////////////////
        protected virtual void Start()
        {
            Destroy( gameObject, destroyInSeconds );
        }
    }
}
