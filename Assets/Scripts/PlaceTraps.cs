using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum PowerUps { None, Traps, Electric, Laser, Sheild }
public class PlaceTraps : MonoBehaviour
{
    
    public PowerUps currentPowerUp;
    public float trapLength = 2f;
    public ParticleSystem trap;
    public ParticleSystem trail;
    public GameObject EMP;
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
        ///SoundManager.instance.resetPickUp();
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
    void PlaceEMP()
    {
       // SoundManager.instance.resetPickUp();
        GameManager.instance.OnPlaceTrap();
        GameObject e = Instantiate(EMP, transform.position, Quaternion.identity);
        e.GetComponent<StickTo>().target = transform;
    }
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !placing && SoundManager.instance.CheckMaxed())
        {
            if (currentPowerUp == PowerUps.Traps)
            {
                StartCoroutine(PlaceTrap());
            }
            if(currentPowerUp == PowerUps.Electric)
            {
                PlaceEMP();
            }
        }
    }

}
