using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {

	public Vector3 movementSpeed;
	private new Rigidbody rigidbody;

    private bool pauseButtonPressed;
    public bool gamePaused;
	private Vector3 prepauseSpeed;
	private int direction;
	private float nextTime;
	private int spriteStage;
	private int clothesLevel;
	private int numberOfFramesPerDirection = 2;

	private static Human instance;
	public SpriteRenderer spriteRenderer;
	public Sprite[] sprites = new Sprite[5];  // here via inspector lay sprites in order

	// STATIC METHODS
	public static Human Instance {
		get {
			if (instance == null)
				instance = GameObject.FindGameObjectWithTag("Human").GetComponent<Human>();
			return instance;
		}
	}

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

	// Use this for initialization
	void Start () {
		rigidbody = gameObject.GetComponent<Rigidbody>();
		movementSpeed = new Vector3(Config.Instance.HUMAN_SPEED,0,-Config.Instance.HUMAN_SPEED);
		rigidbody.velocity = movementSpeed;
		prepauseSpeed = gameObject.GetComponent<Rigidbody>().velocity;
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		gamePaused = true;
		currentLevel = 0;
		questProgress = 0;
		gameFinished = false;
		RefreshStatusBar();
		nextTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log(rigidbody.velocity);
		pauseButtonPressed = Input.GetButtonDown("Pause");
        if (pauseButtonPressed) {
			if (gamePaused) {
				GameObject.FindGameObjectWithTag("PausedPanel").transform.position -= new Vector3(100.0f, 0.0f, 0.0f);
				
				gameObject.GetComponent<Rigidbody>().velocity = prepauseSpeed;
	            gamePaused = false;
			} else {
				GameObject.FindGameObjectWithTag("PausedPanel").transform.position += new Vector3(100.0f, 0.0f, 0.0f);

				prepauseSpeed = gameObject.GetComponent<Rigidbody>().velocity;
				gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
	            gamePaused = true;
			}
        }

		// Pause 
        if (gamePaused) {
            return;
        }

		rigidbody.velocity = rigidbody.velocity.normalized;
		// UpdatePosition();
		if (Random.value < Config.Instance.HUMAN_RANDOM_MOVEMENT_PROB) {
			float speed = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z).magnitude;
			float angle = Random.value;
			rigidbody.velocity = new Vector3(angle * speed, 0, (1-angle) * speed);
		}
		rigidbody.velocity += 0.005f * Thing.TractionForPosition(Position);
		rigidbody.velocity = rigidbody.velocity.normalized / 1.4f;

		if (Mathf.Abs(rigidbody.velocity.x) > Mathf.Abs(rigidbody.velocity.z)) { // left or right
			if (rigidbody.velocity.x > 0) { // right
				direction = 1;
			} else { // left
				direction = 3;
			}
		} else { // up or down
			if (rigidbody.velocity.z > 0) { // up
				direction = 0;
			} else { // down
				direction = 2;
			}
		}

		// direction = 2;
		if (Time.time >= nextTime) {
			int spriteID = numberOfFramesPerDirection * 4 * clothesLevel;
			spriteID += direction * 2;
			spriteID += spriteStage;
			spriteRenderer.sprite = sprites[spriteID];
			if(direction == 3) {
				spriteRenderer.flipX = true;
			} else {
				spriteRenderer.flipX = false;
			}
            nextTime = Time.time + 0.2f;
            spriteStage = (spriteStage >= numberOfFramesPerDirection - 1) ? 0 : spriteStage + 1;
        }
	}

	private void Move2Menu() {
		Application.LoadLevel("Scenes/MenuScene");
	} 

	public void Win() {
		GameObject.FindGameObjectWithTag("WinPanel").transform.position -= new Vector3(100f, 0f, 0f);
		Invoke("Move2Menu", 3f);
	}

	public void Lose() {
		GameObject.FindGameObjectWithTag("LosePanel").transform.position -= new Vector3(100f, 0f, 0f);
		Invoke("Move2Menu", 3f);
	}

	public void SetClothesLevel(int level) {
		Debug.Log("SETCLOSTHES");
		clothesLevel = level;
	}

		// ==============================================================
	// ==============================================================
	// ==============================================================
	// ==============================================================
	// ==============================================================
	// TODO: THIS SHIT GO TO HUMAN

	private int currentLevel;
	private int questProgress;
	private bool gameFinished;

	private void RefreshStatusBar() {
		// Debug.Log("RefreshStatusBar");
		TextMesh statusBar = GameObject.FindGameObjectWithTag("StatusBar").GetComponent<TextMesh>();
		List<string> quest = Quests.GetQuest(currentLevel);
		
		if (quest == null) {
			Human.Instance.Win();
			return;
		}
		
		GUIClass.Instance.leftText = "Quest #" + (currentLevel+1) + ": " + Quests.GetQuestName(currentLevel);
		string rightText = "";
		for (int i=0; i < quest.Count; i++) {
			string color = (i < questProgress) ? "green" : "red";
			rightText += "<color=" + color + ">" + quest[i] + "</color>";
			rightText += (i+1 < quest.Count) ? " <b>|</b> " : "";
		}
		GUIClass.Instance.rightText = rightText;
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Window")) {
			Lose();
		}

		GameObject otherGameObject = other.gameObject.transform.parent.gameObject;
		if (!otherGameObject.GetComponent<Thing>()) {
			return;
		}

		List<string> quest = Quests.GetQuest(currentLevel);
		string nextObjective = quest[questProgress];

		if (otherGameObject.name.Equals(nextObjective)) {
			Quests.QuestProgressCallback(currentLevel, questProgress);

			questProgress++;
			RefreshStatusBar();  // do it also here so that Tomas dosn't picovat
			if (questProgress >= quest.Count) {
				questProgress = 0;
				currentLevel++;

				List<string> nextQuest = Quests.GetQuest(currentLevel);
				if (nextQuest == null) {
					Win();
					return;
				}
				GUIClass.Instance.LevelEnd();
			}
			RefreshStatusBar();
		}
	}
}
