using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class PlaceTraps : MonoBehaviour
{
    public float trapLength = 2f;
    public ParticleSystem trap;
    public ParticleSystem trail;
    public Light2D readyLight;

    bool placing = false;

    public void TurnOnTrail()
    {
        trail.Play();
        readyLight.enabled = true;
    }
    public void TurnOffTrail()
    {
        trail.Stop();
        readyLight.enabled = false;

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
        gameObject.GetComponent<Crash>().invinciable = true;
        yield return new WaitForSeconds(GetDuration());
        placing = false;
        gameObject.GetComponent<Crash>().invinciable = false;

        trap.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0)) && !placing && SoundManager.instance.CheckMaxed())
        {
            StartCoroutine(PlaceTrap());
        }
    }

}
