using UnityEngine;
using System;

public class Destroyable : Hauntable {

	public SpriteRenderer spriteRenderer;

	public Sprite notDestroyedSprite;
	public Sprite destroyedSprite;
	public bool activeOnDestroy = false;
	public bool destroyed = false;
	public float oneTimeTractionDurability;
	private DateTime tractionStartedOn;

	public AudioClip crashSound;

	protected virtual void Destroy() {
		destroyed = true;
			
		tractionStartedOn = DateTime.Now;

		PlaySound(); 
		spriteRenderer.sprite = destroyedSprite;
	}

	public override void Haunt() {
		Destroy();
	}

	protected override float GetTraction() {
		if (activeOnDestroy == destroyed) {
			Debug.Log(DateTime.Now - tractionStartedOn);
			Debug.Log(oneTimeTractionDurability);
			if ((DateTime.Now - tractionStartedOn).TotalMilliseconds < oneTimeTractionDurability) {
				return base.GetTraction();
			} else {
				return 0f;
			}
		} else {
			return 0f;
		}
	}

	protected override AudioClip SelectSound() {
		return crashSound;
	}
}
