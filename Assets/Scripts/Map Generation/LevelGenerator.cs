using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Linq;

public enum Direction { North, South, East, West };
public enum ChunkType { Corridor, Corner, T, Room };

// TODO : UPDATE THE RAND SYSTEM WITH AN ID PARAMETER IN EACH PREFAB
// TODO : MAKE THE GENERATION FUNCTIONS COMPLETELY INDEPENDANT FROM ONE ANOTHER

public class LevelGenerator : MonoBehaviour
{
    // Level ressources
    public GameObject[] chunks;
    public GameObject startingRoom;
    public GameObject endWall;

    // Level parameters
    public int nbOfRooms;
    public float roomSpawnRate;
    public int mapSideDimension;
    public float decreaseRate;

    //private parameters
    private float rotY;
    private List<int> cache;
    private Map map;
    private bool collision;
    private Direction direction;
    private GameObject newChunk;
    private ChunksMap newMapChunk;
    private MapCoordinates origin;
    private Transform mountTransform;
    private List<GameObject> _chunks;
    private List<int> _spawnRates;
    private List<KeyValuePair<Direction, Transform>> _unMountedPoints;
    private KeyValuePair<Direction, Transform> _currentPoint;
    private Transform _parentPoint;

    [ContextMenu("Generate Map")]
    public void GenerateMap()
    {
        // Initialisation
        origin = new MapCoordinates((int)(mapSideDimension / 2) + 1, (int)(mapSideDimension / 2) + 1);
        mountTransform = gameObject.transform;
        map = new Map(mapSideDimension);
        cache = new List<int>();
        _chunks = new List<GameObject>();
        _unMountedPoints = new List<KeyValuePair<Direction, Transform>>();
        _spawnRates = GetInitialSpawnRates();
        collision = false;
        direction = Direction.East;
        rotY = 0;
        int rand;
        bool added;

        DestroyAllChildren();
        GenerateLevelBasicStructure();
        do
        { 
            do
            {
                rand = GetRandomChunk();
                added = GenerateChunk(rand);
                if (cache.Count < chunks.Length && added)
                {
                    _chunks.Add(newChunk);

                    if (newChunk.GetComponent<Chunks>().type != ChunkType.Room)
                    {
                        UpdateSpawnRate(rand);
                    }

                    if (IsThereRoomMountingPoints(newChunk))
                    {
                        mountTransform = newChunk.GetComponent<Chunks>().roomPoints[0];
                        origin = newChunk.GetComponent<Chunks>().chunkMap.roomPoints[0];
                        Direction tempDir = UpdateDirection(direction, mountTransform);
                        if (GenerateRoom(5, origin, mountTransform, tempDir))
                            newChunk.GetComponent<Chunks>().roomPoints[0].GetComponent<MountPoint>().available = false;
                    }

                    if (IsThereRoomMountingPoints(newChunk) && newChunk.GetComponent<Chunks>().roomPoints.Count > 1)
                    {
                        mountTransform = newChunk.GetComponent<Chunks>().roomPoints[1];
                        origin = newChunk.GetComponent<Chunks>().chunkMap.roomPoints[1];
                        Direction tempDir = UpdateDirection(direction, mountTransform);
                        if (GenerateRoom(5, origin, mountTransform, tempDir))
                            newChunk.GetComponent<Chunks>().roomPoints[1].GetComponent<MountPoint>().available = false;
                    }
                }
                else if (cache.Count == chunks.Length)
                {
                    ClearCache();
                    _unMountedPoints.Add(_currentPoint);
                    break;
                }
            } while (!added);
        } while (CountNumberOfRooms() < nbOfRooms && SetNextMountingPoint());

        // if one of the condition is not fullfiled then restart the process
        //if (CountNumberOfRooms() < nbOfRooms)
        //{
        //    Debug.LogWarning("Couldn't generate level. Try different settings");
        //}

        Debug.Log(_spawnRates[0] + " " + _spawnRates[1] + " " + _spawnRates[2] + " " + _spawnRates[3] + " " + _spawnRates[4] + " " + _spawnRates[5]);

        // BUGS : mur mal orienté dans le cas des corridors, mettre la texture de l'autre côté
        CloseCorridors();

        Debug.Log("END");
    }


    [ContextMenu("Destroy all children")]
    private void DestroyAllChildren()
    {
        var tempList = transform.Cast<Transform>().ToList();
         foreach(var child in tempList)
         {
             DestroyImmediate(child.gameObject);
         }
    }

