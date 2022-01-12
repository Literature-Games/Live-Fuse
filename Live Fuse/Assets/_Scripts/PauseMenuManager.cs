using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/*
* This file is the manager for the pause menu
*/

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager pm;

    [Header("Menu Panels")]
    [SerializeField] GameObject pauseMenu;

    // First buttons selected in the menus attached to the main menu
    [Header("Selection Buttons")]
    [SerializeField] GameObject pauseFirstSelected;

    void Awake()
    {
        // setup reference to game manager
		if (pm == null)
			pm = this.GetComponent<PauseMenuManager>();
    }

    public void OpenPause()
    {
        pauseMenu.SetActive(true);
        // cleared first, then wait
        //for at least one frame befroe setting current object
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
    }

    public void ClosePause()
    {
        pauseMenu.SetActive(false);
        // cleared first
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OpenMainMenu()
    {
        if(Time.timeScale == 0f)
        Time.timeScale = 1f; // this unpauses the game action (ie. back to normal)
        SceneTransition.st.LoadLevel("Menu");
    }
}
