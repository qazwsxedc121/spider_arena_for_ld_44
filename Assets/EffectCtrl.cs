using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtrl : MonoBehaviour {

    private float startTime = 0.0f;
	// Use this for initialization
	void Start () {
        		
	}
	
	// Update is called once per frame
	void Update () {
        startTime += Time.deltaTime;
        if(startTime > 0.5f) {
            Destroy(gameObject);
        }
	}
}
