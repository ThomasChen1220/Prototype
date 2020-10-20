using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 3;
    public GameObject clone;

    private Rigidbody2D rb;
    private Animator anim;
    private List<Vector2> inputHistory;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputHistory = new List<Vector2>();
        startPos = transform.position;
        //anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject c = Instantiate(clone);
            c.transform.position = startPos;
            c.GetComponent<Clone>().SetInput(inputHistory);
            startPos = transform.position;
            inputHistory.Clear();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 playerInput = new Vector2(x, y);
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        rb.velocity = playerInput * speed;
        inputHistory.Add(playerInput);
    }
}
