using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rig;
    PlayerController pc;

    //动画参数ID
    int speedID;
    int verticalVelocityID;
    //int isJumpID;
    int isOnGroundID;
    int isCrouchID;
    int isHangingID;
   
    void Start()
    {
        anim = GetComponent<Animator>();
        pc = GetComponentInParent<PlayerController>();
        rig = GetComponentInParent<Rigidbody2D>();


        speedID = Animator.StringToHash("speed");
        verticalVelocityID = Animator.StringToHash("verticalVelocity");
        //isJumpID = Animator.StringToHash("isJump");
        isOnGroundID = Animator.StringToHash("isOnGround");
        isCrouchID = Animator.StringToHash("isCrouch");
        isHangingID = Animator.StringToHash("isHanging");
    }

    
    void Update()
    {
        if (GameManager.returnGameIsOver())
        {
            anim.SetFloat(speedID, 0);
            return;
        }//GameOver，人物保持静止
        anim.SetFloat(speedID, Mathf.Abs(pc.xVelocity));//控制Grounded的动画

        anim.SetFloat(verticalVelocityID, rig.velocity.y);//控制MidAir的动画

        anim.SetBool(isCrouchID, pc.isCrouch);//控制Crouch的动画

        anim.SetBool(isHangingID, pc.isHanging);//控制Hanging的动画

        //anim.SetBool(isJumpID, pc.isJump);//判断Jump

        anim.SetBool(isOnGroundID, pc.isOnGround);//判断isOnGround


    }
    public void PlayWalkStepAudio()
    {
        AudioManager.WalkStepAudio();
    }//人物移动音效播放

    public void PlayCrouchStepAudio()
    {
        AudioManager.CrouchStepAudio();
    }//人物蹲下移动音效播放
}
