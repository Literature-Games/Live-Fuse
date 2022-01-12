using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller2D : MonoBehaviour
{
    [Header("Player Controls")]
    public float moveSpeed = 5f;
    public float jumpForce = 600f;
    public int playerHealth = 1;
	
    //for checking if player is grounded
    public Transform groundCheck;
    public bool playerCanMove = true;

    //LayerMask to determine what is considered ground for player
	[Header("LayerMask")]
    public LayerMask whatIsGround;

    [Header("SFX")]
    public AudioClip fuseSFX;
	public AudioClip deathSFX;
	public AudioClip fallSFX;
	public AudioClip jumpSFX;
	public AudioClip victorySFX;

    //private variables below
    Transform _transform;
    Rigidbody2D _rigidbody;
    Animator _animator;
    AudioSource _audio;

    // hold player motion in this timestep
	Vector2 moveDirection;

	// player tracking
	bool facingRight = true;
	bool isGrounded = false;
	bool isRunning = false;

	// store the layer the player is on (setup in Awake)
	int _playerLayer;

	// number of layer that Platforms are on (setup in Awake)
	int _platformLayer;

    void Awake()
    {
        // get a reference to the components we are going to be changing and store a reference for efficiency purposes
		_transform = GetComponent<Transform> ();
		
		_rigidbody = GetComponent<Rigidbody2D> ();
		if (_rigidbody==null) // if Rigidbody is missing
			Debug.LogError("Rigidbody2D component missing from this gameobject");
		
		_animator = GetComponent<Animator>();
		if (_animator==null) // if Animator is missing
			Debug.LogError("Animator component missing from this gameobject");
		
		_audio = GetComponent<AudioSource> ();
		if (_audio==null) { // if AudioSource is missing
			Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
			// let's just add the AudioSource component dynamically
			_audio = gameObject.AddComponent<AudioSource>();
		}

		// determine the player's specified layer
		_playerLayer = this.gameObject.layer;

		// determine the platform's specified layer
		_platformLayer = LayerMask.NameToLayer("Platform");
    }

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
       // exit update if player cannot move or game is paused
		if (!playerCanMove || (Time.timeScale == 0f))
			return;

		// Determine if running based on the horizontal movement
		moveDirection = InputManager.im.GetMoveDirection();

		// set the running animation state
		_animator.SetBool("Running", isRunning);

		// Determine if running based on the horizontal movement
		if (InputManager.im.moving)
		{
			isRunning = true;
		}
		else isRunning = false;

		// get the current vertical velocity from the rigidbody component
		moveDirection.y = _rigidbody.velocity.y;

		// Check to see if character is grounded by raycasting from the middle of the player
		// down to the groundCheck position and see if collected with gameobjects on the
		// whatIsGround layer
		isGrounded = Physics2D.Linecast(_transform.position, groundCheck.position, whatIsGround);  

		// Set the grounded animation states
		_animator.SetBool("Grounded", isGrounded);

		if(isGrounded && InputManager.im.GetJumpPressed()) // If grounded AND jump button pressed, then allow the player to jump
		{
			DoJump();
		}
	
		// If the player stops jumping mid jump and player is not yet falling
		// then set the vertical velocity to 0 (he will start to fall from gravity)
		if(InputManager.im.GetJumpPressed() && moveDirection.y>0f)
		{
			moveDirection.y = 0f;
		}

		// Change the actual velocity on the rigidbody
		_rigidbody.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y);

		// if moving up then don't collide with platform layer
		// this allows the player to jump up through things on the platform layer
		// NOTE: requires the platforms to be on a layer named "Platform"
		Physics2D.IgnoreLayerCollision(_playerLayer, _platformLayer, (moveDirection.y > 0.0f));

    }

    void LateUpdate()
    {
        // get the current scale
		Vector3 localScale = _transform.localScale;

		if (moveDirection.x > 0) // moving right so face right
		{
			facingRight = true;
		} else if (moveDirection.x < 0) { // moving left so face left
			facingRight = false;
		}

		// check to see if scale x is right for the player
		// if not, multiple by -1 which is an easy way to flip a sprite
		if (((facingRight) && (localScale.x<0)) || ((!facingRight) && (localScale.x>0))) {
			localScale.x *= -1;
		}

		// update the scale
		_transform.localScale = localScale;
    }

    public void DoJump()
    {
		// reset current vertical motion to 0 prior to jump
		moveDirection.y = 0f;
		// add a force in the up direction
		_rigidbody.AddForce (new Vector2 (0, jumpForce));
		// play the jump sound
		PlaySound(jumpSFX);
    }

    	// if the player collides with a MovingPlatform, then make it a child of that platform
	// so it will go for a ride on the MovingPlatform
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag=="MovingPlatform")
		{
			this.transform.parent = other.transform;
		}
	}

	// if the player exits a collision with a moving platform, then unchild it
	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.tag=="MovingPlatform")
		{
			this.transform.parent = null;
		}
	}

    // do what needs to be done to freeze the player
 	void FreezeMotion() {
		playerCanMove = false;
        _rigidbody.velocity = new Vector2(0,0);
		_rigidbody.isKinematic = true;
	}

	// do what needs to be done to unfreeze the player
	void UnFreezeMotion() {
		playerCanMove = true;
		_rigidbody.isKinematic = false;
	}

	// play sound through the audiosource on the gameobject
	void PlaySound(AudioClip clip)
	{
		_audio.PlayOneShot(clip);
	}

	// public function to apply damage to the player
	public void ApplyDamage (int damage) {
		if (playerCanMove) {
			playerHealth -= damage;

			if (playerHealth <= 0) { // player is now dead, so start dying
				PlaySound(deathSFX);
				StartCoroutine (KillPlayer ());
			}
		}
	}

	// public function to kill the player when they have a fall death
	public void FallDeath () {
		if (playerCanMove) {
			playerHealth = 0;
			PlaySound(fallSFX);
			StartCoroutine (KillPlayer ());
		}
	}

	// coroutine to kill the player
	IEnumerator KillPlayer()
	{
		if (playerCanMove)
		{
			// freeze the player
			FreezeMotion();

			// play the death animation
			//_animator.SetTrigger("Death");
			
			// After waiting tell the GameManager to reset the game
			yield return new WaitForSeconds(2.0f);

			if (GameManager.gm) // if the gameManager is available, tell it to reset the game
				GameManager.gm.ResetGame();
			else // otherwise, just reload the current level
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	public void CollectFuse(int amount) {
		PlaySound(fuseSFX);

		if (GameManager.gm) // add the points through the game manager, if it is available
			GameManager.gm.AddPoints(amount);
	}

	// public function on victory over the level
	public void Victory() {
		PlaySound(victorySFX);
		FreezeMotion ();
		//_animator.SetTrigger("Victory");

		if (GameManager.gm) // do the game manager level compete stuff, if it is available
			GameManager.gm.LevelCompete();
	}

	// public function to respawn the player at the appropriate location
	public void Respawn(Vector3 spawnloc) {
		UnFreezeMotion();
		playerHealth = 1;
		_transform.parent = null;
		_transform.position = spawnloc;
		//_animator.SetTrigger("Respawn");
	}
}
