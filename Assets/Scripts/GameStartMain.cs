using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartMain : MonoBehaviour {

    public GameObject gameFirstPanel;
    private AudioSource se;

	void Start()
    {
        se = GetComponent<AudioSource>();
    }

	void Update () {
        if(gameFirstPanel.active == true)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                se.Play();
                gameFirstPanel.SetActive(false);
                GameManager.Instance.gameStart();
            }
    }
}
