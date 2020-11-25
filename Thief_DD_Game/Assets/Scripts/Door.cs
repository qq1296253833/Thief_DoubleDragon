using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;

    int openID;
    void Start()
    {
        anim = GetComponent<Animator>();

        openID = Animator.StringToHash("Open");

        GameManager.RegisterDoor(this);
    }

    
    public void OpenDoor()
    {
        anim.SetTrigger(openID);

        AudioManager.OpenDoorAudio();
    }//控制门open和closed
}
