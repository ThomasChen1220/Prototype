using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTraps : MonoBehaviour
{
    public ParticleSystem trap;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            trap.Play();
        }
        if(Input.GetMouseButtonUp(0))
        {
            trap.Stop();
        }
    }
}
