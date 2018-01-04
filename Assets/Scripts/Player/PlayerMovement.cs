using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;

	Rigidbody playerRB;
	Vector3 movement;
	Animator anim;
	int floorMask;
	float camRayLength = 100f;

	void Awake(){

		floorMask = LayerMask.GetMask ("Floor");

		playerRB = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();

	}

	void FixedUpdate(){
		
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

		movePlayer (horizontal, vertical);
		playWalkingAnimation (horizontal, vertical);
		turnDirection ();

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
}
