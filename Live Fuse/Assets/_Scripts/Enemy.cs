using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject Enemies; // reference to the platform to move

	public GameObject[] myWaypoints; // array of all the waypoints

    //LayerMask to determine what is considered a collision object
    public LayerMask whatCanCollide;

    public string enemyLayer = "EnemyLayer";
    public string playerLayer = "Player";  // name of the player layer to ignore collisions with when stunned   

    //for checking if player is grounded
    public Transform playerCheck;

	[Range(0.0f, 10.0f)] // create a slider in the editor and set limits on moveSpeed
	public float moveSpeed = 5f; // enemy move speed
	public float waitAtWaypointTime = 1f; // how long to wait at a waypoint before _moving to next waypoint
    
    public int damageAmount = 10; // probably deal a lot of damage to kill player immediately

	public bool loopWaypoints = true; // should it loop through the waypoints

	// private variables
	Transform _transform;
    Rigidbody2D _rigidbody;
    Animator _animator;
	int _myWaypointIndex = 0;		// used as index for My_Waypoints
	float _moveTime;
    float _vx = 0f;
	bool _moving = true;
    bool canCollide = false;

    void Awake() {
		// get a reference to the components we are going to be changing and store a reference for efficiency purposes
		_transform = GetComponent<Transform> ();
		
		_rigidbody = GetComponent<Rigidbody2D> ();
		if (_rigidbody==null) // if Rigidbody is missing
			Debug.LogError("Rigidbody2D component missing from this gameobject");

		_animator = GetComponent<Animator>();
		if (_animator==null) // if Animator is missing
			Debug.LogError("Animator component missing from this gameobject");
    }

	// Use this for initialization
	void Start () {
		_transform = Enemies.transform;
		_moveTime = 0f;
		_moving = true;
	}
	
	// game loop
	void Update () {
		// if beyond _moveTime, then start moving
		if (Time.time >= _moveTime) {
			Movement();
		}
        canCollide = Physics2D.Linecast(_transform.position, playerCheck.position, whatCanCollide);  

	}

	void Movement() {
		// if there isn't anything in My_Waypoints
		if ((myWaypoints.Length != 0) && (_moving)) {
			
			// make sure the enemy is facing the waypoint (based on previous movement)
			Flip (_vx);
			
			// determine distance between waypoint and enemy
			_vx = myWaypoints[_myWaypointIndex].transform.position.x-_transform.position.x;
			
			// if the enemy is close enough to waypoint, make it's new target the next waypoint
			if (Mathf.Abs(_vx) <= 0.05f) {
				// At waypoint so stop moving
				_rigidbody.velocity = new Vector2(0, 0);
				
				// increment to next index in array
				_myWaypointIndex++;
				
				// reset waypoint back to 0 for looping
				if(_myWaypointIndex >= myWaypoints.Length) {
					if (loopWaypoints)
						_myWaypointIndex = 0;
					else
						_moving = false;
				}
				
				// setup wait time at current waypoint
				_moveTime = Time.time + waitAtWaypointTime;
			} else {
				
				// Set the enemy's velocity to moveSpeed in the x direction.
				_rigidbody.velocity = new Vector2(_transform.localScale.x * moveSpeed, _rigidbody.velocity.y);
			}
			
		}
	}

    // flip the enemy to face torward the direction he is moving in
	void Flip(float _vx) {
		
		// get the current scale
		Vector3 localScale = _transform.localScale;
		
		if ((_vx>0f)&&(localScale.x<0f))
			localScale.x*=-1;
		else if ((_vx<0f)&&(localScale.x>0f))
			localScale.x*=-1;
		
		// update the scale
		_transform.localScale = localScale;
	}

	// if the player touches the enemy, then kill the player
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Player" ))
		{
            Controller2D player = other.gameObject.GetComponent<Controller2D>();
			if (player.playerCanMove) {
				// Make sure the enemy is facing the player on attack
				Flip(other.transform.position.x-_transform.position.x);
				
				// stop moving
				_rigidbody.velocity = new Vector2(0, 0);
				
				// apply damage to the player
				player.ApplyDamage (damageAmount);
			}
		}
    }
}
