using System;
using UnityEngine;

public class Invader_S : MonoBehaviour
{
	public Sprite[] AnimationSprites;

	public float AnimationTime = 1f;

	private SpriteRenderer SpriteRenderer;

	private int AnimationFrame;

	public Action Killed;

	public int score = 10;

	private void Awake()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		InvokeRepeating("AnimateSprite", AnimationTime, AnimationTime);
	}

	private void AnimateSprite()
	{
		AnimationFrame++;
		if (AnimationFrame >= AnimationSprites.Length)
		{
			AnimationFrame = 0;
		}
		SpriteRenderer.sprite = AnimationSprites[AnimationFrame];
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
		{
			Killed();
			base.gameObject.SetActive(value: false);
			GameManager_S.Instance.InvaderKilled(this);
		}
	}
}
