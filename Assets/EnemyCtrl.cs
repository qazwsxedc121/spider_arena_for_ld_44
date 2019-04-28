using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour {


    private int paralysis = 0;
    public int maxParalysis = 5;
    public bool paralysised = false;
    public bool binded = false;
    private GameObject paraicon;

    private float t = 0.0f;
    private float aiInterval = 3.0f;

	// Use this for initialization
	void Start () {
        paraicon = transform.Find("paraicon").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if(t > aiInterval && !paralysised) {
            t = 0.0f;
            AITick();
        }
	}

    public void OnShoot(int dam) {
        paralysis += dam;
        CheckParalysised();
    }

    public void OnBite(int dam) {
        paralysis += dam;
        CheckParalysised();
    }

    void CheckParalysised() {
        if(paralysis >= maxParalysis) {
            paralysised = true;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            UpdateAnimation();
        }

    }

    void AITick() {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    }

    void StopAI() {
        
    }

    void UpdateAnimation() {
        if (paralysised) {
            paraicon.SetActive(true);
        }
    }

    void OnBind() {
        binded = true;
        UpdateAnimation();
    }
}
