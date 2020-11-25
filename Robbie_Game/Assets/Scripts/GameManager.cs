using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    Fader fader;
    Door door;
    List<OrbCollected> orbs;

    public int orbNum;
    public int deathNum;

    float gameTime;
    bool gameIsOver;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this);

        orbs = new List<OrbCollected>();
    }

    

    private void Update()
    {
        if (gameIsOver)
        {
            gameTime = 0;//游戏结束时间要先清零
            return;
        }
        //instance.orbNum = instance.orbs.Count;//获得当前orb总数，之后会删掉这部分代码
        UIManager.DisplayOrbsCount(instance.orbs.Count);//显示剩余Orb数量

        gameTime += Time.deltaTime;

        UIManager.DisplayGameTime(gameTime);//显示游戏时间

        if (Input.GetKeyDown(KeyCode.R))
        {
            instance.Invoke("RestartGame",0);
        }
        
    }
    public static void PlayerDied()
    {
        instance.fader.FaderOut();

        instance.gameIsOver = true;

        instance.Invoke("RestartGame", 2.0f);

        instance.deathNum++;

        UIManager.DisplayDeathCount(instance.deathNum);//显示人物死亡次数
    }//死亡后有延迟的重新加载场景

    public static void PlayerGetOrbs(OrbCollected orb)
    {
        if (!instance.orbs.Contains(orb))
            return;
        instance.orbs.Remove(orb);
        
    }//获得Orb，从列表中删去当前orb

    public static void Success()
    {
        if (instance.orbs.Count == 0)
            instance.door.OpenDoor();
    }//获得所有orb，开门

    public void RestartGame()
    {
        instance.orbs.Clear();//游戏重载时，在开始游戏前清空列表

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        instance.gameIsOver = false;

       
    }//重载场景

    

    public static void RegisterFader(Fader obj)
    {
        instance.fader = obj;
    }//在Fader脚本中注册

    public static void RegisterOrbs(OrbCollected orb)
    {

        if (instance == null)
            return;
        if (!instance.orbs.Contains(orb))
            instance.orbs.Add(orb);
        
        

    }//在orbCollecter脚本中注册

    public static void RegisterDoor(Door obj)
    {
        instance.door = obj;
    }//在Door脚本中注册

    public static void GameIsOver()
    {
        instance.gameIsOver = true;
        UIManager.DisplayGameOver();

    }

    public static bool returnGameIsOver()
    {
        return instance.gameIsOver;
    }

    
}
