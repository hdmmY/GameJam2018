using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabber : MonoBehaviour
{



	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.P
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	private void OnTriggerEnter (Collider other)
	{
		if (other.transform.root != transform.root)
		{
		}
	}

	/// <summary>
	/// OnTriggerStay is called once per frame for every Collider other
	/// that is touching the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	private void OnTriggerStay (Collider other)
	{
		if (other.transform.root != transform.root)
		{
		}
	}
}