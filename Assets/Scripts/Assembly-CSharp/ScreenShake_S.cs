using UnityEngine;

public class ScreenShake_S : MonoBehaviour
{
	private Vector3 OriginPos;

	[SerializeField]
	private float ShakeDuration;

	[SerializeField]
	private float ShakeAmount;

	[SerializeField]
	private float Soften;

	private void Start()
	{
		OriginPos = base.transform.localPosition;
	}

	private void Update()
	{
		if (ShakeDuration > 0f)
		{
			base.transform.localPosition = OriginPos + Random.insideUnitSphere * ShakeAmount;
			ShakeDuration -= Time.deltaTime * Soften;
		}
		else
		{
			ShakeDuration = 0f;
			base.transform.localPosition = OriginPos;
		}
	}

	public void ResetShaker()
	{
		ShakeDuration = 1f;
	}
}
