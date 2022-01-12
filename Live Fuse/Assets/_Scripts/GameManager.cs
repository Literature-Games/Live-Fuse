using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    // levels to move to on victory and lose
	public string levelAfterVictory;
	public string levelAfterGameOver;
	[HideInInspector] public string currentLevel;
	public Text fusesText;

    [Header("game performance")] 
    public int score = 0;
    public int lives = 3;

    [Header("UI elements to control")]
    public GameObject[] UIExtraLives;

    // private variables
	GameObject _player;
	Vector3 _spawnLocation;
	Scene _scene;

	// set things up here
	void Awake () {
		// setup reference to game manager
		if (gm == null)
			gm = this.GetComponent<GameManager>();

		// setup all the variables, the UI, and provide errors if things not setup properly.
		setupDefaults();
	}

	// game loop
	void Update() {
		// if ESC pressed then pause the game
        
		if (InputManager.im.GetPausePressed()) {
			if (Time.timeScale > 0f) {
				PauseMenuManager.pm.OpenPause(); // this brings up the pause UI
				Time.timeScale = 0f; // this pauses the game action
			} else {
				Time.timeScale = 1f; // this unpauses the game action (ie. back to normal)
				PauseMenuManager.pm.ClosePause(); // this brings up the pause UI
			}
		}
	}

	// setup all the variables, the UI, and provide errors if things not setup properly.
	void setupDefaults() {
		// setup reference to player
		if (_player == null)
			_player = GameObject.FindGameObjectWithTag("Player");
		
		if (_player==null)
			Debug.LogError("Player not found in Game Manager");

		// get current scene
		_scene = SceneManager.GetActiveScene();

		// get initial _spawnLocation based on initial position of player
		_spawnLocation = _player.transform.position;

		// if levels not specified, default to current level
		if (levelAfterVictory=="") {
			Debug.LogWarning("levelAfterVictory not specified, defaulted to current level");
			levelAfterVictory = _scene.name;
		}
		
		if (levelAfterGameOver=="") {
			Debug.LogWarning("levelAfterGameOver not specified, defaulted to current level");
			levelAfterGameOver = _scene.name;
		}

		if(currentLevel=="") {
			currentLevel = _scene.name;
		}
		
		// get stored player prefs
		refreshPlayerState();

		// get the UI ready for the game
		refreshGUI();
	}

	// get stored Player Prefs if they exist, otherwise go with defaults set on gameObject
	void refreshPlayerState() {
		//lives = PlayerPrefManager.GetLives();

		// special case if lives <= 0 then must be testing in editor, so reset the player prefs
		if (lives <= 0) {
			//PlayerPrefManager.ResetPlayerState(startLives,false);
			//lives = PlayerPrefManager.GetLives();
		}

		// save that this level has been accessed so the MainMenu can enable it
		//PlayerPrefManager.UnlockLevel();
	}

	// refresh all the GUI elements
	void refreshGUI() {
		// turn on the appropriate number of life indicators in the UI based on the number of lives left
		for(int i=0;i<UIExtraLives.Length;i++) {
			if (i<(lives-1)) { // show one less than the number of lives since you only typically show lifes after the current life in UI
				UIExtraLives[i].SetActive(true);
			} else {
				UIExtraLives[i].SetActive(false);
			}
		}
	}

	// public function to add points and update the gui and highscore player prefs accordingly
	public void AddPoints(int amount)
	{
		// increase score
		score+=amount;
	}

	// public function to remove player life and reset game accordingly
	public void ResetGame() {
		// remove life and update GUI
		lives--;
		refreshGUI();

		if (lives<=0) { // no more lives
			// load the gameOver screen
			SceneManager.LoadScene(levelAfterGameOver);
		} else { // tell the player to respawn
			_player.GetComponent<Controller2D>().Respawn(_spawnLocation);
		}
	}

	// public function for level complete
	public void LevelCompete() {
		if(LevelLocker.ll)
			LevelLocker.ll.UnlockLevel(currentLevel);
		// use a coroutine to allow the player to get fanfare before moving to next level
		SceneTransition.st.LoadLevel(levelAfterVictory);
	}
}
