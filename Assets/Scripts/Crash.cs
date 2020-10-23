using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    public GameObject vfx;
    private void Start()
    {
        GameManager.instance.ships.Add(this);
    }
    public void OnCrash()
    {
        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
        if (particle != null)
        {
            particle.transform.SetParent(null, true);
        }

        GameManager.instance.ships.Remove(this);
        if (gameObject.tag == "Player")
        {
            GameManager.instance.OnGameEnd();
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        var g = Instantiate(vfx, transform.position, Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var c = collision.gameObject.GetComponent<Crash>();
        if ( c != null)
        {
            c.OnCrash();
            OnCrash();
        }
    }
}
