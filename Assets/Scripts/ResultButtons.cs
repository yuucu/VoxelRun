using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultButtons : MonoBehaviour {


    public GameObject resultPanel;

    public void OnClickTitle()
    {
        if (FadeManager.Instance.GetIsFading() == false)
        {
            GetComponent<AudioSource>().Play();
            FadeManager.Instance.LoadScene("Title", 1f);
        }
    }

    public void OnClickRetry()
    {
        if (FadeManager.Instance.GetIsFading() == false)
        {
            GetComponent<AudioSource>().Play();
            FadeManager.Instance.LoadScene("Main", 1f);
        }
    }

}
