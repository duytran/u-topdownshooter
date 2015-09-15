using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	// Handling
	public float rotationSpeed = 450f;
	public float walkSpeed = 5f;
	public float runSpeed = 8f;

	// System
	private Quaternion targetRotation;

	// Components
	public Gun gun;
	private CharacterController controller;
	private Camera cam;


	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
		cam = Camera.main;
	}

	// Update is called once per frame
	void Update () {
		ControlMouse ();
		//ControlWSAD ();

		if ( Input.GetButtonDown ("Shoot") ) {
			gun.Shoot ();
		} else if ( Input.GetButton ("Shoot") ) {
			gun.ShootContinuous ();
		}
	}

	void ControlMouse () {
		// Get mouse position
		Vector3 mousePos = Input.mousePosition;
		// Using the main camera to convert screen to world point
		// Screen
		// ----------w,h
		// |          |
		// |          |
		// 0,0(x, y)--
		//
		//      [] Camera Z
		//      |
		//      |
		//      Object Z
		mousePos = cam.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));

		// Get target rotation with X and Z axis of mouse with world point
		targetRotation = Quaternion.LookRotation (mousePos - new Vector3(transform.position.x, 0, transform.position.z));
		// Rotate object
		transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

		// Get input to move left and right
		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

		// Calculator the motion
		Vector3 motion = input;
		// If hold horizontal and vertical the motion speed should slower a little ( 0.7, 1)
		motion *= (Mathf.Abs(motion.x) == 1 && Mathf.Abs(motion.z) == 1)? .7f: 1;
		// If hold button run the object will motion faster
		motion *= (Input.GetButton("Run"))? runSpeed: walkSpeed;
		// up to 8
		motion += Vector3.up * -8;
		// Move object
		controller.Move (motion * Time.deltaTime);
	}

	void ControlWSAD () {
		// Get input to move left and right
		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

		// Checking rotation of object
		if ( input != Vector3.zero ) {
			// Target rotation dependence with input
			targetRotation = Quaternion.LookRotation(input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
		}

		// Calculator the motion
		Vector3 motion = input;
		// If hold horizontal and vertical the motion speed should slower a little ( 0.7, 1)
		motion *= (Mathf.Abs(motion.x) == 1 && Mathf.Abs(motion.z) == 1)? .7f: 1;
		// If hold button run the object will motion faster
		motion *= (Input.GetButton("Run"))? runSpeed: walkSpeed;
		// up to 8
		motion += Vector3.up * -8;
		// Move object
		controller.Move (motion * Time.deltaTime);
	}
}
