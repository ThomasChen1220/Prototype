using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.OnPlayerTouchGoal();
            SoundManager.instance.pickedUpSphereSound();
            Destroy(gameObject);
        }
    }
}
