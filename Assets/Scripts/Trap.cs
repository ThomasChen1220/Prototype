using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Crash c = other.GetComponent<Crash>();
        if (other.gameObject.tag=="Clone" && c != null)
        {
            c.OnCrash();
        }
    }
}
