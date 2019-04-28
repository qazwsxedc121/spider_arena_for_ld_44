using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour {

    public float progress = 1.0f;
    private GameObject innerImage;

    private float selflen;
    private float innerlen;

	// Use this for initialization
	void Start () {
        innerImage = gameObject.transform.FindChild("inner").gameObject;
        selflen = gameObject.GetComponent<RectTransform>().rect.width;

	}
	
	// Update is called once per frame
	void Update () {
        float len = progress * selflen;
        innerImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, len);
    }
}
