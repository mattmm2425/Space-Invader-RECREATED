using UnityEngine;

public class StartScreen_S : MonoBehaviour
{
	public GameObject startScreenUI;

	public GameObject gameUI;

	public GameObject startGameText;

	private void Start()
	{
		InvokeRepeating("BlinkStartGameText", 0.5f, 0.5f);
	}

	private void Update()
	{
		if (Input.anyKeyDown)
		{
			startScreenUI.SetActive(value: false);
			gameUI.SetActive(value: true);
			CancelInvoke("BlinkStartGameText");
			startGameText.SetActive(value: false);
		}
	}

	private void BlinkStartGameText()
	{
		if (startGameText.activeSelf)
		{
			startGameText.SetActive(value: false);
		}
		else
		{
			startGameText.SetActive(value: true);
		}
	}
}
