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
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mInput = GetComponent<InputController>();
    }
    
    void FixedUpdate()
    {
        Vector2 playerInput = mInput.GetInput();
        if (playerInput.magnitude == 0)
        {
            playerInput = transform.up;
        }

        transform.up = Vector3.RotateTowards(transform.up, playerInput, rotationSpeed*Time.deltaTime, 0.0f);
        rb.velocity = transform.up * speed;
    }

    
}
