using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    bool isWrappingX = false;
    bool isWrappingY = false;
    Renderer mRend;
   
    // Start is called before the first frame update
    void Start()
    {
        mRend = GetComponent<Renderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        WrapScreen();
    }
    bool CheckRenderers()
    {
        if (transform.position.x < -GameManager.screenWidth || transform.position.x > GameManager.screenWidth)
            return false;
        if (transform.position.y < -GameManager.screenHeight || transform.position.y > GameManager.screenHeight)
            return false;

        return true;
    }
    void WrapScreen()
    {
        var isVisible = CheckRenderers();
        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY)
        {
            return;
        }

        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(transform.position);
        var newPosition = transform.position;

        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            isWrappingX = true;
        }

        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        transform.position = newPosition;
    }
}
