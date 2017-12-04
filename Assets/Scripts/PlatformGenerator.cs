using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {

	public int numberOfRows;
	public int maxNumberOfPlatforms;
	public int minNumberOfPlatforms;
	public float maxPlatformSpeed;
	public float minPlatformSpeed;
	public int spaceBetweenPlatforms;
	public Transform platform;
	public Transform emptyGameObject;
	public int platformRadius;

	private float[] platformSpeed;

	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i < numberOfRows; i++)
		{
			int platformCount = Random.Range(minNumberOfPlatforms, maxNumberOfPlatforms);
			float separationAngle = 360 / platformCount;

			Vector3 currentRow = transform.position + Vector3.up * spaceBetweenPlatforms * i;

			Transform platformRow = Instantiate(emptyGameObject, transform.position, Quaternion.identity, transform) as Transform;

			for(int j = 0; j < platformCount; j++)
			{
				float angleRot = j * separationAngle;
				Instantiate(platform, GetRotationLocation(currentRow, platformRadius, angleRot), Quaternion.identity, platformRow);
			}
		}

		platformSpeed = new float[numberOfRows];
		for(int i = 0; i < platformSpeed.Length; i++)
		{
			platformSpeed[i] = Random.Range(minPlatformSpeed, maxPlatformSpeed);
			Debug.Log(platformSpeed[i]);
		}


	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < numberOfRows; i++)
		{
			transform.GetChild(i).Rotate(Vector3.up * platformSpeed[i] *Time.deltaTime, Space.World);
		}
	}

	Vector3 GetRotationLocation(Vector3 center, float radius, float angle)
	{
		Vector3 pos;

		pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
		pos.y = center.y;
		pos.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);

		return pos;
	}
}
