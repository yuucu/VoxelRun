using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyLv : ScriptableObject {

    EnemyLvStatus[] lv;
}

public class EnemyLvStatus
{
    public int enemyCreate;
    public int time;
}
