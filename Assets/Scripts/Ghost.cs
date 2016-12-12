using UnityEngine;
using System;
using System.Collections.Generic;

public class Ghost : MonoBehaviour {
	// Private ghost static instance
	private static Ghost instance;
	
	// Static instance getter
	public static Ghost Instance {
		get {
			if (instance == null)
				instance =  GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
			return instance;
		}
	}
	
	// General shit
	private float currectSpeedX;
	private float currectSpeedZ;
	private bool stopX;
	private bool stopZ;
	private bool showCaptions;

	public AudioClip cantPutDownSound; 

    // What is the ghost holding 
    private Movable heldThing;

	// Controlls 
	private bool arrowUpPressed;
	private bool arrowDownPressed;
	private bool arrowLeftPressed;
	private bool arrowRightPressed;
	private bool rotationLeftPressed;
	private bool rotationRightPressed;
	private bool hauntObjectButtonPressed;
	private bool moveFurtnireButtonPressed;
	private bool toggleCaptionsButtonPressed;

	public SpriteRenderer spriteRenderer;
	public AudioClip pickUpSound;
	private AudioSource pickUpSoundSource;
    public Sprite[] sprites = new Sprite[3]; // here via inspector lay sprites in order
    int stage = 0; // current frame number
    private float nextTime;

    // Position 
	public Vector3 Position {
		get {
			return gameObject.transform.position;
		}
	}
	
	public float X {
		get {
			return gameObject.transform.position.x;
		}
	}
	
	public float Y {
		get {
			return gameObject.transform.position.y;
		}
	}

	public float Z {
		get {
			return gameObject.transform.position.z;
		}
	}
	
	// UNITY METHODS
	void Start () {
		heldThing = null;
		showCaptions = true;
		stopX = false;
		stopZ = false;
	}
	
	void Update() {
		// Keyboard
        UpdateContollButtons();

		UpdateFrame();

		// Pause 
        if (Human.Instance.gamePaused) {
            return;
        }

		// Show thing captions
		if (toggleCaptionsButtonPressed) {
			showCaptions = !showCaptions; 
		}
		foreach (Thing thing in FindObjectsOfType(typeof(Thing))) {
			thing.SetGUI(showCaptions ? Utils.Clip(1f - thing.distanceFromGhost/Config.Instance.CAPTION_RADIUS, 0f, 1f) : 0f);	
		}

		// Haunt an object 
        if (hauntObjectButtonPressed) {
			Hauntable nearestHauntable = Hauntable.FindNearestToGhost();
			if (nearestHauntable) {
				nearestHauntable.Haunt();
			}
        }

		// Move with shit 
        if (moveFurtnireButtonPressed) {
			if (heldThing) {
				try {
					heldThing.PutDown();
					Destroy(pickUpSoundSource);
					heldThing = null;
				} catch (Exception e) {
					SoundManager.Instance.Play(cantPutDownSound);
				}
			} else {
				Movable nearestMovable = Movable.FindNearestToGhost();
				if (nearestMovable) {
					heldThing = nearestMovable;
					heldThing.PickUp();
					pickUpSoundSource = SoundManager.Instance.Play(pickUpSound, true);
				}
			}
        }

		// Compute DPosition
		Vector3 dPosition = DPosition();
		if (heldThing) {
			dPosition *= heldThing.slowdownCoef;
		}
		Vector3 desiredPos = gameObject.transform.position + dPosition;

		float newX = Utils.Clip(desiredPos.x, -9.5f, 9.5f);
		float newZ = Utils.Clip(desiredPos.z, -4.5f, 4.5f);
		// stopX = Utils.AreTheseTwoFloatsNearlyTheSamePlease(desiredPos.x, newX);
		// stopZ = Utils.AreTheseTwoFloatsNearlyTheSamePlease(desiredPos.z, newZ);
		stopX = newX <= -9.5f || newX >= 9.5f; 
		stopZ = newZ <= -4.5f || newZ >= 4.5f; 

		// Move/Rotate 
		if (heldThing) {
			heldThing.Move(new Vector3(newX, heldThing.Position.y, newZ));
		
			Rigidbody[] thingRigidBodies = heldThing.gameObject.GetComponentsInChildren<Rigidbody>();
			if (stopX) {
				foreach (Rigidbody thingRigidBody in thingRigidBodies) thingRigidBody.velocity = new Vector3(0f, 0f, thingRigidBody.velocity.z);
			}
			if (stopZ) {
				foreach (Rigidbody thingRigidBody in thingRigidBodies) thingRigidBody.velocity = new Vector3(thingRigidBody.velocity.x, 0f, 0f);
			}

			// heldThing.MoveBy(dPosition);
			
			float dr = 0f;
			if (rotationLeftPressed) dr -= Config.Instance.OBJECT_ROTATION_SPEED * heldThing.slowdownCoef;
			if (rotationRightPressed) dr += Config.Instance.OBJECT_ROTATION_SPEED * heldThing.slowdownCoef;
			heldThing.RotateBy(dr);
		}

		// Move the ghost (potentially slowdowned speed)
		gameObject.transform.position = new Vector3(newX, Y, newZ);

		Rigidbody ghostRigidBody = gameObject.GetComponent<Rigidbody>();
		if (stopX) {
			ghostRigidBody.velocity = new Vector3(0f, ghostRigidBody.velocity.y, ghostRigidBody.velocity.z);
			currectSpeedX = 0f;
		}
		if (stopZ) {
			ghostRigidBody.velocity = new Vector3(ghostRigidBody.velocity.x, ghostRigidBody.velocity.y, 0f);
			currectSpeedZ = 0f;
		}
	}
	
