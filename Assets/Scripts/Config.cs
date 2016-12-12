using UnityEngine;

public class Config : MonoBehaviour {
	// Private config static instance
	private static Config instance;
	
	// Static instance getter
	public static Config Instance {
		get {
			if (instance == null)
				instance =  GameObject.FindGameObjectWithTag("Config").GetComponent<Config>();
			return instance;
		}
	}

	[Range(0, 100)] public float HAUNT_RADIUS = 1.0f;					// Distance from whence zou can haunt the object
	[Range(0, 100)] public float CAPTION_RADIUS = 2.0f;					// Distance from whence zou can haunt the object
	[Range(0, 0.5f)] public float GHOST_SPEEDUP = 0.002f;				// Speedup of the ghost per update
	[Range(0, 0.5f)] public float GHOST_SLOWDOWN = 0.001f;				// Speedup of the ghost per update
	[Range(0, 2)] public float GHOST_SPEED = 0.1f;						// Speed of the ghost per update
	[Range(0, 1)] public float GHOST_ALPHA = 0.45f;						// Ghost alpha
	[Range(0, 20)] public float HUMAN_SPEED = 0.0f;						// Speed of the human per update
	[Range(0, 1)] public float HUMAN_RANDOM_MOVEMENT_PROB = 0.00001f;	// 
	[Range(0, 10000)] public float TRACTOR_TRESHOLD = 5000.0f;				// Threshold for tractor activity
	[Range(0, 100)] public float DEFAULT_TRACTION_FORCE = 20.0f;		// Default force for tractors
	[Range(0, 20)] public float OBJECT_ROTATION_SPEED = 5f;				// How fast the held object rotates when Q/E is pressed (per update)
	[Range(0, 1)] public float LIGHT_INTENSITY = 1.0f; 					// Light intensity
	[Range(0, 100)] public int OBJECT_COUNT = 50;						// Count of objects that can interact with each other

	[Range(0, 2000)] public float ONE_TIME_TRACTION_DURABILITY = 1.0f;    // Duration of one time tractors 
	
	void Start () {}
	void Update() {}
}
