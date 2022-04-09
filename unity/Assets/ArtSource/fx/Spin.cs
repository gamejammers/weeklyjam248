//
// (c) GameJammers 2022
// http://www.jamming.games
//

using UnityEngine;

namespace Metal
{

public class Spin
	: MonoBehaviour
{
	//
	// fields /////////////////////////////////////////////////////////////////
	//

	public Vector3 spinSpeed 									= new Vector3(10.0f, 0f, 0f);
	
	//
	// unity callbacks ////////////////////////////////////////////////////////
	//
	
	protected virtual void Update()
	{
		transform.localRotation *= Quaternion.Euler( spinSpeed * Time.deltaTime );
	}
}

}
