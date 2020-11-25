using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rig;
    private BoxCollider2D coll;

    [Header("移动参数")]
    public float speed = 8f;//人物移动速度；
    public float crouchSpeedDivisor = 3f;//人物蹲下后速度的除数

    [Header("跳跃参数")]
    public float jumpForce = 6.3f; 
    public float jumpHeldForce = 1.9f;
    public float jumpHeldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;
    public float hangJumpForce = 16.0f;

    float jumpTime;

    [Header("状态")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadBlocked;
    public bool isHanging;

    [Header("环境检测")]
    public float footOffset = 0.5f;
    public float headClearance = 0.5f;
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;
    float playerHeight;
    public float eyeHeight = 1.5f;
    public float grabDistance = 0.4f;
    public float reachOffset = 0.6f;


    public float xVelocity;//获得键盘水平方向输入值
    public bool lockKeyBoard;//false
    

    //碰撞体尺寸
    Vector2 colliderStandsize;
    Vector2 colliderStandoffset;
    Vector2 colliderCrouchsize;
    Vector2 colliderCrouchoffset;

    //按键设置
    bool jumpPressed;
    bool jumpHeld;
    bool crouchHeld;
    bool crouchPressed;
    


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        playerHeight = coll.size.y;
        //获得碰撞体尺寸以及设置蹲下时碰撞体尺寸
        colliderStandsize = coll.size;
        colliderStandoffset = coll.offset;
        colliderCrouchsize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchoffset = new Vector2(coll.offset.x, coll.offset.y / 2f);
    }

    
    void Update()
    {
        if (GameManager.returnGameIsOver())
            lockKeyBoard = true;
        if (lockKeyBoard == false)
        {
            jumpPressed = Input.GetButtonDown("Jump");
            jumpHeld = Input.GetButton("Jump");
            crouchHeld = Input.GetButton("Crouch");
            crouchPressed = Input.GetButtonDown("Crouch");
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.returnGameIsOver())
            lockKeyBoard = true;
        if (lockKeyBoard == false)
        {
            PhysicsCheck();
            GroundMovement();
            MidAirMovement();
        }//游戏结束时锁掉键盘输入
        else
        {           
            rig.velocity = new Vector2(0, 0);//游戏结束让人物静止
        }
        
    }

    void PhysicsCheck()
    {
        //脚部检测
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0.05f), Vector2.down, groundDistance, groundLayer);//人物左脚检测地面
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0.05f), Vector2.down, groundDistance, groundLayer);//人物右脚检测地面
        
        if ((leftCheck || rightCheck) && !isHanging)
            isOnGround = true;
        else isOnGround = false;
        
        //头顶检测
        RaycastHit2D headCheck = Raycast(new Vector2(0, coll.size.y), Vector2.up, headClearance, groundLayer);//头顶碰撞检测
        
        isHeadBlocked = headCheck;

        //悬挂检测
        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0);//射线发射方向

        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance, groundLayer);//人物头顶射线       
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer);//人物眼睛射线
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance,groundLayer);//人物壁挂检测射线
        
        if(!isOnGround && rig.velocity.y < 0f && !blockedCheck && wallCheck && ledgeCheck)
        {
            Vector2 pos = transform.position;
            pos.x += (wallCheck.distance-0.05f) * direction;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;//悬挂时固定悬挂的位置,让其更加贴合墙壁

            isHanging = true;            
            rig.bodyType = RigidbodyType2D.Static;//悬挂时静止            
        }
    }//射线检测
    void GroundMovement()
    {
        if (isHanging)//悬挂状态下不能进行方向移动
            return;
        if (crouchHeld && isOnGround && !isCrouch)//按下蹲键
            Crouch();
        else if (!crouchHeld && isCrouch && !isHeadBlocked)//松开下蹲键
            StandUp();
        else if (!isOnGround && isCrouch)//在空中仍是下蹲状态时，碰撞体不为Crouch时一般的状态
            StandUp();

        
        xVelocity = Input.GetAxis("Horizontal");//-1 1的值

        if (isCrouch)
            xVelocity /= crouchSpeedDivisor;
        
        rig.velocity = new Vector2(xVelocity * speed, rig.velocity.y);//控制人物水平移动

        FaceDirection();//控制人物朝向
    }//地面的运动

    void MidAirMovement()
    {
        if (isHanging)
        {
            if (jumpPressed)
            {
                rig.bodyType = RigidbodyType2D.Dynamic;
                rig.velocity = new Vector2(rig.velocity.x, hangJumpForce);
                isHanging = false;              
            }
        }//悬挂时按下跳跃键可以跳上平台
        if(isHanging)
        {
            if(crouchPressed)
            {
                rig.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }
        }//悬挂时按下下蹲可以跳出悬挂状态下落
        if (jumpPressed && isOnGround && !isJump)
        {
            if(isCrouch && isOnGround && !isHeadBlocked)
            {
                StandUp();
                rig.AddForce(new Vector2(0, crouchJumpBoost), ForceMode2D.Impulse);
            }
            isOnGround = false;
            isJump = true;

            jumpTime = Time.time + jumpHeldDuration; //跳跃持续时间为0.1f

            rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            AudioManager.JumpAudio();//播放跳跃音效
        }//下蹲的蓄力跳跃，再次增加一段力使其跳的更高
        else if(isJump)
        {
            if(jumpHeld)
            rig.AddForce(new Vector2(0, jumpHeldForce), ForceMode2D.Impulse);
            if (jumpTime < Time.time) //跳跃时间超过时，不能跳跃，人物落下
                isJump = false;
        }//长按跳跃键跳的更高
        
    }//空中的运动
    void FaceDirection()
    {
        if (xVelocity > 0)
            transform.localScale = new Vector3(1, 1, 1);//控制人物朝向右
        else if (xVelocity < 0)
            transform.localScale = new Vector3(-1, 1, 1);//控制人物朝向左
    }//人物面朝方向
    
    void Crouch()
    {
        isCrouch = true;
        coll.size = colliderCrouchsize;
        coll.offset = colliderCrouchoffset;
    }//人物下蹲
    void StandUp()
    {
        isCrouch = false;
        coll.size = colliderStandsize;
        coll.offset = colliderStandoffset;
    }//人物站立

    RaycastHit2D Raycast(Vector2 offset, Vector2 dir, float distance, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, dir, distance, layer);

        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, dir * distance, color);
        
        return hit;
    }//重载射线检测函数
}
