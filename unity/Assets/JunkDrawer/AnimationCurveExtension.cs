//=============================================================================
//
// (C) BLACKTRIANGLES 2015
// http://www.blacktriangles.com
// contact@blacktriangles.com
//
// Howard N Smith | hsmith | howard@blacktriangles.com
//
//=============================================================================

using UnityEngine;
using blacktriangles;

public static class AnimationCurveExtension
{
    public static float GetDuration( this AnimationCurve self )
    {
        int len = self.length;
        return self[len-1].time;
    }
}
