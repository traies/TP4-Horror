﻿using System.Collections.Generic;
using UnityEngine;

public class Chunks : MonoBehaviour
{
    public List<Transform> mountPoints;
    public List<Transform> roomPoints;
    public List<Transform> itemPoints;
    public ChunksMap chunkMap;
    public Direction instantiatedDirection;
    public ChunkType type;
    public int initialSpawnRate;

    public int id;

    //public int GetId()
    //{
    //    return _id;
    //}
}
