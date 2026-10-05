using UnityEngine;

public class ChargingShotUI_S : MonoBehaviour
{
	public Player_S player;

	public float Width;

	public float Height;

	[SerializeField]
	private RectTransform chargingShotBar;

	private void Start()
	{
	}

	private void Update()
	{
		SetCharge(player);
	}

	public void SetChargedShot(Player_S player)
	{
	}

	public void SetCharge(Player_S player)
	{
		float x = player.chargingShot / player.chargedShot * Width;
		chargingShotBar.sizeDelta = new Vector2(x, Height);
	}
}
