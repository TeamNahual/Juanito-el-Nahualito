using UnityEngine;
using System.Collections;

public class ButterflySpawner : MonoBehaviour
{
    public GameObject butterflyPrefab;

    public int spawnCount = 10;

    public float spawnRadius = 6.0f;

    [Range(0.1f, 20.0f)]
    public float velocity = 10.0f;

    [Range(0.0f, 1.0f)]
    public float velocityVariation = 0.5f;
	
	private GameObject[] butterflies;

    void Start()
    {
		butterflies = new GameObject[spawnCount];
        for (var i = 0; i < spawnCount; i++) butterflies[i] = Spawn();
    }

    public GameObject Spawn()
    {
        Vector3 position = transform.position + Random.insideUnitSphere * spawnRadius;
        var rotation = Quaternion.Slerp(transform.rotation, Random.rotation, 0.3f);
        var butterfly = Instantiate(butterflyPrefab, position, rotation) as GameObject;
        butterfly.GetComponent<ButterflyBehavior>().controller = this;
		butterfly.GetComponent<ButterflyBehavior>().origin = transform.position;
		butterfly.GetComponent<ButterflyBehavior>().velBase = velocity;
		butterfly.GetComponent<ButterflyBehavior>().velVariation = velocity * velocityVariation;
		butterfly.GetComponent<ButterflyBehavior>().spawnRadius = spawnRadius;
        return butterfly;
    }
	
	public void OnSpiritModeStart() {
		for (var i = 0; i < butterflies.Length / 2; i++)
			butterflies[i].GetComponent<ButterflyBehavior>().followJuanito = true;
	}
	
	public void OnSpiritModeEnd() {
		for (var i = 0; i < butterflies.Length; i++)
			butterflies[i].GetComponent<ButterflyBehavior>().followJuanito = false;
	}
}