    private void UpdateContollButtons() {
		arrowUpPressed = Input.GetButton("ArrowUp");
		arrowDownPressed = Input.GetButton("ArrowDown");
		arrowLeftPressed = Input.GetButton("ArrowLeft");
		arrowRightPressed = Input.GetButton("ArrowRight");
		rotationLeftPressed = Input.GetButton("RotationLeft");
		rotationRightPressed = Input.GetButton("RotationRight");
		hauntObjectButtonPressed = Input.GetButtonDown("HauntObject");
		moveFurtnireButtonPressed = Input.GetButtonDown("MoveFurniture");
		toggleCaptionsButtonPressed = Input.GetButtonDown("ToggleCaptions");
    }

    private Vector3 DPosition() {
		currectSpeedX -= Config.Instance.GHOST_SLOWDOWN * Math.Sign(currectSpeedX);
		currectSpeedZ -= Config.Instance.GHOST_SLOWDOWN * Math.Sign(currectSpeedZ);

        if (arrowLeftPressed) currectSpeedX -= Config.Instance.GHOST_SPEEDUP;
        if (arrowRightPressed) currectSpeedX += Config.Instance.GHOST_SPEEDUP;
        if (arrowUpPressed) currectSpeedZ += Config.Instance.GHOST_SPEEDUP;
        if (arrowDownPressed) currectSpeedZ -= Config.Instance.GHOST_SPEEDUP;
		
		currectSpeedX = Utils.Clip(currectSpeedX, -Config.Instance.GHOST_SPEED, Config.Instance.GHOST_SPEED);
		currectSpeedZ = Utils.Clip(currectSpeedZ, -Config.Instance.GHOST_SPEED, Config.Instance.GHOST_SPEED);
        return new Vector3(currectSpeedX, 0.0f, currectSpeedZ);
    }

	private void UpdateFrame() {
    	if(Time.time >= nextTime)
		{ 
			nextTime = Time.time + 0.15f;
			spriteRenderer.sprite = sprites[stage];
			stage = (stage+2 > sprites.Length) ? 0 : stage+1;
		}
	}
}
