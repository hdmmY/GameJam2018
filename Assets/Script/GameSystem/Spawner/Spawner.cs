using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public float Radius = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 SpawnPosition()
    {
        var dir = Random.insideUnitCircle * Radius;
        return transform.position + new Vector3(dir.x, 0, dir.y);

    }
}
