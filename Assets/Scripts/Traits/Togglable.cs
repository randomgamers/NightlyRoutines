using UnityEngine;

public class Togglable : Hauntable {

	public SpriteRenderer spriteRenderer;
	public Sprite offSprite;
	public Sprite onSprite;
	protected bool active = false;
	public AudioClip turnOnSound;
	public AudioClip mainSound;
	public AudioClip turnOffSound;
	public AudioSource nowPlaying;

	protected virtual void Toggle() {
		active = !active;
		if (active) {
			if (onSprite) {
				spriteRenderer.sprite = onSprite;
			}
		} else {
			if (offSprite) {
				spriteRenderer.sprite = offSprite;
			}
		}
	}

	public override void Haunt() {
		Toggle();
		PlaySound();
	}

	protected override float GetTraction() {
		return active ? base.GetTraction() : 0f;
	}

	public override void PlaySound() {
		Debug.Log("Play sound");
		if (active) {
			SoundManager.Instance.Play(turnOnSound);
			nowPlaying = SoundManager.Instance.Play(mainSound, true);
		} else {
			if (nowPlaying) {
				Debug.Log("stopping");
				Debug.Log(nowPlaying);
				Debug.Log(nowPlaying.gameObject);
				Destroy(nowPlaying.gameObject);
			}
			SoundManager.Instance.Play(turnOffSound);
		}
	}

	protected override AudioClip SelectSound() {
		return null;
	}
}