    // We have to "close" the map = instantiate a wall at each mountPoint that wasn't used ( available = true )
    // Also for stuckPoints, and roomPoints which are still available
    // NOT OPTIMIZED
    private void CloseCorridors()
    {
        Direction dir;
        // We check all remaining mountPoint still available
        foreach (GameObject obj in _chunks)
        {
            foreach (Transform point in obj.GetComponent<Chunks>().mountPoints)
            {
                if (point.GetComponent<MountPoint>().available)
                {
                    GameObject wall = Instantiate(endWall) as GameObject;
                    wall.transform.parent = transform;
                    wall.transform.position = point.transform.position;
                    dir = UpdateDirection(obj.GetComponent<Chunks>().instantiatedDirection, point);
                    wall.transform.Rotate(new Vector3(0, UpdateRotation(dir), 0));
                }
            }
        }

        // We check all stuckPoints : is it really necessary to keep the information of the MountPoint ? the Transform could be all ?
        foreach (KeyValuePair<Direction, Transform> point in _unMountedPoints)
        {
            GameObject wall = Instantiate(endWall) as GameObject;
            wall.transform.parent = transform;
            wall.transform.position = point.Value.transform.position;
            dir = UpdateDirection(point.Key, point.Value);
            wall.transform.Rotate(new Vector3(0, UpdateRotation(dir), 0));
        }

        // We check all roomPoints still available
        foreach (GameObject obj in _chunks)
        {
            foreach (Transform point in obj.GetComponent<Chunks>().roomPoints)
            {
                if (point.GetComponent<MountPoint>().available)
                {
                    GameObject wall = Instantiate(endWall) as GameObject;
                    wall.transform.parent = transform;
                    wall.transform.position = point.transform.position;
                    dir = UpdateDirection(obj.GetComponent<Chunks>().instantiatedDirection, point);
                    wall.transform.Rotate(new Vector3(0, UpdateRotation(dir), 0));
                }
            }
        }
    }

    private Transform GetRoomMountingPoint()
    {
        foreach (GameObject obj in _chunks)
        {
            foreach (Transform roomPoint in obj.GetComponent<Chunks>().roomPoints)
            {
                if (roomPoint.GetComponent<MountPoint>().available)
                {
                    roomPoint.GetComponent<MountPoint>().available = false;
                    return roomPoint;
                }
            }
        }
        return null; // Never triggered if uses correctly
    }

    private MapCoordinates GetRoomMountingPoint(bool param)
    {
        foreach (GameObject obj in _chunks)
        {
            foreach (MapCoordinates roomPoint in obj.GetComponent<Chunks>().chunkMap.roomPoints)
            {
                return roomPoint;
            }
        }
        return null; // Never triggered if uses correctly
    }

    private bool IsThereRoomMountingPoints(GameObject chunk)
    {
        foreach (Transform roomPoint in chunk.GetComponent<Chunks>().roomPoints)
        {
            if (roomPoint.GetComponent<MountPoint>().available)
            {
                return true;
            }
        }
        return false;
    }

    private void GenerateStartingRoom()
    {
        newMapChunk = new ChunksMap(8, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(startingRoom) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            ClearCache();
        }
    }

