using UnityEngine;

public class CameraFollow : MonoBehaviour {

	// Use this for initialization
	GameObject ghost;
	new Camera camera;
	float widthConst = 6.0f;
	float heightConst = 3.0f;
	void Start () {
		ghost = GameObject.FindGameObjectWithTag("Ghost");
		camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 ghostPosition = ghost.transform.position;

		float widthPercentage = (ghostPosition.x + 9.5f) / 19.0f;
		float heightPercentage = (ghostPosition.z + 4.5f) / 9.0f;

		transform.position = new Vector3(6.0f * widthPercentage - 3.0f, transform.position.y, 3.0f * heightPercentage - 1.5f);
	}
}
