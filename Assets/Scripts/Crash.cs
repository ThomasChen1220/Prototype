using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    public GameObject vfx;
    public void OnCrash()
    {
        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
        if (particle != null)
        {
            particle.transform.SetParent(null, true);
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
