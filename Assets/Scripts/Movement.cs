using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 3;
    public float rotationSpeed = 1;
    

    private Rigidbody2D rb;
    private Animator anim;
    private InputController mInput;
    private bool stopped = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mInput = GetComponent<InputController>();
    }

    public void Stop(float time)
    {
        StartCoroutine(stop(time));
    }
    IEnumerator stop(float time)
    {
        stopped = true;
        yield return new WaitForSeconds(time);
        stopped = false;
    }
    
    void FixedUpdate()
    {
        if (stopped)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        else
        {
            Vector2 playerInput = mInput.GetInput();
            if (playerInput.magnitude == 0)
            {
                playerInput = transform.up;
            }

            transform.up = Vector3.RotateTowards(transform.up, playerInput, rotationSpeed * Time.deltaTime, 0.0f);
            rb.velocity = transform.up * speed;
        }
    }

    
}
