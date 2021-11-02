using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	public bool destroyNonPlayerObjects = true;

	// Handle gameobjects collider with a deathzone object
	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Player")
		{
			// if player then tell the player to do its FallDeath
			other.gameObject.GetComponent<Controller2D>().FallDeath ();
		} else if (destroyNonPlayerObjects) { // not player so just kill object - could be falling enemy for example
			Destroy(other.gameObject);
		}
	}
}
