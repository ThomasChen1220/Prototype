using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    public float speed = 3;

    private List<Vector2> input;
    private int index = 0;
    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        input = new List<Vector2>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetInput(List<Vector2> l)
    {

        foreach(Vector2 v in l)
        {
            input.Add(v);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (index < input.Count)
        {
            Vector2 playerInput = input[index];
            playerInput = Vector2.ClampMagnitude(playerInput, 1f);

            rb.velocity = playerInput * speed;
            index++;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
