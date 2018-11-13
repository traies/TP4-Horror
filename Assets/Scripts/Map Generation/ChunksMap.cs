using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksMap
{
    public List<MapCoordinates> coords;
    public List<MapCoordinates> mountPoints;
    public List<MapCoordinates> roomPoints;

    public ChunksMap(int id, MapCoordinates origin, Direction direction)
    {
        coords =  new List<MapCoordinates>();
        mountPoints = new List<MapCoordinates>();
        roomPoints = new List<MapCoordinates>();
        coords.Add(origin);

        switch (id)
        {
            case 0: // corridor straight
                switch (direction)
                {
                    case Direction.North:
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y - 2));
                        mountPoints.Add(new MapCoordinates(origin.x, origin.y - 3));
                        break;
                    case Direction.East:
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 2, origin.y));
                        mountPoints.Add(new MapCoordinates(origin.x + 3, origin.y));
                        break;
                    case Direction.South:
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y + 2));
                        mountPoints.Add(new MapCoordinates(origin.x, origin.y + 3));
                        break;
                    case Direction.West:
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 2, origin.y));
                        mountPoints.Add(new MapCoordinates(origin.x - 3, origin.y));
                        break;
                }
                break;

            case 1: // corner right
                switch (direction)
                {
                    case Direction.North:
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        mountPoints.Add(new MapCoordinates(origin.x + 2, origin.y - 1));
                        break;
                    case Direction.East:
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        mountPoints.Add(new MapCoordinates(origin.x + 1, origin.y + 2));
                        break;
                    case Direction.South:
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        mountPoints.Add(new MapCoordinates(origin.x - 2, origin.y + 1));
                        break;
                    case Direction.West:
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        mountPoints.Add(new MapCoordinates(origin.x - 1, origin.y - 2));
                        break;
                }
                break;

            case 2: // corner left
                switch (direction)
                {
                    case Direction.North:
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        mountPoints.Add(new MapCoordinates(origin.x - 2, origin.y - 1));
                        break;
                    case Direction.East:
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        mountPoints.Add(new MapCoordinates(origin.x + 1, origin.y - 2));
                        break;
                    case Direction.South:
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        mountPoints.Add(new MapCoordinates(origin.x + 2, origin.y + 1));
                        break;
                    case Direction.West:
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        mountPoints.Add(new MapCoordinates(origin.x - 1, origin.y + 2));
                        break;
                }
                break;

            case 3: // T corner (duplicate)
                switch (direction)
                {
                    case Direction.North:
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        mountPoints.Add(new MapCoordinates(origin.x - 2, origin.y - 1));
                        mountPoints.Add(new MapCoordinates(origin.x + 2, origin.y - 1));
                        break;
                    case Direction.East:
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        mountPoints.Add(new MapCoordinates(origin.x + 1, origin.y - 2));
                        mountPoints.Add(new MapCoordinates(origin.x + 1, origin.y + 2));
                        break;
                    case Direction.South:
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        mountPoints.Add(new MapCoordinates(origin.x + 2, origin.y + 1));
                        mountPoints.Add(new MapCoordinates(origin.x - 2, origin.y + 1));
                        break;
                    case Direction.West:
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        mountPoints.Add(new MapCoordinates(origin.x - 1, origin.y + 2));
                        mountPoints.Add(new MapCoordinates(origin.x - 1, origin.y - 2));
                        break;
                }
                break;

            case 4: // T corner (unify)
                switch (direction)
                {
                    case Direction.North:
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y - 2));
                        mountPoints.Add(new MapCoordinates(origin.x, origin.y - 3));
                        mountPoints.Add(new MapCoordinates(origin.x + 2, origin.y - 1));
                        break;
                    case Direction.East:
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x + 2, origin.y));
                        mountPoints.Add(new MapCoordinates(origin.x + 3, origin.y));
                        mountPoints.Add(new MapCoordinates(origin.x + 1, origin.y + 2));
                        break;
                    case Direction.South:
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y + 2));
                        mountPoints.Add(new MapCoordinates(origin.x, origin.y + 3));
                        mountPoints.Add(new MapCoordinates(origin.x - 2, origin.y + 1));
                        break;
                    case Direction.West:
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x - 2, origin.y));
                        mountPoints.Add(new MapCoordinates(origin.x - 3, origin.y));
                        mountPoints.Add(new MapCoordinates(origin.x - 1, origin.y - 2));
                        break;
                }
                break;

            case 5: // square room
                switch (direction)
                {
                    case Direction.North:
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y - 2));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 2));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 2));
                        break;
                    case Direction.East:
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 2, origin.y));
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x + 2, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x + 2, origin.y + 1));
                        break;
                    case Direction.South:
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y + 2));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 2));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 2));
                        break;
                    case Direction.West:
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 2, origin.y));
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x - 2, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x - 2, origin.y + 1));
                        break;
                }
                break;

            case 6: // corridor straight with one door
                switch (direction)
                {
                    case Direction.North:
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y - 2));
                        mountPoints.Add(new MapCoordinates(origin.x, origin.y - 3));
                        roomPoints.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        break;
                    case Direction.East:
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 2, origin.y));
                        mountPoints.Add(new MapCoordinates(origin.x + 3, origin.y));
                        roomPoints.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        break;
                    case Direction.South:
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y + 2));
                        mountPoints.Add(new MapCoordinates(origin.x, origin.y + 3));
                        roomPoints.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        break;
                    case Direction.West:
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 2, origin.y));
                        mountPoints.Add(new MapCoordinates(origin.x - 3, origin.y));
                        roomPoints.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        break;
                }
                break;

            case 7: // corridor straight with two doors
                switch (direction)
                {
                    case Direction.North:
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y - 2));
                        mountPoints.Add(new MapCoordinates(origin.x, origin.y - 3));
                        roomPoints.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        roomPoints.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        break;
                    case Direction.East:
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 2, origin.y));
                        mountPoints.Add(new MapCoordinates(origin.x + 3, origin.y));
                        roomPoints.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        roomPoints.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        break;
                    case Direction.South:
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y + 2));
                        mountPoints.Add(new MapCoordinates(origin.x, origin.y + 3));
                        roomPoints.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        roomPoints.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        break;
                    case Direction.West:
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 2, origin.y));
                        mountPoints.Add(new MapCoordinates(origin.x - 3, origin.y));
                        roomPoints.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        roomPoints.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        break;
                }
                break;

            case 8: // starting room
                switch (direction)
                {
                    case Direction.North:
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y - 2));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 2));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 2));
                        mountPoints.Add(new MapCoordinates(origin.x, origin.y - 3));
                        break;
                    case Direction.East:
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 2, origin.y));
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x + 2, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x + 2, origin.y + 1));
                        mountPoints.Add(new MapCoordinates(origin.x + 3, origin.y));
                        break;
                    case Direction.South:
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y + 2));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 2));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x + 1, origin.y + 2));
                        mountPoints.Add(new MapCoordinates(origin.x, origin.y + 3));
                        break;
                    case Direction.West:
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y));
                        coords.Add(new MapCoordinates(origin.x - 2, origin.y));
                        coords.Add(new MapCoordinates(origin.x, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x - 2, origin.y - 1));
                        coords.Add(new MapCoordinates(origin.x, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x - 1, origin.y + 1));
                        coords.Add(new MapCoordinates(origin.x - 2, origin.y + 1));
                        mountPoints.Add(new MapCoordinates(origin.x - 3, origin.y));
                        break;
                }
                break;
        }
    }
}
