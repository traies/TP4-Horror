using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject[] items;
    public float decreaseRate;
    public int NbItemsEasy;
    public int NbItemsNormal;
    public int NbItemsHard;

    private List<GameObject> _items;
    private List<KeyValuePair<Transform, GameObject>> _dropPositions;
    private List<GameObject> _rooms;
    private int _numberOfItems;
    private GameManager _gameManager;
    private List<int> _spawnRates;

}
