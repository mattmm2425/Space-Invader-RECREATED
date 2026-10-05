using System;
using UnityEngine;

public class Player_S : MonoBehaviour
{
	public Projectile_S laserPrefab;

	public Projectile_S chargedshotPrefab;

	public ScreenShake_S camShake;

	private float ScreenRight;

	private float ScreenLeft;

	public float Speed = 5f;

	private bool _LaserActive;

	[Range(0f, 5f)]
	public float chargingShot;

	public float chargedShot = 5f;

	private bool isCharging;

	private void Start()
	{
		Camera main = Camera.main;
		ScreenRight = main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x;
		ScreenLeft = main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			base.transform.position += Vector3.right * Speed * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			base.transform.position += Vector3.left * Speed * Time.deltaTime;
		}
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
		{
			Shoot();
		}
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
		{
			chargingShot = 0f;
			if (!isCharging)
			{
				InvokeRepeating("ChargingShot", 1f, 1f);
				isCharging = true;
			}
		}
		if ((Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)) && chargingShot >= chargedShot)
		{
			ShootChargedLaser();
			chargingShot = 0f;
		}
		if (base.transform.position.x > ScreenRight)
		{
			base.transform.position = new Vector3(ScreenLeft, base.transform.position.y, base.transform.position.z);
		}
		if (base.transform.position.x < ScreenLeft)
		{
			base.transform.position = new Vector3(ScreenRight, base.transform.position.y, base.transform.position.z);
		}
		chargingShot = Mathf.Clamp(chargingShot, 0f, chargedShot);
	}

	private void Shoot()
	{
		if (!_LaserActive)
		{
			Projectile_S projectile_S = UnityEngine.Object.Instantiate(laserPrefab, base.transform.position, Quaternion.identity);
			projectile_S.destroyed = (Action)Delegate.Combine(projectile_S.destroyed, new Action(LaserDestroyed));
			_LaserActive = true;
		}
	}

	private void ShootChargedLaser()
	{
		if (!_LaserActive)
		{
			Projectile_S projectile_S = UnityEngine.Object.Instantiate(chargedshotPrefab, base.transform.position, Quaternion.identity);
			projectile_S.destroyed = (Action)Delegate.Combine(projectile_S.destroyed, new Action(LaserDestroyed));
			_LaserActive = true;
		}
	}

	private void ChargingShot()
	{
		if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
		{
			CancelInvoke("ChargingShot");
			isCharging = false;
			chargingShot = 0f;
		}
		else if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
		{
			chargingShot++;
		}
		else
		{
			CancelInvoke("ChargingShot");
			isCharging = false;
			chargingShot = 0f;
		}
	}

	private void LaserDestroyed()
	{
		_LaserActive = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Invader") || other.gameObject.layer == LayerMask.NameToLayer("Missile"))
		{
			camShake.ResetShaker();
			GameManager_S.Instance.OnPlayerKilled(this);
		}
	}
}
