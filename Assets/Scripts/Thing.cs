using UnityEngine;
using System.Collections.Generic;
using Tuples;

public class Thing : MonoBehaviour {
	public Vector3 Position {
		get {   return gameObject.transform.position;   }
	}
	public float slowdownCoef;
	public float baseTraction;
	public float distanceFromGhost;
	private float GUIlevel = 0.0f;
	private GUIStyle centeredStyle = null;
	
	protected virtual float GetTraction() {
		return baseTraction;
	}

	private static List<Tuple<string, string>> validCollisionPairs;

	public static Vector3 TractionForPosition(Vector3 otherPosition) {
        Vector3 totalTraction = Vector3.zero;

        Object[] things = FindObjectsOfType(typeof(Thing));
        foreach (Thing thing in things) {
            float distance = Utils.Distance(thing.Position, otherPosition);
            if (distance < Config.Instance.TRACTOR_TRESHOLD) {
                totalTraction += (thing.Position - otherPosition) * thing.GetTraction() / Mathf.Pow(distance, 2.0f);
            }
        }

        return new Vector3(totalTraction.x, 0f, totalTraction.z);
    }

	public virtual void Start() {
		validCollisionPairs = new List<Tuple<string, string>>() { 
			new Tuple<string, string>("Bar", "Glass"),
			new Tuple<string, string>("Glass", "Bar"),
			new Tuple<string, string>("Plate", "Table"),
			new Tuple<string, string>("Table", "Plate"),
			new Tuple<string, string>("Honey", "Kitchen"),
			new Tuple<string, string>("Kitchen", "Honey"),
			new Tuple<string, string>("Kitchen", "Microwave"),
			new Tuple<string, string>("Microwave", "Kitchen")
		};
	}

	public virtual void Update() {
		distanceFromGhost = Utils.Distance(Position, Ghost.Instance.Position);
	}

	public virtual void PlaySound() {
		SoundManager.Instance.Play(SelectSound());
	}

    protected virtual AudioClip SelectSound() {
		return null;
	}

	public void OnTriggerEnter(Collider other) {
		// Legacy resons, don't delete
	}

	public virtual void OnTriggerStay(Collider other) {
		// Debug.Log("shit: " + other.gameObject.transform.parent.gameObject.name + " " + name);
		// Debug.Log("On trigger stay before check: " + other.gameObject.transform.parent.gameObject.name + " " + name);
		if (CheckedCollision(other.gameObject.transform.parent.gameObject)) return;
		
		//Debug.Log("Staying on " + other.gameObject.transform.parent.gameObject + " " + name);
		CollisionMatrix.isColliding = true;
	}

	public virtual void OnTriggerExit(Collider other) {
		// Debug.Log("On trigger exit before check");
		if (CheckedCollision(other.gameObject.transform.parent.gameObject)) return;

		// Debug.Log("Exiting");
		
		CollisionMatrix.isColliding = false;  // CollisionMatrix.RecordCollisionEnter(other.GetInstanceID());
	}

	public bool CheckedCollision(GameObject other) {
		if (other.tag.Equals("Ghost") || other.tag.Equals("Human")) return true;
		if (gameObject.name.Equals("Room") || other.name.Equals("Room")) return true;
		if (validCollisionPairs.Contains(new Tuple<string, string>(gameObject.name, other.name))) return true;
		return false;
	}

	public void SetGUI(float newGUIlevel) {
		GUIlevel = newGUIlevel;
	}

	void OnGUI() {

		if (Human.Instance.gamePaused) return;

		if (centeredStyle == null) {
			centeredStyle = GUI.skin.GetStyle("Label");
		}

		if (GUIlevel > 0.0f) {
			Vector3 newPosition = GUIUtility.ScreenToGUIPoint(Camera.main.WorldToScreenPoint(Position));
			//centeredStyle.normal.textColor = new Color(1, 1, 1, GUIlevel);
			//centeredStyle.alignment = TextAnchor.UpperCenter;

			Color newBlack = new Color(0, 0, 0, GUIlevel);
			Color newWhite = new Color(1, 1, 1, GUIlevel); 
			Utils.DrawOutline(new Rect(newPosition.x - 25.0f, (Screen.height - newPosition.y) - 35.0f, 50, 70), gameObject.name, centeredStyle, newBlack, newWhite);
		}
	}
}