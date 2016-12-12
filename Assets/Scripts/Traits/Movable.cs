using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class Movable : Thing {

	public bool isPickedUp;
	public AudioClip pickUpSound;
	public AudioClip putDownSound;

	public static Movable FindNearestToGhost() {
		Movable minMovable = null;
		double minDist = double.MaxValue;

		foreach (Movable movable in FindObjectsOfType(typeof(Movable))) {
			if ((movable.distanceFromGhost < Config.Instance.HAUNT_RADIUS) && (movable.distanceFromGhost < minDist)) {
				minDist = movable.distanceFromGhost;
				minMovable = movable;
			}
		}

		return minMovable;
	}
	public void RotateBy(float angle) {
		gameObject.transform.Rotate(new Vector3(0f, 1f, 0f), angle);
	}

	public void Move(Vector3 position) {
		gameObject.transform.position = position;
	}

	public void MoveBy(Vector3 position) {
		Move(Position + position);
	}

	public void PickUp() {
		foreach (Collider collider in Utils.GetAllColliders(gameObject, false)) {
			collider.enabled = false;
		}
		
		SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
		Color c = spriteRenderer.color;
		
		c.a = Config.Instance.GHOST_ALPHA;
		spriteRenderer.color = c;
		spriteRenderer.transform.position += new Vector3(0.0f, 1.0f, 0.0f); 

		PlaySound();
	}

	public void PutDown() {
		UnityEngine.Object[] myColliders = Utils.GetAllColliders(gameObject, false);
		foreach (Collider collider in myColliders) {
			collider.enabled = true;
		}

		if (CollisionMatrix.isColliding) {
			// revert the shit
			foreach (Collider collider in myColliders) {
				collider.enabled = false;
			}
			// throw some shit
			Debug.Log("DENIED");
			throw new Exception("no");
		}
		
		SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
		Color c = spriteRenderer.color;
		
		c.a = 1.0f;
		spriteRenderer.color = c;
		spriteRenderer.transform.position -= new Vector3(0.0f, 1.0f, 0.0f);

		PlaySound();
	}

	protected override AudioClip SelectSound() {
		if (isPickedUp) {
			return putDownSound;
		} else {
			return pickUpSound;
		}
	}

	public override void Start() {
		foreach (Collider c in Utils.GetAllColliders(gameObject, false)) {
			Collider clone = (Collider) Utils.CopyComponent(c);

			if (clone.GetType().Equals(typeof(MeshCollider))) {
				((MeshCollider) clone).convex = true;
			}
			clone.isTrigger = true;

			Rigidbody rigidbody = c.gameObject.AddComponent<Rigidbody>();
			rigidbody.useGravity = false;
			rigidbody.isKinematic = false;
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;

			c.gameObject.AddComponent<ChildTrigger>();
		}
	}

}
