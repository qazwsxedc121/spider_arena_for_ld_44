using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCtrl : MonoBehaviour {


    public float interval = 3.0f;
    public float t = 0.0f;
    public GameObject enemyPrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if(t > interval) {
            SpawnRandom();
            t = 0.0f;
            interval = interval > 1.0f ? interval - 0.1f : interval;
        }
	}

    void SpawnRandom() {
        Instantiate(enemyPrefab, new Vector3(Random.Range(-6, 6), Random.Range(-4, 4), 0), Quaternion.identity);       
    }
}
