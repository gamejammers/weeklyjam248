//
// (c) BLACKTRIANGLES 2015
// http://www.blacktriangles.com
//

using UnityEngine;
using System.Collections.Generic;

namespace blacktriangles
{
    public static class ArrayExtension
    {
        public static bool IsNullOrEmpty( this System.Array self)
        {
            return self == null || self.Length == 0;
        }

        //
        // --------------------------------------------------------------------
        //

        public static bool Contains( this System.Array self, object item )
        {
            return self.IndexOf( item ) >= 0;
        }

        //
        // --------------------------------------------------------------------
        //

        public static bool IsValidIndex( this System.Array self, int index )
        {
            return ( index >= 0 && index < self.Length );
        }

        //
        // --------------------------------------------------------------------
        //

        public static bool IsValidIndex( this System.Array self, int x, int y)
        {
            return ( x >= 0 && x < self.GetLength(0) &&
                     y >= 0 && y < self.GetLength(1) );
        }

        //
        // --------------------------------------------------------------------
        //

        public static bool IsValidIndex( this System.Array self, int x, int y, int z)
        {
            return ( x >= 0 && x < self.GetLength(0) &&
                     y >= 0 && y < self.GetLength(1) &&
                     z >= 0 && z < self.GetLength(2) );
        }

        //
        // --------------------------------------------------------------------
        //

        #if UNITY_STANDALONE || UNITY_EDITOR
                
        public static bool IsValidIndex( this System.Array self, UnityEngine.Vector2Int index)
        {
            return self.IsValidIndex(index.x, index.y);
        }

        //
        // --------------------------------------------------------------------
        //

        public static bool IsValidIndex( this System.Array self, UnityEngine.Vector3Int index)
        {
            return self.IsValidIndex(index.x, index.y, index.z);
        }
        
        #endif

        //
        // --------------------------------------------------------------------
        //

        public static int IndexOf( this System.Array self, object item )
        {
            return System.Array.IndexOf( self, item );
        }

        //
        // --------------------------------------------------------------------
        //

        public static int IndexOfFirstNull( this System.Array self )
        {
            for( int i = 0; i < self.Length; ++i )
            {
                if( self.GetValue(i) == null ) return i;
            }

            return -1;
        }

        //
        // --------------------------------------------------------------------
        //

        public static void Shuffle( this System.Array self )
        {
            System.Random rnd = new System.Random();
            Shuffle( self, rnd );
        }
        
        //
        // --------------------------------------------------------------------
        //

        public static void Shuffle( this System.Array self, System.Random rnd )
        {
            for (int i = self.Length; i > 1; i--)
            {
                // Pick random element to swap.
                int j = rnd.Next(0,i); // 0 <= j <= i-1
                // Swap.
                object tmp = self.GetValue(j);
                self.SetValue( self.GetValue( i - 1 ), j );
                self.SetValue( tmp, i - 1 );
            }
        }

        //
        // --------------------------------------------------------------------
        //

        public static int RandomIndex<T>( this T[] self )
        {
            return btRandom.Range(0, self.Length-1);
        }
        

        //
        // --------------------------------------------------------------------
        //

        public static T Random<T>( this T[] self )
        {
            return self[ RandomIndex(self) ];
        }

        //
        // --------------------------------------------------------------------
        //

        public static T Random<T>( this T[] self, System.Random rng )
        {
            if( self.Length <= 0 ) return default(T);
            return self[ rng.Next( self.Length ) ];
        }

        //
        // --------------------------------------------------------------------
        //

        public static void DebugDump( this byte[] self )
        {
            System.Array.ForEach( self, (b)=>{ Debug.Log( b.ToString() ); } );
        }

        //
        // --------------------------------------------------------------------
        //

        public static string ToDebugString<T>( this T[] self )
        {
          System.String res = "Array["+self.Length.ToString()+"]";
          System.Array.ForEach( self, (b)=>{ res += b.ToString() + "\n"; } );
          return res;
        }

        //
        // --------------------------------------------------------------------
        //

        public static T[] Slice<T>(this T[] source, int start, int end = -1)
        {
            // Handles negative ends.
            if (end < 0)
            {
              end = source.Length;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
              res[i] = source[i + start];
            }
            return res;
        }

        //
        // --------------------------------------------------------------------
        //

        public static T[] ConvertAll<T,U>( this U[] self )
          where T: U
        {
            return System.Array.ConvertAll<U,T>( self, (i)=>{ return (T)i; } );
        }

