using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    int TrapsLayer;
    public GameObject deathVFXPrefabs;
    
    void Start()
    {
        TrapsLayer = LayerMask.NameToLayer("Traps");
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == TrapsLayer)
        {
            Instantiate(deathVFXPrefabs, transform.position, transform.rotation);//死亡特效

            gameObject.SetActive(false);

            AudioManager.DeathAudio();

            GameManager.PlayerDied();//角色死亡后重载场景

            
        }
    }//碰撞障碍物检测

    

}
