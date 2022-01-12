using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent interactAction;
    public bool isInRange = false;
    [HideInInspector] public string interacted = "";

    //private variables below
    bool hasInteracted;

	public virtual void Interact()
	{
		Debug.Log("Interacting with base class.");
	}


    void Update()
	{
		if(isInRange && !hasInteracted)
		{
            // let the item know the player has interacted
			if(InputManager.im.GetInteractPressed())
			{
				Debug.Log("Play has interacted with " + interacted);
				hasInteracted = true;
				interactAction.Invoke(); //Start Event
            }
        }
    }

    // if the player touches the item, it has not already been taken, and the player can move (not dead or victory)
	// then interact with the item
	void OnTriggerEnter2D(Collider2D other)
	{
		if ((other.tag == "Player" ) && (other.gameObject.GetComponent<Controller2D>().playerCanMove))
        {
			isInRange = true;
			Debug.Log("Player now in range");
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if ((other.tag == "Player" ) && (other.gameObject.GetComponent<Controller2D>().playerCanMove))
        {
			isInRange = false;
			hasInteracted = false;
			Debug.Log("Player now out range of " + interacted);
			interacted = "";
			Debug.Log("Interacted is now " + interacted);
		}
	}
}
