using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
        //    DontDestroyOnLoad(this.gameObject);
    }

    private Player player;
    private GameObject enemy1;
    private GameObject enemy2;
    private GameObject enemy3;
    private GameObject bomb;

    private bool isValid = false;
    float enemyCreateCount = 4f;
    int lv = 1;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        enemy2 = Resources.Load<GameObject>("Prefabs/enemy2");
        enemy1 = Resources.Load<GameObject>("Prefabs/enemy1");
        enemy3 = Resources.Load<GameObject>("Prefabs/enemy3");
        bomb = Resources.Load<GameObject>("Prefabs/bomb");
    }

    public void gameStart()
    {
        isValid = true;
    }

    public void gameStop()
    {
        isValid = false;
    }

    public void upgradeEnemy()
    {
        lv++;
    }

    void createEnemy()
    {
        float middleZ = Random.Range(20f, 24f);
        Instantiate(enemy1, new Vector3(Random.Range(-1, 2) * 0.8f, 1f, player.getPos().z + middleZ + Random.Range(2f, 4f)), Quaternion.Euler(0, 180, 0), transform);

        GameObject obj = Instantiate(bomb, new Vector3(Random.Range(-1, 2) * 0.8f, 1f, player.getPos().z + Random.Range(20, 22)), Quaternion.identity) as GameObject;
        obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * -250);

        if (lv > 3)
        {
            Instantiate(enemy2, new Vector3(Random.Range(-1, 2) * 0.8f, 1f, player.getPos().z + middleZ + Random.Range(10f, 12f)), Quaternion.Euler(0, 180, 0), transform);
        }
        if (lv > 6)
        {
            Instantiate(enemy1, new Vector3(Random.Range(-1, 2) * 0.8f, 1f, player.getPos().z + middleZ + Random.Range(14f, 16f)), Quaternion.Euler(0, 180, 0), transform);
        }
        if (lv > 9)
        {
            Instantiate(enemy3, new Vector3(Random.Range(-1, 2) * 0.8f, 1f, player.getPos().z + middleZ + Random.Range(6f, 8f)), Quaternion.Euler(0, 180, 0), transform);
        }
        if (lv > 12)
        {
            Instantiate(enemy2, new Vector3(Random.Range(-1, 2) * 0.8f, 1f, player.getPos().z + middleZ + Random.Range(18f, 20f)), Quaternion.Euler(0, 180, 0), transform);
        }
        if (lv > 15)
        {
            Instantiate(enemy3, new Vector3(Random.Range(-1, 2) * 0.8f, 1f, player.getPos().z + middleZ + Random.Range(0f, 1f)), Quaternion.Euler(0, 180, 0), transform);
        }



    }

    float time = 0f;
    private void Update()
    {
        if (isValid)
        {
            time += Time.deltaTime;
            if (time > enemyCreateCount)
            {
                time = 0f;
                createEnemy();
            }
        }

    }
}