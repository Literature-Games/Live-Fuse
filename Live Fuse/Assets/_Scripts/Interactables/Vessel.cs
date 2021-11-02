using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vessel : Interactable
{
    public int neededFuses = 0; //set the amount of fuses in the level
    public Controller2D _controller;

    //private variables below
    int fusesLeft = 0;  //set the amount of fuses left in the level for the player to collect

    public override void Interact()
	{
		Debug.Log("Interacting with vessel.");
        Collect();
	}

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.gm)
            GameManager.gm.fusesText.text = neededFuses.ToString();

        //set the max at the start of the level
        fusesLeft = neededFuses;
    }

    void Collect()
    {
        if(GameManager.gm)
        {
            fusesLeft = neededFuses - GameManager.gm.score;
            GameManager.gm.fusesText.text = fusesLeft.ToString();
        }

        //there are no fuses left in the level, player won
        if(fusesLeft == 0)
            _controller.Victory();
    }
}
