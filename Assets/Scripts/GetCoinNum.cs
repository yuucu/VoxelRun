using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetCoinNum : MonoBehaviour
{

    public GameObject coin;
    private TextMesh coinText;
    public GameObject coinObj1;
    public GameObject coinObj2;
    public GameObject coinObj3;



    // Use this for initialization
    void Start()
    {
        coinText = coin.GetComponent<TextMesh>();
        int coinNum = PlayerPrefs.GetInt("coin", 0);
        coinText.text = coinNum.ToString();

        StartCoroutine("coinCreate", coinNum);
    }

    IEnumerator coinCreate(int num)
    {
        if (num < 500)
            for (int i = 0; i < num; i++)
            {
                Instantiate(coinObj1, transform.position + new Vector3(Random.value * 0.1f, Random.value * 0.4f, Random.value * 0.1f), new Quaternion(Random.value, Random.value, Random.value, Random.value), transform);
                if (i % 30 == 0)
                    yield return null;
            }
        else if(num < 5000)
            for (int i = 0; i < num; i+=30)
            {
                Instantiate(coinObj2, transform.position + new Vector3(Random.value * 0.1f, Random.value * 0.4f, Random.value * 0.1f), new Quaternion(Random.value, Random.value, Random.value, Random.value), transform);
                if (i % 150 == 0)
                    yield return null;
            }
        else if(num < 10000)
            for (int i = 0; i < num; i += 100)
            {
                Instantiate(coinObj3, transform.position + new Vector3(Random.value * 0.1f, Random.value * 0.4f, Random.value * 0.1f), new Quaternion(Random.value, Random.value, Random.value, Random.value), transform);
                if (i % 150 == 0)
                    yield return null;
            }
        else
            for (int i = 0; i < num; i += 100)
            {
                float n = Random.value;

                if(n<0.3f)
                    Instantiate(coinObj1, transform.position + new Vector3(Random.value * 0.1f, Random.value * 0.4f, Random.value * 0.1f), new Quaternion(Random.value, Random.value, Random.value, Random.value), transform);
                else if(n<0.7f)
                    Instantiate(coinObj2, transform.position + new Vector3(Random.value * 0.1f, Random.value * 0.4f, Random.value * 0.1f), new Quaternion(Random.value, Random.value, Random.value, Random.value), transform);
                else
                    Instantiate(coinObj3, transform.position + new Vector3(Random.value * 0.1f, Random.value * 0.4f, Random.value * 0.1f), new Quaternion(Random.value, Random.value, Random.value, Random.value), transform);

                if (i % 150 == 0)
                    yield return null;

                if (i > 30000)
                    break;
            }
    }

}
