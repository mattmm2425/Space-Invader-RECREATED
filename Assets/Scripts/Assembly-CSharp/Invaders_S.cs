using System;
using UnityEngine;

public class Invaders_S : MonoBehaviour
{
	public Invader_S[] prefabs;

	public int rows = 5;

	public int columns = 11;

	public AnimationCurve speed;

	private Vector3 Direction = Vector3.right;

	public float MissileAttackRate = 1f;

	public Projectile_S MissilePrefab;

	private Vector3 initialPosition;

	public int AmountKilled { get; private set; }

	public int TotalInvaders => rows * columns;

	public float PercentKilled => (float)AmountKilled / (float)TotalInvaders;

	public int AmountAlive => TotalInvaders - AmountKilled;

	private void Awake()
	{
		initialPosition = base.transform.position;
		CreateInvaderGrid();
	}

	private void CreateInvaderGrid()
	{
		for (int i = 0; i < rows; i++)
		{
			float num = 2f * (float)(columns - 1);
			float num2 = 2f * (float)(rows - 1);
			Vector2 vector = new Vector2((0f - num) / 2f, (0f - num2) / 2f);
			Vector3 vector2 = new Vector3(vector.x, vector.y + (float)i * 2f, 0f);
			for (int j = 0; j < columns; j++)
			{
				Invader_S invader_S = UnityEngine.Object.Instantiate(prefabs[i], base.transform);
				invader_S.Killed = (Action)Delegate.Combine(invader_S.Killed, new Action(InvaderKilled));
				Vector3 localPosition = vector2;
				localPosition.x += (float)j * 2f;
				invader_S.transform.localPosition = localPosition;
			}
		}
	}

	private void Update()
	{
		base.transform.position += Direction * speed.Evaluate(PercentKilled) * Time.deltaTime;
		Vector3 vector = Camera.main.ViewportToWorldPoint(Vector3.zero);
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(Vector3.right);
		foreach (Transform item in base.transform)
		{
			if (item.gameObject.activeInHierarchy)
			{
				if (Direction.x < 0f && item.position.x < vector.x + 1f)
				{
					AdvanceRow();
				}
				else if (Direction.x > 0f && item.position.x > vector2.x - 1f)
				{
					AdvanceRow();
				}
			}
		}
	}

	private void Start()
	{
		InvokeRepeating("MissileAttack", MissileAttackRate, MissileAttackRate);
	}

	private void AdvanceRow()
	{
		Direction.x *= -1f;
		Vector3 position = base.transform.position;
		position.y -= 0.5f;
		base.transform.position = position;
	}

	private void InvaderKilled()
	{
		AmountKilled++;
		_ = AmountKilled;
		_ = TotalInvaders;
	}

	private void MissileAttack()
	{
		foreach (Transform item in base.transform)
		{
			if (item.gameObject.activeInHierarchy && UnityEngine.Random.value < 1f / (float)AmountAlive)
			{
				UnityEngine.Object.Instantiate(MissilePrefab, item.position, Quaternion.identity);
				break;
			}
		}
	}

	public void ResetInvaders()
	{
		Direction = Vector3.right;
		base.transform.position = initialPosition;
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(value: true);
		}
		AmountKilled = 0;
	}

	public int GetAliveInvaders()
	{
		int num = 0;
		foreach (Transform item in base.transform)
		{
			if (item.gameObject.activeInHierarchy)
			{
				num++;
			}
		}
		return num;
	}
}
