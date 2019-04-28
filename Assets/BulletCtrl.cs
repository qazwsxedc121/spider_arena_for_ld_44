using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

    public int dam = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Bound") {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<EnemyCtrl>().OnShoot(dam);
            Destroy(gameObject);
        }
    }
}
