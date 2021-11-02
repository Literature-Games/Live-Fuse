using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour
{
	public int value = 1;
	public bool taken = false;
	public GameObject explosion;


	// if the player touches the fuse, it has not already been taken, and the player can move (not dead or victory)
	// then take the fuse
	void OnTriggerEnter2D (Collider2D other)
	{
		if ((other.tag == "Player" ) && (!taken) && (other.gameObject.GetComponent<Controller2D>().playerCanMove))
		{
			// mark as taken so doesn't get taken multiple times
			taken=true;

			// do the player collect fuse
			other.gameObject.GetComponent<Controller2D>().CollectFuse(value);

			// destroy the fuse
			DestroyObject(this.gameObject);
		}
	}
}
