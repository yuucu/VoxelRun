using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultButtons : MonoBehaviour {


    public GameObject resultPanel;

    public void OnClickTitle()
    {
        if (FadeManager.Instance.GetIsFading() == false)
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            int preCoin = PlayerPrefs.GetInt("coin", 0);
            int curCoin = preCoin + player.getCoin();
            PlayerPrefs.SetInt("coin",curCoin);
            PlayerPrefs.Save();
            GetComponent<AudioSource>().Play();
            FadeManager.Instance.LoadScene("Title", 1f);
        }
    }

    public void OnClickRetry()
    {
        if (FadeManager.Instance.GetIsFading() == false)
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            int preCoin = PlayerPrefs.GetInt("coin", 0);
            int curCoin = preCoin + player.getCoin();
            PlayerPrefs.SetInt("coin",curCoin);
            PlayerPrefs.Save();

            GetComponent<AudioSource>().Play();
            FadeManager.Instance.LoadScene("Main", 1f);
        }
    }

}
