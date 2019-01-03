using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : SingletonMonoBehaviour<StageManager>
{

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
    }


    [SerializeField]
    private GameObject stageParent;

    private Player player;

    private GameObject normal1;
    private GameObject normal2;
    private GameObject torch;
    private GameObject well;
    private GameObject bridgeDual;
    private GameObject fireCenter;
    private GameObject trapCenter;
    private GameObject trapLeft;
    private GameObject trapRight;
    private GameObject trapDual;

    private GameObject coin;
    private GameObject specialCoin;
    private GameObject magnet;
    private GameObject coinUp;

    private float stageLength = -1;
    private int lv = 0;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        normal1 = Resources.Load<GameObject>("Prefabs/normal1");
        normal2 = Resources.Load<GameObject>("Prefabs/normal2");
        torch = Resources.Load<GameObject>("Prefabs/torch");
        well = Resources.Load<GameObject>("Prefabs/well");
        bridgeDual = Resources.Load<GameObject>("Prefabs/bridgeDual");
        trapCenter = Resources.Load<GameObject>("Prefabs/trapCenter");
        trapLeft = Resources.Load<GameObject>("Prefabs/trapLeft");
        trapRight = Resources.Load<GameObject>("Prefabs/trapRight");

        trapDual = Resources.Load<GameObject>("Prefabs/trapDual");
        fireCenter = Resources.Load<GameObject>("Prefabs/fireCenter");

        coin = Resources.Load<GameObject>("Prefabs/coin");
        specialCoin = Resources.Load<GameObject>("Prefabs/specialCoin");
        magnet = Resources.Load<GameObject>("Prefabs/magnet");
        coinUp = Resources.Load<GameObject>("Prefabs/coinUpItem");
        createStage();
    }

    void Update()
    {
        float z = player.getPos().z;
        if (z > stageLength * 2.7f - 30)
        {
            createStage();
            destroyStage(z - 10);
        }
    }

    void createStage()
    {
        for (int i = 0; i < 25; i++)
        {
            int random = UnityEngine.Random.Range(1, 100);
            GameObject createObj = normal1;
            if (random < 5)
                createObj = normal1;
            else if (random < 10)
                createObj = normal2;
            else if (random < 20)
                createObj = torch;
            else if (random < 30)
                createObj = well;
            else if (random < 55)
                createObj = bridgeDual;
            else if (random < 70)
                createObj = trapCenter;
            else if (random < 85)
                createObj = trapLeft;
            else
                createObj = trapRight;

            if (createObj == torch || createObj == well)
            {
                List<int> line = new List<int>();
                line.Add(-1);
                line.Add(0);
                line.Add(1);
                int intx = line[Random.Range(0, line.Count)];
                float x = intx * 0.9f;
                coinCreate(x);
                line.Remove(intx);
                if (lv > 2)
                {
                    intx = line[Random.Range(0, line.Count)];
                    x = intx * 0.9f;
                    coinCreate(x);
                    line.Remove(intx);
                    if (lv > 12)
                    {
                        intx = line[Random.Range(0, line.Count)];
                        x = intx * 0.9f;
                        coinCreate(x);
                    }
                }
            }

            if (createObj == normal1 || createObj == normal2)
            {
                if (i != 0)
                {
                    List<int> line = new List<int>();
                    line.Add(-1);
                    line.Add(0);
                    line.Add(1);
                    int intx = line[Random.Range(0, line.Count)];
                    float x = intx * 0.9f;
                    coinCreate(x);
                    line.Remove(intx);
                    if (lv > 4)
                    {
                        intx = line[Random.Range(0, line.Count)];
                        x = intx * 0.9f;
                        coinCreate(x);
                        line.Remove(intx);
                        if (lv > 14)
                        {
                            intx = line[Random.Range(0, line.Count)];
                            x = intx * 0.9f;
                            coinCreate(x);
                        }
                    }
                }
            }

            if (createObj == trapRight)
            {
                List<int> line = new List<int>();
                line.Add(-1);
                line.Add(0);
                int intx = line[Random.Range(0, line.Count)];
                float x = intx * 0.9f;
                coinCreate(x);
                line.Remove(intx);

                if (lv > 16)
                {
                    intx = line[Random.Range(0, line.Count)];
                    x = intx * 0.9f;
                    coinCreate(x);
                    line.Remove(intx);
                }

            }

            if (createObj == trapLeft)
            {
                List<int> line = new List<int>();
                line.Add(1);
                line.Add(0);
                int intx = line[Random.Range(0, line.Count)];
                float x = intx * 0.9f;
                if (lv > 6)
                {
                    coinCreate(x);
                    line.Remove(intx);
                }
                if (lv > 18)
                {
                    intx = line[Random.Range(0, line.Count)];
                    x = intx * 0.9f;
                    coinCreate(x);
                    line.Remove(intx);
                }
            }

            if (createObj == trapCenter)
            {
                List<int> line = new List<int>();
                line.Add(1);
                line.Add(-1);
                int intx = line[Random.Range(0, line.Count)];
                float x = intx * 0.9f;
                if (lv > 8)
                {
                    coinCreate(x);
                    line.Remove(intx);
                }
                if (lv > 20)
                {
                    intx = line[Random.Range(0, line.Count)];
                    x = intx * 0.9f;
                    coinCreate(x);
                    line.Remove(intx);
                }

            }



            if (i < 5)
            {
                createObj = normal1;
            }

            if (i == 0 )
            {
                int rand = UnityEngine.Random.Range(0, 10);
                GameObject createItem = specialCoin;
                if (rand < 4)
                    createItem = specialCoin;
                if (rand >= 4 && rand < 7)
                    createItem = magnet;
                if (rand >= 7 && rand < 10)
                    createItem = coinUp;

                Instantiate(createItem, new Vector3(0, 0.6f, stageLength * 2.7f), Quaternion.identity, stageParent.transform);
            }

            Instantiate(createObj, new Vector3(0, 0, stageLength * 2.7f), Quaternion.Euler(0, 90f, 0), stageParent.transform);
            stageLength += 1;

        }
    }

    void coinCreate(float x)
    {
        Instantiate(coin, new Vector3(x, 0.6f, stageLength * 2.7f), Quaternion.identity, stageParent.transform);
        Instantiate(coin, new Vector3(x, 0.6f, stageLength * 2.7f + 0.9f), Quaternion.identity, stageParent.transform);
        Instantiate(coin, new Vector3(x, 0.6f, stageLength * 2.7f + 1.8f), Quaternion.identity, stageParent.transform);
    }

    public void upgradeStage()
    {
        lv++;
    }

    void destroyStage(float pos)
    {
        Transform parent = stageParent.transform;
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject obj = parent.GetChild(i).gameObject;
            if (obj.transform.position.z < pos)
                GameObject.Destroy(obj);
        }
    }
}
