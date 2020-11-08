using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public float spawnCounter = 16;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.OnPlayerTouchGoal(gameObject);
            SoundManager.instance.PlayPickedUpSphereSound();
        }
    }
    public void Blink() {
        GetComponent<Animator>().SetBool("Blink", true);
    }
    private void Update()
    {
            spawnCounter -= Time.deltaTime;
            if (spawnCounter < 0)
            {
                GameManager.instance.OnPlayerMissedGoal(gameObject);
            }
            if( spawnCounter<=5 )
            {
                Blink();
            }
    }
}
