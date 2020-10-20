using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : InputController
{
    public float spawnIntervel = 2f;
    public GameObject clone;
    public static List<Vector2> inputHistory;

    private Vector3 startPos;
    private float lastSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        inputHistory = new List<Vector2>();
        startPos = transform.position;
        lastSpawn = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if (lastSpawn + spawnIntervel < Time.time)
        {
            GameObject c = Instantiate(clone, startPos, Quaternion.identity);
            lastSpawn = Time.time;
        }
    }
    public override Vector2 GetInput() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 playerInput = new Vector2(x, y);
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        inputHistory.Add(playerInput);
        return playerInput;
    }
}