        //
        // --------------------------------------------------------------------
        //

        public static string[] ConvertAll<U>( this U[] self)
            where U: class
        {
            return System.Array.ConvertAll<U, string>(self, (i)=>{ return i.ToString(); });
        }

        //
        // --------------------------------------------------------------------
        //

        public static List<U> TryConvertAll<T, U>( this T[] self)
            where T: class
            where U: T
        {
            List<U> result = new List<U>();
            foreach(T orig in self)
            {
                if(orig is U cast)
                {
                    result.Add(cast);
                }
            }

            return result;
        }
        

        //
        // --------------------------------------------------------------------
        //

        public static T[] ShallowClone<T>( this T[] self )
        {
            T[] result = new T[ self.Length ];
            System.Array.Copy( self, result, self.Length );
            return result;
        }

        //
        // --------------------------------------------------------------------
        //
        

        public static List<T> Map<T,U>( this U[] self, System.Func<U, T> mapper )
        {
            List<T> result = new List<T>();
            foreach(U item in self)
            {
                result.Add(mapper(item));
            }
            return result;
        }

        //
        // ------------------------------------------------------------------------
        //
        
        public static List<T> Filter<T>( this T[] self, System.Func<T,bool> filter)
        {
            List<T> result = new List<T>();
            foreach(T t in self)
            {
                if(filter(t))
                    result.Add(t);
            }

            return result;
        }

        //
        // --------------------------------------------------------------------
        //
        
        public static void Fill<T>( this T[] self, T val )
        {
            for(int i = 0; i < self.Length; ++i)
            {
                self[i] = val;
            }
        }

        //
        // --------------------------------------------------------------------
        //

        public static void ForEach2d<T>( this T[,] self, System.Action<int,int,T> cb)
        {
            for(int y = 0; y < self.GetLength(1); ++y)
            {
                for(int x = 0; x < self.GetLength(0); ++x)
                {
                    cb(x,y,self[x,y]);
                }
            }
        }

        //
        // --------------------------------------------------------------------
        //

        public static void ForEach2d<T>( this T[,] self, System.Func<int,int,T, T> cb)
        {
            for(int y = 0; y < self.GetLength(1); ++y)
            {
                for(int x = 0; x < self.GetLength(0); ++x)
                {
                    self[x,y] = cb(x,y,self[x,y]);
                }
            }
        }
        
        //
        // --------------------------------------------------------------------
        //

        public static void ForEach3d<T>( this T[,,] self, System.Func<int,int,int,T,T> cb)
        {
            for(int z = 0; z < self.GetLength(2); ++z)
            {
                for(int y = 0; y < self.GetLength(1); ++y)
                {
                    for(int x = 0; x < self.GetLength(0); ++x)
                    {
                        self[x,y,z] = cb(x,y,z,self[x,y,z]);
                    }
                }
            }
        }

        //
        // --------------------------------------------------------------------
        //

        public static UnityEngine.Vector2Int GetLengths<T>( this T[,] self )
        {
            return new UnityEngine.Vector2Int(self.GetLength(0), self.GetLength(1));
        }

        //
        // --------------------------------------------------------------------
        //

        public static UnityEngine.Vector3Int GetLengths<T>( this T[,,] self )
        {
            return new UnityEngine.Vector3Int(self.GetLength(0), self.GetLength(1), self.GetLength(2));
        }

        //
        // --------------------------------------------------------------------
        //

        #if UNITY_STANDALONE || UNITY_EDITOR
        public static void ForEach3d<T>( this T[,,] self, System.Func<UnityEngine.Vector3Int,T,T> cb)
        {
            for(int z = 0; z < self.GetLength(2); ++z)
            {
                for(int y = 0; y < self.GetLength(1); ++y)
                {
                    for(int x = 0; x < self.GetLength(0); ++x)
                    {
                        self[x,y,z] = cb(new UnityEngine.Vector3Int(x,y,z),self[x,y,z]);
                    }
                }
            }
        }       

        //
        // --------------------------------------------------------------------
        //

        public static IEnumerable<UnityEngine.Vector3Int> EachIndex3d<T>( this T[,,] self, int start = 0, int step = 1)
        {
            for(int x = 0; x < self.GetLength(0); x+=step)
            {
                for(int y = 0; y < self.GetLength(1); y+=step)
                {
                    for( int z = 0; z < self.GetLength(2); z+=step)
                    {
                        yield return new UnityEngine.Vector3Int(x,y,z);
                    }
                }
            }
        }
        #endif
    }
}
