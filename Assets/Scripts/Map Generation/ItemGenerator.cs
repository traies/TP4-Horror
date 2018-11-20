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

    //public void GenerateItems()
    //{
    //    _gameManager = FindObjectOfType<GameManager>();
    //    _spawnRates = GetInitialSpawnRates();
    //    _rooms = GetMapRooms();
    //    _dropPositions = GetMapDropPositions();
    //    _dropPositions = Shuffle(_dropPositions);
    //    _items = new List<GameObject>();
    //    _numberOfItems = SetNumberOfItems(_gameManager.difficulty);

    //    GameObject item;
    //    GameObject room;
    //    Transform dropPosition;
    //    KeyValuePair<Transform, GameObject> localisation;

    //    while (_items.Count < _numberOfItems && AnyPositionAvailable())
    //    {
    //        item = ChooseFromPoolSize();
    //        localisation = ChooseDropPosition();
    //        dropPosition = localisation.Key;
    //        room = localisation.Value;
    //        DropItem(room, dropPosition, item);
    //    }
    //}

    //private bool AnyPositionAvailable()
    //{
    //    foreach (KeyValuePair<Transform, GameObject> pos in _dropPositions)
    //    {
    //        if (pos.Key.GetComponent<ItemPoint>().available)
    //            return true;
    //    }
    //    return false;
    //}

    //private List<GameObject> GetMapRooms()
    //{
    //    LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
    //    List<GameObject> rooms = new List<GameObject>();
    //    foreach (GameObject obj in levelGenerator.GetChunks())
    //    {
    //        if (obj.GetComponent<Chunks>().type == ChunkType.Room)
    //        {
    //            rooms.Add(obj);
    //        }
    //    }
    //    return rooms;
    //}

    //private List<KeyValuePair<Transform, GameObject>> GetMapDropPositions()
    //{
    //    List<KeyValuePair<Transform, GameObject>> dropPositions = new List<KeyValuePair<Transform, GameObject>>();
    //    foreach (GameObject room in _rooms)
    //    {
    //        foreach (Transform dropPosition in room.GetComponent<Chunks>().itemPoints)
    //        {
    //            dropPositions.Add(new KeyValuePair<Transform, GameObject>(dropPosition, room));
    //        }
    //    }
    //    return dropPositions;
    //}

    //private KeyValuePair<Transform, GameObject> ChooseDropPosition()
    //{
    //    foreach (KeyValuePair<Transform, GameObject> pos in _dropPositions)
    //    {
    //        if (pos.Key.GetComponent<ItemPoint>().available)
    //        {
    //            pos.Key.GetComponent<ItemPoint>().available = false;
    //            return pos;
    //        }
    //    }
    //    return new KeyValuePair<Transform, GameObject>();
    //}

    //// Completely Random choice, dropRates are not considered
    //private GameObject ChooseItem()
    //{
    //    int range = items.Length;
    //    int index = (int)(UnityEngine.Random.value * range - 0.1);
    //    return items[index];
    //}

    //private void DropItem(GameObject room, Transform dropPosition, GameObject item)
    //{
    //    GameObject instantiatedItem = Instantiate(item) as GameObject;
    //    instantiatedItem.transform.parent = room.transform;
    //    instantiatedItem.transform.position = dropPosition.position;
    //    _items.Add(instantiatedItem);
    //}

    //private int SetNumberOfItems (Difficulty difficulty)
    //{
    //    int res;
    //    switch (difficulty)
    //    {
    //        case Difficulty.Easy:
    //            res = NbItemsEasy;
    //            break;
    //        case Difficulty.Hard:
    //            res = NbItemsHard;
    //            break;
    //        default:
    //            res = NbItemsNormal;
    //            break;
    //    }
    //    return res;
    //}

    //private List<KeyValuePair<Transform, GameObject>> Shuffle (List<KeyValuePair<Transform, GameObject>> list)
    //{
    //    System.Random rng = new System.Random();
    //    int n = list.Count;
    //    while (n > 1)
    //    {
    //        n--;
    //        int k = rng.Next(n + 1);
    //        KeyValuePair<Transform, GameObject> value = list[k];
    //        list[k] = list[n];
    //        list[n] = value;
    //    }
    //    return list;
    //}

    //private GameObject ChooseFromPoolSize()
    //{
    //    int id = 0;
    //    int poolSize = 0;
    //    for (int i = 0; i < items.Length; i++)
    //    {
    //        poolSize += _spawnRates[i];
    //    }
    //    int randomNbInPool = (int)UnityEngine.Random.Range(0, poolSize);
    //    int accumalatedProbability = 0;
    //    for (int i = 0; i < items.Length; i++)
    //    {
    //        accumalatedProbability += _spawnRates[i];
    //        if (randomNbInPool <= accumalatedProbability)
    //        {
    //            id = i;
    //            break;
    //        }
    //    }
    //    UpdateSpawnRate(id);
    //    return items[id];
    //}

    //private void UpdateSpawnRate(int id)
    //{
    //    int spawnRate = _spawnRates[id];
    //    _spawnRates[id] -= (int)(decreaseRate * spawnRate);
    //}

    //private List<int> GetInitialSpawnRates()
    //{
    //    List<int> spawnRates = new List<int>();
    //    for (int i = 0; i < items.Length; i++)
    //    {
    //        spawnRates.Add(items[i].GetComponent<Item>().dropRate);
    //    }
    //    return spawnRates;
    //}
}
