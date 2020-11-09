using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public PowerUps mType = PowerUps.None;
    public float spawnCounter = 10;
    private void DoEffect(PlaceTraps pt)
    {
        if (mType != PowerUps.None)
        {
            pt.SetPowerUp(mType);
        }
        else
        {
            SoundManager.instance.PlayPickedUpSphereSound();
        }
        GameManager.instance.OnPlayerTouchGoal(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DoEffect(collision.gameObject.GetComponent<PlaceTraps>());
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
