using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    int playerLayer;

    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == playerLayer)
        {
            GameManager.GameIsOver();

            AudioManager.WinAudio();
        }
       
    }//win zone开门后的过关检测

}
