using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Togglable {

    public Sprite[] sprites = new Sprite[2]; // here via inspector lay sprites in order
    int stage = 0; // current frame number
    private float nextTime;

	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();

		if(Time.time >= nextTime)
        { 
            nextTime = Time.time + 0.1f;
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[stage];
            stage = (stage+2 > sprites.Length) ? 0 : stage+1;
        }

	}
}
