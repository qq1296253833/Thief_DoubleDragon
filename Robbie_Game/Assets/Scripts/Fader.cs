using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    Animator anim;

    int fadeID;
    void Start()
    {
        anim = GetComponent<Animator>();
        fadeID = Animator.StringToHash("Fade");

        GameManager.RegisterFader(this);
    }

   public void FaderOut()
    {
        anim.SetTrigger(fadeID);
    }
}
