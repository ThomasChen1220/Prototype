using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTraps : MonoBehaviour
{
    public float trapLength = 2f;
    public ParticleSystem trap;
    public ParticleSystem trail;

    bool placing = false;

    public void TurnOnTrail()
    {
        trail.Play();
    }
    public void TurnOffTrail()
    {
        trail.Stop();
    }
    private float GetDuration() {
        return trapLength / GetComponent<Movement>().speed;
    }
    private IEnumerator PlaceTrap() {
        SoundManager.instance.resetPickUp();
        SoundManager.instance.PlayTrapSound();
        GameManager.instance.OnPlaceTrap();
        placing = true;
        trap.Play();
        yield return new WaitForSeconds(GetDuration());
        placing = false;
        trap.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !placing && SoundManager.instance.CheckMaxed())
        {
            StartCoroutine(PlaceTrap());
        }
    }

}
