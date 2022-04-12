//
// (C) BLACKTRIANGLES 2015
// http://www.blacktriangles.com
//

using UnityEngine;
using System;
using System.Security.Cryptography;

namespace blacktriangles
{
    public static class btRandom
    {
        //
        // members ////////////////////////////////////////////////////////////
        //
        
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        //
        // public methods /////////////////////////////////////////////////////
        //

        public static bool Chance(float chance)
        {
            return Range(0.0f, 1.0f) < chance;
        }

        //
        // --------------------------------------------------------------------
        //

        public static byte RandomByte()
        {
            byte[] randomNumber = new byte[1];
            _generator.GetBytes(randomNumber);
            return randomNumber[0];
        }

        //
        // --------------------------------------------------------------------
        //

        public static int RandomInt()
        {
            return Range(System.Int32.MinValue, System.Int32.MaxValue);
        }

        //
        // --------------------------------------------------------------------
        //

        public static double Range( double minimumValue, double maximumValue )
        {
            double asciiValueOfRandomCharacter = (double)RandomByte();
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d));
            double range = maximumValue - minimumValue;

            return multiplier * range + minimumValue;
        }

        //
        // --------------------------------------------------------------------
        //

        public static float Range( float minimumValue, float maximumValue )
        {
            double asciiValueOfRandomCharacter = (double)RandomByte();
            float multiplier = (float)Math.Max(0, (asciiValueOfRandomCharacter / 255d));
            float range = maximumValue - minimumValue;

            return multiplier * range + minimumValue;
        }

        //
        // --------------------------------------------------------------------
        //

        public static int Range(int minimumValue, int maximumValue)
        {
            double asciiValueOfRandomCharacter = (double)RandomByte();

            // We are using Math.Max, and substracting 0.00000000001,
            // to ensure "multiplier" will always be between 0.0 and .99999999999
            // Otherwise, it's possible for it to be "1", which causes problems in our rounding.
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            // We need to add one to the range, to allow for the rounding done with Math.Floor
            System.Int64 range = (System.Int64)maximumValue - (System.Int64)minimumValue + 1;
            double randomValueInRange = Math.Floor(multiplier * range);

            return (int)(minimumValue + randomValueInRange);
        }

        //
        // --------------------------------------------------------------------
        //

        public static float UnitFloat()
        {
            return Range( -1.0f, 1.0f );
        }

        //
        // --------------------------------------------------------------------
        //

        public static double UnitDouble()
        {
            return Range( -1.0, 1.0 );
        }

        //
        // --------------------------------------------------------------------
        //

        public static float Gaussian( float mean, float variance )
        {
            float u1 = UnitFloat();
            float u2 = UnitFloat();
            float randNormal = Mathf.Sqrt( -2.0f * Mathf.Log(u1) ) * Mathf.Sin( 2.0f * Mathf.PI * u2 );
            return mean + ( variance * randNormal );
        }

        //
        // --------------------------------------------------------------------
        //

        public static double Gaussian( double mean, double variance )
        {
            double u1 = UnitDouble();
            double u2 = UnitDouble();
            double randNormal = System.Math.Sqrt( -2.0f * System.Math.Log(u1) ) * System.Math.Sin( 2.0f * System.Math.PI * u2 );
            return mean + ( variance * randNormal );
        }

        //
        // --------------------------------------------------------------------
        //

        // http://stackoverflow.com/questions/5837572/generate-a-random-point-within-a-circle-uniformly
        public static UnityEngine.Vector2 RandomPointInCircle(
                    UnityEngine.Vector2 center = default(UnityEngine.Vector2),
                    float radius = 1.0f
                )
        {
            float t = 2 * Mathf.PI * UnitFloat();
            float u = UnitFloat() + UnitFloat();
            float r = ( u > 1 ) ? 2 - u : u;
            return new UnityEngine.Vector2( r * radius * Mathf.Cos( t ), r * radius * Mathf.Sin( t ) ) + center;
        }

        //
        // --------------------------------------------------------------------
        //

        public static UnityEngine.Vector3 RandomPointInCylinder(
                    UnityEngine.Vector3 center = default(UnityEngine.Vector3),
                    float radius = 1.0f,
                    float height = 1.0f
                )
        {
            UnityEngine.Vector2 circlePos = btRandom.RandomPointInCircle( new UnityEngine.Vector2( center.x, center.z ), radius );
            return new UnityEngine.Vector3( circlePos.x, btRandom.UnitFloat() * height, circlePos.y );
        }

        //
        // --------------------------------------------------------------------
        //
    }
}
