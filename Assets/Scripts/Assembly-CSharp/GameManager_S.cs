using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_S : MonoBehaviour
{
	[SerializeField]
	private GameObject newGameUI;

	[SerializeField]
	private GameObject HUD;

	[SerializeField]
	private GameObject gameOverUI;

	[SerializeField]
	private TMP_Text scoreText;

	[SerializeField]
	private GameObject startGameText;

	[SerializeField]
	private TMP_Text highScoreText;

	[SerializeField]
	private TMP_Text finalScoreText;

	[SerializeField]
	private TMP_Text finalHighScoreText;

	[SerializeField]
	private TMP_Text bunkerRepairText;

	[SerializeField]
	private Image ThreeLives;

	[SerializeField]
	private Image TwoLives;

	[SerializeField]
	private Image OneLive;

	private Player_S player;

	private Invaders_S invaders;

	private MysteryShip_S mysteryShip;

	public Bunker_S[] bunkers;

	public int score;

	public static int highScore;

	public static GameManager_S Instance { get; private set; }

	public int lives { get; private set; } = 3;

	private void Awake()
	{
		if (Instance != null)
		{
			Object.DestroyImmediate(base.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void Start()
	{
		player = Object.FindAnyObjectByType<Player_S>();
		invaders = Object.FindAnyObjectByType<Invaders_S>();
		mysteryShip = Object.FindAnyObjectByType<MysteryShip_S>();
		InvokeRepeating("BlinkStartGameText", 0.5f, 0.5f);
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

	private void Update()
	{
		if (gameOverUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
		{
			NewGame();
			Respawn();
		}
		if (newGameUI.activeSelf && Input.anyKeyDown)
		{
			newGameUI.SetActive(value: false);
			HUD.SetActive(value: true);
			CancelInvoke("BlinkStartGameText");
			startGameText.SetActive(value: false);
			NewGame();
			Respawn();
		}
		if (bunkerRepairText.gameObject.activeSelf && Input.GetKeyDown(KeyCode.R) && score >= 300)
		{
			RepairBunkers();
		}
	}

	private void NewGame()
	{
		score = 0;
		lives = 3;
		gameOverUI.SetActive(value: false);
		HUD.SetActive(value: true);
		player.gameObject.SetActive(value: true);
		NewRound();
		ThreeLives.enabled = true;
		TwoLives.enabled = true;
		OneLive.enabled = true;
		scoreText.text = score.ToString();
		highScoreText.text = highScore.ToString();
		bunkerRepairText.gameObject.SetActive(value: false);
		for (int i = 0; i < bunkers.Length; i++)
		{
			bunkers[i].ResetBunker();
		}
	}

	private void NewRound()
	{
		invaders.ResetInvaders();
		invaders.gameObject.SetActive(value: true);
	}

	private void Respawn()
	{
		Vector3 position = player.transform.position;
		position.x = 0f;
		player.transform.position = position;
		player.gameObject.SetActive(value: true);
	}

	private void GameOver()
	{
		gameOverUI.SetActive(value: true);
		HUD.SetActive(value: false);
		invaders.gameObject.SetActive(value: false);
		player.gameObject.SetActive(value: false);
		finalScoreText.text = score.ToString();
		if (highScore < score)
		{
			highScore = score;
			finalHighScoreText.text = highScore.ToString();
		}
		else
		{
			finalHighScoreText.text = highScore.ToString();
		}
	}

	private void SetScore(int score)
	{
		this.score = score;
		scoreText.text = this.score.ToString();
	}

	private void SetLives(int lives)
	{
		switch (lives)
		{
		case 3:
			ThreeLives.enabled = true;
			TwoLives.enabled = true;
			OneLive.enabled = true;
			break;
		case 2:
			ThreeLives.enabled = false;
			TwoLives.enabled = true;
			OneLive.enabled = true;
			break;
		case 1:
			ThreeLives.enabled = false;
			TwoLives.enabled = false;
			OneLive.enabled = true;
			break;
		default:
			ThreeLives.enabled = false;
			TwoLives.enabled = false;
			OneLive.enabled = false;
			break;
		}
	}

	public void OnPlayerKilled(Player_S player)
	{
		SetLives(--lives);
		player.gameObject.SetActive(value: false);
		if (lives >= 0)
		{
			Invoke("Respawn", 1f);
		}
		else if (lives < 0)
		{
			GameOver();
		}
	}

	public void InvaderKilled(Invader_S invader)
	{
		SetScore(score + invader.score);
		if (invaders.GetAliveInvaders() == 0)
		{
			NewRound();
		}
	}

	public void MysteryShipKilled(MysteryShip_S mysteryShip)
	{
		SetScore(score + mysteryShip.score);
	}

	private void RepairBunkers()
	{
		score -= 300;
		bunkerRepairText.gameObject.SetActive(value: false);
		for (int i = 0; i < bunkers.Length; i++)
		{
			bunkers[i].ResetBunker();
		}
		scoreText.text = score.ToString();
	}
}
