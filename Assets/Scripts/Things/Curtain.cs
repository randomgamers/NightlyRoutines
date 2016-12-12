using UnityEngine;

public class Curtain : Togglable {

	GameObject curtain;
	GameObject curtainCloser;

	public override void Start() {
		curtain = GameObject.FindGameObjectWithTag("ClosableCurtain");
		curtain.SetActive(false);

		curtainCloser = GetComponentInChildren<Collider>().gameObject;
		curtainCloser.SetActive(false);
	} 

	protected override void Toggle() {
		base.Toggle();
		
		if (active) {
			curtain.SetActive(true);
			curtainCloser.SetActive(true);
		} else {
			curtain.SetActive(false);
			curtainCloser.SetActive(false);
		}
	}
}
