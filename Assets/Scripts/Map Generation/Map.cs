using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public int length;
    public bool[,] map;
    private bool collision;

    public Map(int length)
    {
        map = new bool[length, length];
        collision = false;
    }

    // Returns true if the chunk was added, if not returns false
    public bool Add(ChunksMap chunk)
    {
        foreach (MapCoordinates coord in chunk.coords)
        {
            try
            {
                if (map[coord.x, coord.y] == true)
                {
                    //Debug.Log(coord.x + " : " + coord.y);
                    collision = true;
                }
            } catch (IndexOutOfRangeException ex)
            {
                collision = true;
            }
            
        }
        if (collision)
        {
            collision = false;
            return false;
        }
        else
        {
            foreach (MapCoordinates coord in chunk.coords)
            {
                map[coord.x, coord.y] = true;
            }
            return true;
        }

    }
}
