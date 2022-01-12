using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // include so we can load new scenes

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition st;
    [SerializeField] Animator transition;

    void Awake()
    {
        // setup reference to game manager
		if (st == null)
			st = this.GetComponent<SceneTransition>();
    }

    public void LoadLevel(string level)
    {
        StartCoroutine(LoadNextLevel(level));
    }

    IEnumerator LoadNextLevel(string levelToLoad) {
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(2.5f);
		SceneManager.LoadScene(levelToLoad);
	}
}
