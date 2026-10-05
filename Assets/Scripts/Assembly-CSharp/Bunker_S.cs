using TMPro;
using UnityEngine;

public class Bunker_S : MonoBehaviour
{
	public Texture2D splat;

	private Texture2D originalTexture;

	private SpriteRenderer spriteRenderer;

	private BoxCollider2D boxCollider;

	public TMP_Text bunkerRepairText;

	private bool launchBunker;

	public int bunkerHealth = 10;

	public Vector3 direction;

	private float speed = 10f;

	public float initialXPosition;

	public float initialYPosition;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		boxCollider = GetComponent<BoxCollider2D>();
		originalTexture = spriteRenderer.sprite.texture;
		ResetBunker();
	}

	public void ResetBunker()
	{
		CopyTexture(originalTexture);
		base.gameObject.SetActive(value: true);
		bunkerHealth = 10;
		base.transform.position = new Vector3(initialXPosition, initialYPosition, base.transform.position.z);
	}

	private void CopyTexture(Texture2D source)
	{
		Texture2D texture2D = new Texture2D(source.width, source.height, source.format, mipChain: false);
		texture2D.filterMode = source.filterMode;
		texture2D.anisoLevel = source.anisoLevel;
		texture2D.wrapMode = source.wrapMode;
		texture2D.SetPixels32(source.GetPixels32());
		texture2D.Apply();
		Sprite sprite = Sprite.Create(texture2D, spriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), spriteRenderer.sprite.pixelsPerUnit);
		spriteRenderer.sprite = sprite;
	}

	public bool CheckCollision(BoxCollider2D other, Vector3 hitPoint)
	{
		Vector2 vector = other.size / 2f;
		if (!Splat(hitPoint) && !Splat(hitPoint + Vector3.down * vector.y) && !Splat(hitPoint + Vector3.up * vector.y) && !Splat(hitPoint + Vector3.left * vector.x))
		{
			return Splat(hitPoint + Vector3.right * vector.x);
		}
		return true;
	}

	private bool Splat(Vector3 hitPoint)
	{
		if (!CheckPoint(hitPoint, out var px, out var py))
		{
			return false;
		}
		Texture2D texture = spriteRenderer.sprite.texture;
		px -= splat.width / 2;
		py -= splat.height / 2;
		int num = px;
		for (int i = 0; i < splat.height; i++)
		{
			px = num;
			for (int j = 0; j < splat.width; j++)
			{
				Color pixel = texture.GetPixel(px, py);
				pixel.a *= splat.GetPixel(j, i).a;
				texture.SetPixel(px, py, pixel);
				px++;
			}
			py++;
		}
		texture.Apply();
		return true;
	}

	private bool CheckPoint(Vector3 hitPoint, out int px, out int py)
	{
		Vector3 vector = base.transform.InverseTransformPoint(hitPoint);
		vector.x += boxCollider.size.x / 2f;
		vector.y += boxCollider.size.y / 2f;
		Texture2D texture = spriteRenderer.sprite.texture;
		px = (int)(vector.x / boxCollider.size.x * (float)texture.width);
		py = (int)(vector.y / boxCollider.size.y * (float)texture.height);
		return texture.GetPixel(px, py).a != 0f;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Invader"))
		{
			if (launchBunker)
			{
				other.gameObject.SetActive(value: false);
			}
			else
			{
				bunkerHealth = 0;
				CheckBunkerHealth();
			}
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Missile"))
		{
			Projectile_S component = other.GetComponent<Projectile_S>();
			if (CheckCollision(other as BoxCollider2D, component.transform.position))
			{
				other.gameObject.SetActive(value: false);
				CheckBunkerHealth();
			}
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
		{
			Projectile_S component2 = other.GetComponent<Projectile_S>();
			if (component2.chargedShot)
			{
				launchBunker = true;
				other.gameObject.SetActive(value: false);
				if (component2.destroyed != null)
				{
					component2.destroyed();
				}
			}
			else if (CheckCollision(other as BoxCollider2D, component2.transform.position))
			{
				other.gameObject.SetActive(value: false);
				CheckBunkerHealth();
			}
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
		{
			launchBunker = false;
			bunkerHealth = 0;
			CheckBunkerHealth();
		}
	}

	private void CheckBunkerHealth()
	{
		bunkerHealth--;
		if (bunkerHealth <= 0)
		{
			base.gameObject.SetActive(value: false);
			bunkerRepairText.gameObject.SetActive(value: true);
		}
	}

	private void Update()
	{
		if (bunkerHealth <= 0)
		{
			bunkerRepairText.gameObject.SetActive(value: false);
		}
		if (launchBunker)
		{
			base.transform.position += direction * speed * Time.deltaTime;
		}
	}
}
