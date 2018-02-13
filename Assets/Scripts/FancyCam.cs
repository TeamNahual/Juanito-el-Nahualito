using UnityEngine;
using System.Collections;

public class FancyCam : MonoBehaviour {

    public Transform camHold;
    public float rotationSpeed = 200f;
    public float verticalSpeed = 180f;
	public Transform player;
    // max camera can drift to either side
    public float xDrift = 2f;
    // sets the upper and lower bounds from vertical rotation
    public float vertRotMax = 30f;
    public float vertRotMin = -70f;

    private float camWidth = 0f;
    private float unsignedAngle;
    private float xRot;

    private float xAxisCon = 0;
    private float yAxisCon = 0;

    public static FancyCam ins;

    void Awake()
    {
        ins = this;
    }

    void Start ()
    {
        // sets the camera pivot target to the player
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
    
	void FixedUpdate ()
    {
        // required var for smooth damping
        float velocity = 0;
        // get a "forward vector" for each rotation
        Vector3 forwardA = camHold.rotation * Vector3.forward;
        Vector3 forwardB = player.rotation * Vector3.forward;
        // get a numeric angle for each vector, on the X-Z plane (relative to world forward)
        float angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;
        // get the signed difference in these angles
        float angleDiff = Mathf.DeltaAngle(angleA, angleB);
        // convert angleDiff to unsigned
        unsignedAngle = Mathf.Abs(angleDiff);
        // container for camera holder x postion to offset
        camWidth = camHold.localPosition.x;
        // checks that the player isn't facing the camera, then smoothly drifts the camera holder away from center
        if (unsignedAngle < 150)
        {
            camWidth = Mathf.SmoothDamp(camHold.localPosition.x, Mathf.Clamp(angleDiff/30, -xDrift, xDrift), ref velocity, 1.4f * Time.fixedDeltaTime);
            camHold.localPosition = Vector3.Lerp(camHold.localPosition, new Vector3(camWidth, camHold.localPosition.y, camHold.localPosition.z) , 3f * Time.fixedDeltaTime);
        }
        // if the player is facing the camera, smoothly drift the camera back to center
        else
        {
            camWidth = Mathf.SmoothDamp(camHold.localPosition.x, 0f, ref velocity, 1.4f * Time.fixedDeltaTime);
            camHold.localPosition = Vector3.Lerp(camHold.localPosition, new Vector3(camWidth, camHold.localPosition.y, camHold.localPosition.z) , 1.4f * Time.fixedDeltaTime);
        }
        // move camera pilot to player
        //transform.position = player.position;
        Vector3 velocity3 = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity3, 0.05f);
        // set horizontal rotation on camera pivot
        if(Input.GetAxis("Mouse X") != 0f)
        {
            xAxisCon = Input.GetAxis("Mouse X");
        }
        else if(Input.GetAxis("Horizontal-Joystick-Right") != 0)
        {
            xAxisCon = Input.GetAxis("Horizontal-Joystick-Right");
        }
        transform.Rotate (0f, Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed, 0f);
        // vertical rotation on camera holder
        if (Input.GetAxis("Mouse Y") != 0f)
        {
            yAxisCon = Input.GetAxis("Mouse Y");
        }
        else if (Input.GetAxis("Vertical-Joystick-Right") != 0)
        {
            yAxisCon = Input.GetAxis("Vertical-Joystick-Right");
        }
        xRot +=-yAxisCon * Time.deltaTime * (verticalSpeed - Mathf.Abs((xRot+20)*1.5f));
        // lock vertical rotation to avoid 'camera flipping'
        xRot = Mathf.Clamp(xRot, vertRotMin, vertRotMax);
        // set camera holder rotation
        camHold.localEulerAngles = new Vector3(xRot, camHold.localEulerAngles.y, camHold.localEulerAngles.z);
    }
}