    private bool GenerateChunk(int id)
    {
        newMapChunk = new ChunksMap(id, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(chunks[id]) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            ClearCache();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GenerateLevelBasicStructure()
    {
        GenerateStartingRoom(); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(3); _chunks.Insert(0, newChunk);
        newChunk.GetComponent<Chunks>().mountPoints[1].GetComponent<MountPoint>().available = false;
        SetNextMountingPoint();
        GenerateChunk(2); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(4); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(2); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(4); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(2); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(4); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(2); _chunks.Insert(0, newChunk);
        // Pass the last unused mountingPoint to unavailable manually
        newChunk.GetComponent<Chunks>().mountPoints[0].GetComponent<MountPoint>().available = false;
        SetNextMountingPoint();
    }

    private int CountNumberOfRooms()
    {
        int count = 0;
        foreach (GameObject obj in _chunks)
        {
            if (obj.GetComponent<Chunks>().type == ChunkType.Room)
            {
                count++;
            }
        }
        return count;
    }

    private bool GenerateRoom(int id, MapCoordinates coords, Transform roomPoint, Direction dir)
    {
        GameObject chunk;
        ChunksMap mapChunk = new ChunksMap(id, coords, dir);
        if (map.Add(mapChunk))
        {
            chunk = Instantiate(chunks[id]) as GameObject;
            chunk.transform.parent = transform;
            chunk.transform.position = roomPoint.position;
            chunk.transform.Rotate(new Vector3(0, UpdateRotation(dir), 0));
            chunk.GetComponent<Chunks>().chunkMap = mapChunk;
            chunk.GetComponent<Chunks>().instantiatedDirection = dir;
            _chunks.Insert(0, chunk);
            return true;
        }
        return false;
    }

    private bool SetNextMountingPoint()
    {
        foreach (GameObject obj in _chunks)
        {
            List<Transform> p = obj.GetComponent<Chunks>().mountPoints;
            for (int i = 0; i < p.Count; i++)
            {
                if (p[i].GetComponent<MountPoint>().available)
                {
                    _currentPoint = new KeyValuePair<Direction, Transform>(obj.GetComponent<Chunks>().instantiatedDirection, p[i]);
                    p[i].GetComponent<MountPoint>().available = false;
                    _parentPoint = p[i];
                    mountTransform = p[i];
                    origin = obj.GetComponent<Chunks>().chunkMap.mountPoints[i];
                    direction = obj.GetComponent<Chunks>().instantiatedDirection;
                    UpdateDirection();
                    UpdateRotation();
                    return true;
                }
            }
        }
        return false;
    }

    //Add some rules here to shape the level
    private int GetRandomChunk()
    {
        // With a decided probability, randomly choose between generating a room or a corridor piece
        //if (Random.value <= roomSpawnRate_1)
        //{
        //    return 5; // hardcoded, I have to change that
        //}
        //else
        {
            // Create dynamicaly the index list
            List<int> indexList = new List<int>();
            for (int i = 0; i < chunks.Length; i++)
            {
                if (!cache.Contains(i))
                    indexList.Add(i);
            }

            // Get a random value of the index List : it has to NOT include the room, or the distinction is failed with the corridors
            //int rand = indexList[(int)((indexList.Count - 1) * Random.value - 0.1)];
            // Use a poolSize to select an item depending on its spawnRate
            int rand = ChooseFromPoolSize();

            // Update cache
            cache.Add(rand);
            // Check if there is any possibility left for the procedure to continue
            if (cache.Count == chunks.Length)
            {
                Debug.Log("procedure stopped because there were no more possibilities");
            }
            return rand;
        }
    }

    private Direction UpdateDirection(Direction dir, Transform point)
    {
        Direction res = dir;
        if (point.GetComponent<MountPoint>().direction == Direction.West) // corner left like
        {
            switch (dir)
            {
                case Direction.North: res = Direction.West; break;
                case Direction.West: res = Direction.South; break;
                case Direction.South: res = Direction.East; break;
                case Direction.East: res = Direction.North; break;
            }
        }
        else if (point.GetComponent<MountPoint>().direction == Direction.East) // corner right like
        {
            switch (dir)
            {
                case Direction.North: res = Direction.East; break;
                case Direction.West: res = Direction.North; break;
                case Direction.South: res = Direction.West; break;
                case Direction.East: res = Direction.South; break;
            }
        }
        return res;
    }

    private int UpdateRotation(Direction dir)
    {
        int rot = 0;
        switch (dir)
        {
            case Direction.North:
                rot = -90;
                break;
            case Direction.West:
                rot = -180;
                break;
            case Direction.South:
                rot = 90;
                break;
            case Direction.East:
                rot = 0;
                break;
        }
        return rot;
    }

    private void UpdateDirection()
    {
        if (mountTransform.GetComponent<MountPoint>().direction == Direction.West) // corner left like
        {
            switch (direction)
            {
                case Direction.North: direction = Direction.West; break;
                case Direction.West: direction = Direction.South; break;
                case Direction.South: direction = Direction.East; break;
                case Direction.East: direction = Direction.North; break;
            }
        }
        else if (mountTransform.GetComponent<MountPoint>().direction == Direction.East) // corner right like
        {
            switch (direction)
            {
                case Direction.North: direction = Direction.East; break;
                case Direction.West: direction = Direction.North; break;
                case Direction.South: direction = Direction.West; break;
                case Direction.East: direction = Direction.South; break;
            }
        }
    }

    private void UpdateRotation()
    {
        switch (direction)
        {
            case Direction.North:
                rotY = -90;
                break;
            case Direction.West:
                rotY = -180;
                break;
            case Direction.South:
                rotY = 90;
                break;
            case Direction.East:
                rotY = 0;
                break;
        }
    }

    private void ClearCache()
    {
        cache = new List<int>();
    }

    public void SetCollision(bool value)
    {
        collision = value;
    }

    public List<GameObject> GetChunks()
    {
        return _chunks;
    }

    private int ChooseFromPoolSize()
    {
        int poolSize = 0;
        for (int i = 0; i < chunks.Length; i++)
        {
            poolSize += _spawnRates[i];
        }
        int randomNbInPool = (int)Random.Range(0, poolSize);
        int accumalatedProbability = 0;
        for (int i = 0; i < chunks.Length; i++)
        {
            accumalatedProbability += _spawnRates[i];
            if (randomNbInPool <= accumalatedProbability)
                //return chunks[i].GetComponent<Chunks>().id;
                return i;
        }
        return 0;
    }

    private void UpdateSpawnRate(int id)
    {
        int spawnRate = _spawnRates[id];
        _spawnRates[id] -= (int)(decreaseRate * spawnRate);
    }

    private List<int> GetInitialSpawnRates()
    {
        List<int> spawnRates = new List<int>();
        for (int i = 0; i < chunks.Length; i++)
        {
            spawnRates.Add(chunks[i].GetComponent<Chunks>().initialSpawnRate);
        }
        return spawnRates;
    }
}