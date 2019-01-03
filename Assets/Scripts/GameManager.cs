using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);


    }

    private Player player;

    public void setPlayer(Player player)
    {
        this.player = player;
    }

    public void gameStart()
    {
        player.gameStart();
        EnemyManager.Instance.gameStart();
    }

    public void gameStop()
    {
        EnemyManager.Instance.gameStop();
    }
}
