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
	private CharacterController controller;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	}

	// Update is called once per frame
	void Update () {
		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

		if ( input != Vector3.zero ) {
			targetRotation = Quaternion.LookRotation(input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
		}

		Vector3 motion = input;
		motion *= (Mathf.Abs(motion.x) == 1 && Mathf.Abs(motion.z) == 1)? .7f: 1;
		motion *= (Input.GetButton("Run"))? runSpeed: walkSpeed;
		motion += Vector3.up * -8;

		controller.Move (motion * Time.deltaTime);
	}
}
