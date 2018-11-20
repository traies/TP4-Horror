using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    public GameObject[] enemies;
    public float decreaseRate;
    public int NbItemsEasy;
    public int NbItemsNormal;
    public int NbItemsHard;
    public ObjectType type = ObjectType.Enemy;
}
