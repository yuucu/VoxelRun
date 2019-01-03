using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Text score;

    [SerializeField]
    private Text dis;
    [SerializeField]
    private Text coin;
    [SerializeField]
    private Text bestScore;


    public void show(float s, int c)
    {

      //  resultPanel.SetActive(true);

        int z = 0;
        if (s > 0)
            z = Mathf.FloorToInt(s);

        int p = z + c / 2;
        /*
        dis.text = "" + z;
        coin.text = "" + c;
        score.text = "" + p;
        bestScore.text = "Best Score : " + "---";
        */
        // resultPanel.GetComponent<Animator>().SetTrigger("Show");

        int preCoin = PlayerPrefs.GetInt("coin", 0);
        int curCoin = preCoin + c;
        PlayerPrefs.SetInt("coin",curCoin);
        PlayerPrefs.Save();


        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(p,z,c);
    }

  
}
