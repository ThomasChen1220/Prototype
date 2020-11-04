using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEffect : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void PointerEnter()
    {
        anim.SetBool("Hover", true);
        SoundManager.instance.PlayMouseHoverSound();
    }

    public void PointerExit()
    {
        anim.SetBool("Hover", false);
    }
}