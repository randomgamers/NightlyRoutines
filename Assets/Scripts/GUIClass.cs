using UnityEngine;
public class GUIClass : MonoBehaviour {
    static GUIClass instance;
    public string leftText;
	public string oldLeftText;
    public string rightText;
	public string oldRightText;
 	private GUIStyle leftStyle = new GUIStyle();
	private GUIStyle rightStyle = new GUIStyle();
	private GUIStyle centerStyle = new GUIStyle();
	private bool endingLevel = false;
	private Texture2D fadeTexture;
	private float fadeParam;
	private bool switchFade;
	private Rect leftRect;
	private Rect rightRect;
	private Rect fadeTextureRect;
	private float fadeStep;
    public static GUIClass Instance {
		get {
			if (instance == null)
				instance =  GameObject.FindGameObjectWithTag("Global").GetComponent<GUIClass>();
			return instance;
		}
	}

	public void Start() {
		leftStyle.alignment = TextAnchor.MiddleLeft;
		rightStyle.alignment = TextAnchor.MiddleRight;
		centerStyle.alignment = TextAnchor.MiddleCenter;
		centerStyle.normal.textColor = Color.white;

		fadeTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		fadeParam = 0.0f;
		switchFade = false;
		fadeStep = 0.05f;

		Vector3 leftWidth = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.1f, 0.0f));
		Vector3 leftOffset = Camera.main.ViewportToScreenPoint(new Vector3(0.1f, 0.0f, 0.0f));
		Vector3 rightOffset = Camera.main.ViewportToScreenPoint(new Vector3(0.4f, 0.0f, 0.0f));

		leftRect = new Rect(leftOffset.x, 0.0f, leftWidth.x, leftWidth.y);;
		rightRect = new Rect(leftWidth.x, 0.0f, rightOffset.x, leftWidth.y);
		fadeTextureRect = new Rect(0.0f, 0.0f, Screen.width, leftWidth.y);
	}

	public void LevelEnd() {
		oldLeftText = leftText;
		oldRightText = rightText;
		endingLevel = true;
	}

	public void Update() {
		if (endingLevel || switchFade) {
			fadeParam += fadeStep * (switchFade ? -1 : 1);
			if (fadeParam > 1.5f) {
				switchFade = true;
				endingLevel = false;
			}
			if (fadeParam < 0.0f) {
				switchFade = false;
			}
		}
	}

	void OnGUI() {

		if (Human.Instance.gamePaused) return;

		GUI.color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
		GUI.DrawTexture(fadeTextureRect, fadeTexture, ScaleMode.StretchToFill);

		if (endingLevel || switchFade) {
			GUI.color = new Color(0, 0, 0, Mathf.Clamp(fadeParam, 0.0f, 0.7f));
			GUI.DrawTexture(fadeTextureRect, fadeTexture, ScaleMode.StretchToFill);
			GUI.color = Color.white;
			centerStyle.normal.textColor = new Color(1, 1, 1, Mathf.Clamp(fadeParam, 0.0f, 0.7f));
			leftStyle.normal.textColor = new Color(0, 0, 0, 1.0f - Mathf.Clamp(fadeParam, 0.0f, 0.7f));
			rightStyle.normal.textColor = new Color(0, 0, 0, 1.0f - Mathf.Clamp(fadeParam, 0.0f, 0.7f));

			GUI.Label(fadeTextureRect, "QUEST COMPLETE!", centerStyle);
		}

		GUI.skin.label.normal.textColor = Color.black;

		if (endingLevel) {
			GUI.Label(leftRect, oldLeftText, leftStyle);
			GUI.Label(rightRect, oldRightText, rightStyle);
		} else {
			GUI.Label(leftRect, leftText, leftStyle);
			GUI.Label(rightRect, rightText, rightStyle);
		}
	}
}