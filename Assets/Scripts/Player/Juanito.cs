using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class Juanito : MonoBehaviour {

	public static Juanito ins;

	public GameObject JuanitoHuman;
	public GameObject JuanitoSpirit;

	public Animator HumanAnim;
	public Animator SpiritAnim;

	public bool butterflyRelic = false;
	public bool statueRelic = false;
    public bool hasFood = false;
	public bool isPushing = false;

	
	private float spirit_time_limit = 50;
	public bool SpiritState = false;
	public bool inButterflyZone = false;
	public Vector3 butterflyZoneOrigin;
	public float butterflyZoneRadius;
	public bool spiritCanWalkIntoBodyFlag = false;

	private Vector3 lockedSpiritPosition;

	[HideInInspector]
	public SpiritController SpiritControl;

	private float numberOfButterflies = 0;

	int MAX_SPIRIT_COUNT = 100;

	// Use this for initialization
	void Awake () {
		ins = this;

		Physics.IgnoreCollision(JuanitoHuman.GetComponent<Collider>(), JuanitoSpirit.GetComponent<Collider>());
		SpiritControl = JuanitoSpirit.GetComponent<SpiritController>();
	}
	
	// Update is called once per frame
	void Update () {
		SpiritHandler();
	}

	public float GetSpiritCount()
	{
		return numberOfButterflies;
	}

	public bool AddSpiritCount(float count)
	{
		if(numberOfButterflies < MAX_SPIRIT_COUNT)
		{
			numberOfButterflies += count;
			return true;
		}
		
		return false;
	}

	public void DelSpiritCount(float count)
	{
		numberOfButterflies = Mathf.Max(numberOfButterflies - count, 0); 
	}

	private void EnterSpiritState()
 	{
 		JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
 		JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;

	    if (FancyCam.ins) FancyCam.ins.player = JuanitoSpirit.transform;
	    if (CameraPivotFollow.ins) CameraPivotFollow.ins.player = JuanitoSpirit.transform;
	    
	    JuanitoSpirit.transform.position = JuanitoHuman.transform.position + JuanitoHuman.transform.forward;
		JuanitoSpirit.transform.rotation = JuanitoHuman.transform.rotation;
		JuanitoSpirit.SetActive(true);
		SpiritState = true;
		spiritCanWalkIntoBodyFlag = false;

		JuanitoHuman.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		
		// Shader related
		Shader.SetGlobalVector("_MK_FOG_SPIRIT_MODE_ORIGIN", butterflyZoneOrigin);
		Shader.SetGlobalFloat("_MK_FOG_SPIRIT_MODE_RADIUS", butterflyZoneRadius);
		Shader.SetGlobalInt("_MK_FOG_SPIRIT_MODE_ENABLED", 1);
 	}

 	private void SpiritHandler()
 	{	
 		if(inButterflyZone)
 		{
	 		if(Input.GetKeyDown(KeyCode.Q) || CrossPlatformInputManager.GetButtonDown("Toggle-Spirit"))
			{
				if(!SpiritState)
				{
					// lockedSpiritPosition = JuanitoHuman.transform.position;
					EnterSpiritState();
				}
				else
				{
					EndSpiritState();
				}
			}

			/*if(SpiritState)
			{
				// JuanitoHuman.transform.position = lockedSpiritPosition;
			}*/
		}
		if (SpiritState && JuanitoSpirit.transform.hasChanged) {
			float dist = Vector3.Distance(JuanitoHuman.transform.position, JuanitoSpirit.transform.position);
			
			if (spiritCanWalkIntoBodyFlag)
			{
				if (dist < 2)
				{
					EndSpiritState();
				}
			} else {
				if (dist > 4)
				{
					spiritCanWalkIntoBodyFlag = true;
				}
			}
		}
 	}

 	public void EndSpiritState()
 	{
 		SpiritControl.currentFollower = null;
 		JuanitoSpirit.SetActive(false);
        if (FancyCam.ins) FancyCam.ins.player = JuanitoHuman.transform;
        if (CameraPivotFollow.ins) CameraPivotFollow.ins.player = JuanitoHuman.transform;
 		JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 		JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
 		SpiritState = false;
		UIManager.instance.TooltipDisable();
		JuanitoHuman.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		
		// Shader related
		Shader.SetGlobalInt("_MK_FOG_SPIRIT_MODE_ENABLED", 0);

 	}

 	public bool CheckFacingObjects(GameObject[] targetObjects, int layerMask = -1)
	{
		if(layerMask == -1) layerMask = LayerMask.GetMask("Default");

		Vector3 fwd = JuanitoHuman.transform.TransformDirection(Vector3.forward);
		Ray ray = new Ray(JuanitoHuman.transform.position, fwd);
		RaycastHit hit;

		Debug.DrawRay(JuanitoHuman.transform.position, fwd, Color.green);

		if(Physics.Raycast(ray, out hit, 1.5f, layerMask))
		{
			Debug.Log(hit.transform.gameObject.name);
			foreach(GameObject obj in targetObjects)
			{
				if(obj == hit.transform.gameObject)
				{
					//Debug.Log(obj.name);
					return true;
				}
			}
		}

		return false;
	}

	public bool CheckFacingObjectsSpirit(GameObject[] targetObjects, int layerMask = -1)
	{
		if(layerMask == -1) layerMask = LayerMask.GetMask("Default");

		Vector3 fwd = JuanitoSpirit.transform.TransformDirection(Vector3.forward);
		Ray ray = new Ray(JuanitoSpirit.transform.position + Vector3.up * 0.5f, fwd);
		RaycastHit hit;

		Debug.DrawRay(JuanitoSpirit.transform.position, fwd, Color.green);

		if(Physics.Raycast(ray, out hit, 1, layerMask))
		{
			//Debug.Log(hit.transform.gameObject.name);
			foreach(GameObject obj in targetObjects)
			{
				if(obj == hit.transform.gameObject)
				{
					//Debug.Log(obj.name);
					return true;
				}
			}
		}

		return false;
	}
}
