using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType { Item, Enemy }

public class Generator : MonoBehaviour {

    // information provided by the caller of the Generate method
    private GameObject[] objects;
    private float _decreaseRate;
    private int _NbObjectsEasy;
    private int _NbObjectsNormal;
    private int _NbObjectsHard;

    // generator class internal variables
    private List<GameObject> _objects;
    private List<KeyValuePair<Transform, GameObject>> _dropPositions;
    private List<GameObject> _rooms;
    private int _numberOfObjects;
    private GameManager _gameManager;
    private List<int> _spawnRates;
    private ObjectType _type;

    public void Generate(ObjectType t, GameObject[] obj, int easy, int normal, int hard, float decreaseRate)
    {
        // Instanciating the transferred variables
        objects = obj;
        _NbObjectsEasy = easy;
        _NbObjectsHard = hard;
        _NbObjectsNormal = normal;
        _decreaseRate = decreaseRate;
        _type = t;

        _gameManager = FindObjectOfType<GameManager>();
        _spawnRates = GetInitialSpawnRates();
        //_rooms = GetMapRooms();
        _rooms = GetMapChunks();
        _dropPositions = GetMapDropPositions();
        _dropPositions = Shuffle(_dropPositions);
        _objects = new List<GameObject>();
        _numberOfObjects = SetNumberOfObjects(_gameManager.difficulty);

        GameObject myObject;
        GameObject room;
        Transform dropPosition;
        KeyValuePair<Transform, GameObject> localisation;

        while (_objects.Count < _numberOfObjects && AnyPositionAvailable())
        {
            myObject = ChooseFromPoolSize();
            localisation = ChooseDropPosition();
            dropPosition = localisation.Key;
            room = localisation.Value;
            DropObject(room, dropPosition, myObject);
        }
    }

    private bool AnyPositionAvailable()
    {
        foreach (KeyValuePair<Transform, GameObject> pos in _dropPositions)
        {
            if (pos.Key.GetComponent<DropObjectPoint>().available)
                return true;
        }
        return false;
    }

    private List<GameObject> GetMapChunks()
    {
        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
        //List<GameObject> rooms = new List<GameObject>();
        //foreach (GameObject obj in levelGenerator.GetChunks())
        //{
        //    if (obj.GetComponent<Chunks>().type == ChunkType.Room)
        //    {
        //        rooms.Add(obj);
        //    }
        //}
        //return rooms;
        return levelGenerator.GetChunks();
    }

    private List<KeyValuePair<Transform, GameObject>> GetMapDropPositions()
    {
        List<KeyValuePair<Transform, GameObject>> dropPositions = new List<KeyValuePair<Transform, GameObject>>();
        foreach (GameObject room in _rooms)
        {
            foreach (Transform dropPosition in room.GetComponent<Chunks>().dropPoints)
            {
                if (dropPosition.GetComponent<DropObjectPoint>().type == _type)
                {
                    dropPositions.Add(new KeyValuePair<Transform, GameObject>(dropPosition, room));
                }
            }
        }
        return dropPositions;
    }

    private KeyValuePair<Transform, GameObject> ChooseDropPosition()
    {
        foreach (KeyValuePair<Transform, GameObject> pos in _dropPositions)
        {
            if (pos.Key.GetComponent<DropObjectPoint>().available)
            {
                pos.Key.GetComponent<DropObjectPoint>().available = false;
                return pos;
            }
        }
        return new KeyValuePair<Transform, GameObject>();
    }

    // Completely Random choice, dropRates are not considered
    private GameObject ChooseObject()
    {
        int range = objects.Length;
        int index = (int)(UnityEngine.Random.value * range - 0.1);
        return objects[index];
    }

    private void DropObject(GameObject room, Transform dropPosition, GameObject obj)
    {
        GameObject instantiatedObject = Instantiate(obj) as GameObject;
        instantiatedObject.transform.parent = room.transform;
        instantiatedObject.transform.position = dropPosition.position;
        _objects.Add(instantiatedObject);
    }

    private int SetNumberOfObjects(Difficulty difficulty)
    {
        int res;
        switch (difficulty)
        {
            case Difficulty.Easy:
                res = _NbObjectsEasy;
                break;
            case Difficulty.Hard:
                res = _NbObjectsHard;
                break;
            default:
                res = _NbObjectsNormal;
                break;
        }
        return res;
    }

    private List<KeyValuePair<Transform, GameObject>> Shuffle(List<KeyValuePair<Transform, GameObject>> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            KeyValuePair<Transform, GameObject> value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    private GameObject ChooseFromPoolSize()
    {
        int id = 0;
        int poolSize = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            poolSize += _spawnRates[i];
        }
        int randomNbInPool = (int)UnityEngine.Random.Range(0, poolSize);
        int accumalatedProbability = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            accumalatedProbability += _spawnRates[i];
            if (randomNbInPool <= accumalatedProbability)
            {
                id = i;
                break;
            }
        }
        UpdateSpawnRate(id);
        return objects[id];
    }

    private void UpdateSpawnRate(int id)
    {
        int spawnRate = _spawnRates[id];
        _spawnRates[id] -= (int)(_decreaseRate * spawnRate);
    }

    private List<int> GetInitialSpawnRates()
    {
        List<int> spawnRates = new List<int>();
        for (int i = 0; i < objects.Length; i++)
        {
            spawnRates.Add(objects[i].GetComponent<DropObject>().dropRate);
        }
        return spawnRates;
    }
}
