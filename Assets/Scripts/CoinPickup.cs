using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    bool wasCollected = false;

    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int coinScoreValue = 100;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player" && !wasCollected) {
            wasCollected = true;
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            FindFirstObjectByType<GameSession>().UpdatePlayerScore(coinScoreValue);
        }
    }
}
