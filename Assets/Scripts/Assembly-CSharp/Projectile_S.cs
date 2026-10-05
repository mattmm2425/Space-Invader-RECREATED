using System;
using UnityEngine;

public class Projectile_S : MonoBehaviour
{
	private BoxCollider2D boxCollider;

	public Vector3 Direction;

	public float Speed = 10f;

	public Action destroyed;

	public bool chargedShot;

	private void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
	}

	private void Update()
	{
		base.transform.position += Direction * Speed * Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!chargedShot)
		{
			base.gameObject.SetActive(value: false);
			if (destroyed != null)
			{
				destroyed();
			}
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Boundary") && destroyed != null)
		{
			destroyed();
		}
	}
}
