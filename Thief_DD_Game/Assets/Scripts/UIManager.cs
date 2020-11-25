using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    static UIManager instance;

    public TextMeshProUGUI orbsText, deathText, timeText, gameOverText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        
        DontDestroyOnLoad(this);
    }

    public static void DisplayOrbsCount(int orbsCount)
    {
        instance.orbsText.text = orbsCount.ToString();
    }//显示Orb的剩余数量，在GameManager中调用

    public static void DisplayDeathCount(int deathCount)
    {
        instance.deathText.text = deathCount.ToString();
    }//显示人物死亡次数，在GameManager中调用

    public static void DisplayGameTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        instance.timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }//显示游戏时间

    public static void DisplayGameOver()
    {     
           instance.gameOverText.enabled = true;
    }//显示游戏结束的界面

    
}
