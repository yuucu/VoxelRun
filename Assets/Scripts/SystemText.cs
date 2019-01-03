using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemText : MonoBehaviour
{

    private Player player;

    [SerializeField]
    Text score;
    [SerializeField]
    Text coin;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.getIsAliving())
        {
            int z = 0;
            float playerZ = player.getPos().z;
            if (playerZ > 0)
                z = Mathf.FloorToInt(playerZ);

            score.text = "Distance : " + z + " m";
            coin.text = "Coin   : " + player.getCoin() + " G";
        }
        else
        {
            score.text = "";
            coin.text = "";
        }
    }

}
