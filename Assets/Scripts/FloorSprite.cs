using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSprite : MonoBehaviour {

	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		//renderer.shadowCastingMode = Renderer.shadowCastingMode.On;
		spriteRenderer.receiveShadows = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
