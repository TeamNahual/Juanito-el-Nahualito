using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class package : MonoBehaviour {

	public float pushSpeed;
	public float checkingDistance = 1.5f;
	public float xExtent = 1f, yExtent = 0.25f, zExtent = 1f;

	private bool pushing = false;
	private float horizontal = 0, vertical = 0;
	private bool collidingJuanito = false;
	private Transform camera;
	private Vector3 camera_forward;
	private Vector3 offset;
	private RaycastHit hit;
	private BoxCollider localCollider;
	private Vector3 extents;
	private Rigidbody rb;
	private RigidbodyConstraints moving, still;
	private Juanito juanito;

	void Awake(){
		camera = Camera.main.transform;
		localCollider = GetComponent <BoxCollider>();
		extents = new Vector3 (xExtent, yExtent, zExtent);
		rb = GetComponent <Rigidbody> ();
		moving = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		still = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		rb.constraints = still;
	}

	void Start(){
		juanito = Juanito.ins;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E) && collidingJuanito) {
			Debug.Log ("pushing");
			pushing = true;

			juanito.transform.parent = this.transform;
			juanito.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
			juanito.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;

			rb.constraints = moving;

			juanito.HumanAnim.SetBool ("Pushing", true);
		// this might cause problems when the package is in scene but not being interacted with
		} else if (Input.GetKeyUp (KeyCode.E)) {
			//Debug.Log ("stopped pushing");
			pushing = false;

			juanito.transform.parent = null;
			juanito.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
			juanito.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;

			rb.constraints = still;

			juanito.HumanAnim.SetBool ("Pushing", false);
		}

		if (pushing) {
			
			horizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
			vertical = CrossPlatformInputManager.GetAxis ("Vertical");
			bool keyboard = !(Mathf.Abs (horizontal) < 0.1f && Mathf.Abs (vertical) < 0.1f);

			if (!keyboard) {
				horizontal = CrossPlatformInputManager.GetAxis ("Horizontal-Joystick");
				vertical = CrossPlatformInputManager.GetAxis ("Vertical-Joystick");
			}

			camera_forward = Vector3.Scale (camera.forward, new Vector3 (1, 0, 1)).normalized;
			Vector3 move = vertical * camera_forward + horizontal * camera.right;

			bool hitcheck = Physics.BoxCast(localCollider.bounds.center, extents, move, out hit, transform.rotation, checkingDistance, 13);
			if (hitcheck && !hit.collider.isTrigger) {
				Debug.Log ("Hit : " + hit.transform.gameObject + " with half extents " + extents);
				move = Vector3.zero;
			}
				
			Vector3 diff = juanito.JuanitoHuman.transform.position - transform.position;
			diff = new Vector3 (diff.x, 0f, diff.z);
			float animForward = vertical;
			float angleDiff = Vector3.Angle (camera_forward, diff);
			if (angleDiff < 90f) {
				animForward *= -1;
			}
			Juanito.ins.HumanAnim.SetFloat ("Forward", animForward);

			transform.Translate (move * pushSpeed * Time.deltaTime);
		}

	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject == juanito.JuanitoHuman) {
			collidingJuanito = true;
		}
	}

	void OnCollisionExit(Collision other){
		if (other.gameObject == juanito.JuanitoHuman) {
			collidingJuanito = false;
		}
	}
}