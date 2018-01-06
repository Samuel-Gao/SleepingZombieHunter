using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;
	public int atkUp = 20;
	public float spdUp = 4f;
	public float spawnTime = 2f;
	public GameObject powerup;

	Rigidbody playerRB;
	Vector3 movement;
	Animator anim;
	int floorMask;
	float camRayLength = 100f;

	float bufCounter = 0f;

	void Awake(){

		floorMask = LayerMask.GetMask ("Floor");

		playerRB = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	void FixedUpdate(){
		
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

		movePlayer (horizontal, vertical);
		playWalkingAnimation (horizontal, vertical);
		turnDirection ();
	
		bufCounter += Time.deltaTime;
		if (PlayerShooting.damagePerShot > 20 && bufCounter >= 1) {

			PlayerShooting.damagePerShot -= 1;
			bufCounter = 0f;

		} else if (speed > 6f) {
			speed -= 0.4f * Time.deltaTime;
		}
	}
		
	void movePlayer(float horizontal, float vertical){
		movement.Set (horizontal, 0f, vertical);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRB.MovePosition (transform.position + movement);
	}

	void playWalkingAnimation(float horizontal, float vertical){
		bool isMoving = horizontal != 0f || vertical != 0f;
		anim.SetBool ("IsWalking", isMoving);
	}

	void turnDirection(){
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit rayHit;

		if (Physics.Raycast (camRay, out rayHit, camRayLength, floorMask)) {

			Vector3 playerToMouse = rayHit.point - transform.position;	
			playerToMouse.y = 0f;
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			playerRB.MoveRotation (newRotation);
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("AtkPowerUp")) {
			PlayerShooting.damagePerShot += atkUp;
			Destroy (other.gameObject);
		} else if (other.gameObject.CompareTag ("SpdPowerUp")) {
			speed += spdUp;
			Destroy (other.gameObject);
		}


	}

	void Spawn ()
	{
		float spawnX  = transform.position.x + 10;
		float spawnZ = transform.position.z + 10;
		Vector3 spawnPosition = new Vector3 (Random.Range(-spawnX, spawnX), 0.5f, Random.Range(-spawnZ, spawnZ));

		Instantiate (powerup, spawnPosition, powerup.transform.rotation);
	}
}
