using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get direction you want to point at
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1,1,1);
            // set vector of transform directly
            transform.right = direction;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            // set vector of transform directly
            transform.right = -direction;
        }
    }
}
