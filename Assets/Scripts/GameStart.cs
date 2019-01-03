using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {

    AudioSource se;
    private void Start()
    {
        Screen.SetResolution(396, 704, false, 60);
        se = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {

        if(FadeManager.Instance.GetIsFading() == false)
            if (Input.anyKeyDown)
            {
                se.Play();
                FadeManager.Instance.LoadScene("Main", 1f);
            }
    }
}
