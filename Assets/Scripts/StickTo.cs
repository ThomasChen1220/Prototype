using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickTo : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
