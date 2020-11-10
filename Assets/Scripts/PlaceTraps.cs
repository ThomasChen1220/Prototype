using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum PowerUps { None, Traps, Electric, Laser, Sheild }
public class PlaceTraps : MonoBehaviour
{
    public PowerUpData[] pLib;
    public PowerUps currentPowerUp;
    public float trapLength = 2f;
    public ParticleSystem trap;
    public GameObject EMP;
    public GameObject laser;
    public GameObject currEffect; //reference to current ship effect
    public Transform effectHolder;//parent of the current effect

    bool placing = false;
    public void SetPowerUp(PowerUps p)
    {
        //clear current powerup and visual effects
        TurnOffEffect();
        
        //change state, vfx, play sound
        currentPowerUp = p;
        for(int i = 0; i < pLib.Length; i++)
        {
            if (pLib[i].mType == p)
            {
                SoundManager.instance.PlayPowerUpSound();
                if (p == PowerUps.Sheild)
                {
                    GameObject s = Instantiate(pLib[i].mShipEffect, transform);
                    s.transform.localPosition = Vector3.zero;
                    s.transform.localRotation = Quaternion.identity;
                    s.transform.localScale = new Vector3(1, 1, 1);
                    GetComponent<Crash>().GotSheild(s);
                }
                else
                {
                    //I added "!" in if statement 
                    //Now everytime player picks up power up they will know how to use it
                    if (!GameManager.instance.gameStatsSave.tutorialShown == false)
                    {
                        GameManager.instance.popUpText.SetActive(true);
                    }

                    currEffect = Instantiate(pLib[i].mShipEffect, effectHolder);
                    currEffect.transform.localPosition = Vector3.zero;
                    currEffect.transform.localRotation = Quaternion.identity;
                    currEffect.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

    //TODO: move this part to player
    //public void OnTrapReady() {
    //    currPlayer.GetComponent<PlaceTraps>().TurnOnTrail();
    //    if (gameStatsSave.tutorialShown == false)
    //    {
    //        popUpText.SetActive(true);
    //    }
    //}
    public PowerUps TurnOffEffect()
    {
        PowerUps e = currentPowerUp;
        currentPowerUp = PowerUps.None;
        Destroy(currEffect);
        return e;
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
    void PlaceLaser()
    {
        // SoundManager.instance.resetPickUp();
        GameManager.instance.OnPlaceTrap();
        GameObject e = Instantiate(laser, transform);
        e.transform.localPosition = Vector3.zero;
        e.transform.localRotation = Quaternion.identity;
        e.transform.localScale = new Vector3(1, 1, 1);
    }
    void TryPlayEffect()
    {
        if (currentPowerUp == PowerUps.None)
            return;
        PowerUps e = TurnOffEffect();
        if (e == PowerUps.Traps)
        {
            StartCoroutine(PlaceTrap());
        }
        if (e == PowerUps.Electric)
        {
            PlaceEMP();
        }
        if (e == PowerUps.Laser)
        {
            PlaceLaser();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !placing)
        {
            TryPlayEffect();
        }
    }

}
