using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputClone : InputController
{
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public override Vector2 GetInput()
    {
        Vector2 playerInput = InputPlayer.inputHistory[index++];
        return playerInput;
    }
}
