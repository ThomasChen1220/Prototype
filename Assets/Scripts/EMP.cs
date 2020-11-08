using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : MonoBehaviour
{
    //public float shockTime = 0.5f;
    private void OnParticleCollision(GameObject other)
    {
        //Movement m = other.GetComponent<Movement>();
        //if (other.gameObject.tag == "Clone" && m != null)
        //{
        //    m.Stop(shockTime);
        //}
        Crash c = other.GetComponent<Crash>();
        if (other.gameObject.tag == "Clone" && c != null)
        {
            c.OnCrash();
        }
    }
}
