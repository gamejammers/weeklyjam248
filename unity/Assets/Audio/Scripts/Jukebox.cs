//
// (c) GameJammers 2022
// http://www.jamming.games
//

using UnityEngine;

public class Jukebox
	: MonoBehaviour
{
	//
	// fields /////////////////////////////////////////////////////////////////
	//

	public AudioClip[] tracks									= null;
	public AudioSource player									= null;

	private int currentIdx										= 0;

	//
	// private methods ////////////////////////////////////////////////////////
	//

	public static void Shuffle( System.Array self, System.Random rnd )
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
	// unity callbacks ////////////////////////////////////////////////////////
	//

	protected virtual void Awake()
	{
		Shuffle(tracks, new System.Random() );
		currentIdx = 0;
	}

	//
	// ------------------------------------------------------------------------
	//
	
	protected virtual void Update()
	{
		if(!player.isPlaying)
		{
			if(currentIdx < 0 || currentIdx >= tracks.Length)
			{
				currentIdx = 0;
			}

			player.clip = tracks[currentIdx];
			player.Play();
			++currentIdx;
		}
	}
	
	
}
