using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class PlaceTraps : MonoBehaviour
{
    public float trapLength = 2f;
    public ParticleSystem trap;
    public ParticleSystem trail;
    public ParticleSystem EMP;
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
    void PlaceEMP()
    {
        SoundManager.instance.resetPickUp();
        GameManager.instance.OnPlaceTrap();
        GameObject e = Instantiate(EMP, transform.position, Quaternion.identity).gameObject;
        e.transform.parent = transform;
        e.transform.localScale = new Vector3(1, 1, 1);
    }
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0)) && !placing && SoundManager.instance.CheckMaxed())
        {
            StartCoroutine(PlaceTrap());
        }
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1)) && !placing && SoundManager.instance.CheckMaxed())
        {
            PlaceEMP();
        }
    }

}
