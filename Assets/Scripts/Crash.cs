﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    public GameObject vfx;
    public bool invinciable = false;
    public GameObject mSheild;
    private void Start()
    {
        if(gameObject.tag!="Player")
            GameManager.instance.AddShip(this);
    }
    public void GotSheild(GameObject s) {
        if (mSheild != null)
        {
            Destroy(mSheild);
        }
        mSheild = s;
    }
    public void OnCrash()
    {
        if (invinciable) return;
        if (mSheild != null)
        {
            Destroy(mSheild);
            return;
        }
        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
        if (particle != null)
        {
            particle.transform.SetParent(null, true);
            particle.transform.localScale = new Vector3(1, 1, 1);
        }

        GameManager.instance.RemoveShip(this);
        if (gameObject.tag == "Player")
        {
            GameManager.instance.OnGameEnd();
        }
        SoundManager.instance.PlayCrashedSound();
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        var g = Instantiate(vfx, transform.position, Quaternion.identity);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var c = collision.gameObject.GetComponent<Crash>();
        if (c != null)
        {
            c.OnCrash();
            OnCrash();
        }
    }
}
