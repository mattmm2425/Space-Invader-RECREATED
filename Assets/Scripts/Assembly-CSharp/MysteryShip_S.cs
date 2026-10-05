using UnityEngine;

public class MysteryShip_S : MonoBehaviour
{
	public float speed = 5f;

	public float cycleTime = 30f;

	public int score = 300;

	private Vector2 leftDestination;

	private Vector2 rightDestination;

	private int direction = -1;

	private bool spawned;

	private void Start()
	{
		Vector3 vector = Camera.main.ViewportToWorldPoint(Vector3.zero);
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(Vector3.right);
		leftDestination = new Vector2(vector.x - 1f, base.transform.position.y);
		rightDestination = new Vector2(vector2.x - 1f, base.transform.position.y);
		Despawn();
	}

	private void Update()
	{
		if (spawned)
		{
			if (direction == 1)
			{
				MoveRight();
			}
			else
			{
				MoveLeft();
			}
		}
	}

	private void MoveRight()
	{
		base.transform.position += speed * Time.deltaTime * Vector3.right;
		if (base.transform.position.x >= rightDestination.x)
		{
			Despawn();
		}
	}

	private void MoveLeft()
	{
		base.transform.position += speed * Time.deltaTime * Vector3.left;
		if (base.transform.position.x <= leftDestination.x)
		{
			Despawn();
		}
	}

	private void Spawn()
	{
		direction *= -1;
		base.gameObject.SetActive(value: true);
		if (direction == 1)
		{
			base.transform.position = leftDestination;
		}
		else
		{
			base.transform.position = rightDestination;
		}
		spawned = true;
	}

	private void Despawn()
	{
		spawned = false;
		base.gameObject.SetActive(value: false);
		if (direction == 1)
		{
			base.transform.position = rightDestination;
		}
		else
		{
			base.transform.position = leftDestination;
		}
		Invoke("Spawn", cycleTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
		{
			Despawn();
			GameManager_S.Instance.MysteryShipKilled(this);
		}
	}
}
