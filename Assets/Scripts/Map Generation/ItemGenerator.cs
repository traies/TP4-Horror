using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject[] items;

    private List<GameObject> _items;
    private List<KeyValuePair<Transform, GameObject>> _dropPositions;
    private List<GameObject> _rooms;
    private int numberOfItems = 1;

    public void GenerateItems()
    {
        _rooms = GetMapRooms();
        _dropPositions = GetMapDropPositions();
        _items = new List<GameObject>();

        GameObject item;
        GameObject room;
        Transform dropPosition;
        KeyValuePair<Transform, GameObject> localisation;
        float dropRate;

        while (_items.Count < numberOfItems && AnyPositionAvailable())
        {
            item = ChooseItem();
            localisation = ChooseDropPosition();
            dropPosition = localisation.Key;
            room = localisation.Value;
            dropRate = item.GetComponent<Item>().dropRate;
            if (Random.value <= dropRate)
            {
                DropItem(room, dropPosition, item);
            }
        }
    }

    private bool AnyPositionAvailable()
    {
        foreach (KeyValuePair<Transform, GameObject> pos in _dropPositions)
        {
            if (pos.Key.GetComponent<ItemPoint>().available)
                return true;
        }
        return false;
    }

    private List<GameObject> GetMapRooms()
    {
        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
        List<GameObject> rooms = new List<GameObject>();
        foreach (GameObject obj in levelGenerator.GetChunks())
        {
            if (obj.GetComponent<Chunks>().type == ChunkType.Room)
            {
                rooms.Add(obj);
            }
        }
        return rooms;
    }

    private List<KeyValuePair<Transform, GameObject>> GetMapDropPositions()
    {
        List<KeyValuePair<Transform, GameObject>> dropPositions = new List<KeyValuePair<Transform, GameObject>>();
        foreach (GameObject room in _rooms)
        {
            foreach (Transform dropPosition in room.GetComponent<Chunks>().itemPoints)
            {
                dropPositions.Add(new KeyValuePair<Transform, GameObject>(dropPosition, room));
            }
        }
        return dropPositions;
    }

    private KeyValuePair<Transform, GameObject> ChooseDropPosition()
    {
        foreach (KeyValuePair<Transform, GameObject> pos in _dropPositions)
        {
            if (pos.Key.GetComponent<ItemPoint>().available)
            {
                pos.Key.GetComponent<ItemPoint>().available = false;
                return pos;
            }
        }
        return new KeyValuePair<Transform, GameObject>();
    }

    //Completely Random choice, with no rules at the moment
    private GameObject ChooseItem()
    {
        int range = items.Length;
        int index = (int)(Random.value * range - 0.1);
        return items[index];
    }

    private void DropItem(GameObject room, Transform dropPosition, GameObject item)
    {
        GameObject instantiatedItem = Instantiate(item) as GameObject;
        instantiatedItem.transform.parent = room.transform;
        instantiatedItem.transform.position = dropPosition.position;
        _items.Add(instantiatedItem);
    }
}
