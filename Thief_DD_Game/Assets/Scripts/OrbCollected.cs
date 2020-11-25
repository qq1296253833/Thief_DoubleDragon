using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollected : MonoBehaviour
{
    int playerLayer;
    public GameObject explosionVFXPrefabs;

    
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        
        GameManager.RegisterOrbs(this);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == playerLayer)
        {
            AudioManager.CollectCoinAudio();
            
            Instantiate(explosionVFXPrefabs, transform.position, transform.rotation);
            
            gameObject.SetActive(false);

            GameManager.PlayerGetOrbs(this);//获得Orb

            GameManager.Success();//当收集完所有的orb，门打开
        }
    }//收集Orb的碰撞检测

}
