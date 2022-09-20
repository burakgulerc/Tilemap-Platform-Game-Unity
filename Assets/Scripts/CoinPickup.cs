using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSound;
    [SerializeField] int coinScoreValue;
    
    public int scoreCount = 0;
    // Prevent double collect for the same pickup
    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !wasCollected)
        {
            wasCollected = true;

            FindObjectOfType<GameSession>().UpdateScore(coinScoreValue);

            AudioSource.PlayClipAtPoint(coinPickupSound,Camera.main.transform.position);

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
