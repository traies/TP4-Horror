using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Direction { North, South, East, West };
public enum ChunkType { Corridor, Corner, T, Room };

// TODO : UPDATE THE RAND SYSTEM WITH AN ID PARAMETER IN EACH PREFAB
// TODO : MAKE THE GENERATION FUNCTIONS COMPLETELY INDEPENDANT FROM ONE ANOTHER

public class LevelGenerator : MonoBehaviour
{
    // Level ressources
    public GameObject[] chunks;
    public GameObject startingRoom;

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

    public void Test2()
    {
        // Initialisation
        origin = new MapCoordinates((int)(mapSideDimension / 2) + 1, (int)(mapSideDimension / 2) + 1);
        mountTransform = gameObject.transform;
        map = new Map(mapSideDimension);
        cache = new List<int>();
        _chunks = new List<GameObject>();
        _spawnRates = GetInitialSpawnRates();
        collision = false;
        direction = Direction.East;
        rotY = 0;
        int rand;
        bool added;

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

                    // Update the spawnRate of the choosen Chunk to disminish its spawn probabilities in the future
                    if (newChunk.GetComponent<Chunks>().type != ChunkType.Room)
                    {
                        UpdateSpawnRate(rand);
                    }

                    //if (newChunk.GetComponent<Chunks>().type == ChunkType.Corridor)
                    //{
                    //    if (Random.value <= roomSpawnRate)
                    //    {
                    //        mountTransform = newChunk.GetComponent<Chunks>().roomPoints[0];
                    //        origin = newChunk.GetComponent<Chunks>().chunkMap.roomPoints[0];
                    //        Direction tempDir = UpdateDirection(direction, mountTransform);
                    //        GenerateRoom(5, origin, mountTransform, tempDir); // PAS DE COHERENCE AVEC GENERATECHUNK, LE ADD EST INDEPENDANT
                    //    }
                    //    if (Random.value <= roomSpawnRate)
                    //    {
                    //        mountTransform = newChunk.GetComponent<Chunks>().roomPoints[1];
                    //        origin = newChunk.GetComponent<Chunks>().chunkMap.roomPoints[1];
                    //        Direction tempDir = UpdateDirection(direction, mountTransform);
                    //        GenerateRoom(5, origin, mountTransform, tempDir);
                    //    }
                    //}

                    if (IsThereRoomMountingPoints(newChunk))
                    {
                        mountTransform = newChunk.GetComponent<Chunks>().roomPoints[0];
                        origin = newChunk.GetComponent<Chunks>().chunkMap.roomPoints[0];
                        Direction tempDir = UpdateDirection(direction, mountTransform);
                        GenerateRoom(5, origin, mountTransform, tempDir);
                    }
                    if (IsThereRoomMountingPoints(newChunk))
                    {
                        mountTransform = newChunk.GetComponent<Chunks>().roomPoints[1];
                        origin = newChunk.GetComponent<Chunks>().chunkMap.roomPoints[1];
                        Direction tempDir = UpdateDirection(direction, mountTransform);
                        GenerateRoom(5, origin, mountTransform, tempDir);
                    }
                }
                else if (cache.Count == chunks.Length)
                {
                    ClearCache();
                    break;
                }
            } while (!added);
        } while (SetNextMountingPoint() && CountNumberOfRooms() < nbOfRooms);

        // if one of the condition is not fullfiled then restart the process
        if (CountNumberOfRooms() < nbOfRooms)
        {
            SceneManager.LoadScene("GenerationScene");
        }
        Debug.Log(_spawnRates[0] + " " + _spawnRates[1] + " " + _spawnRates[2] + " " + _spawnRates[3] + " " + _spawnRates[4] + " " + _spawnRates[5]);
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
                    roomPoint.GetComponent<MountPoint>().available = false;
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
        GenerateChunk(3); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(2); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(4); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(2); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(4); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(2); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(4); _chunks.Insert(0, newChunk); SetNextMountingPoint();
        GenerateChunk(2); _chunks.Insert(0, newChunk); SetNextMountingPoint();
    }

    public void Test()
    {
        origin = new MapCoordinates((int)(mapSideDimension/2) + 1, (int)(mapSideDimension / 2) + 1);
        mountTransform = gameObject.transform;
        map = new Map(mapSideDimension);
        cache = new List<int>();
        _chunks = new List<GameObject>();
        _spawnRates = GetInitialSpawnRates();
        collision = false;
        direction = Direction.East;
        rotY = 0;
        int rand;
        bool added;

        newMapChunk = new ChunksMap(6, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(startingRoom) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            _chunks.Insert(0, newChunk);
            ClearCache();
            SetNextMountingPoint();
        }

        rand = 3;
        newMapChunk = new ChunksMap(rand, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(chunks[rand]) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            _chunks.Insert(0, newChunk);
            ClearCache();
            SetNextMountingPoint();
        }

        rand = 2;
        newMapChunk = new ChunksMap(rand, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(chunks[rand]) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            _chunks.Insert(0, newChunk);
            ClearCache();
            SetNextMountingPoint();
        }

        rand = 4;
        newMapChunk = new ChunksMap(rand, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(chunks[rand]) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            _chunks.Insert(0, newChunk);
            ClearCache();
            SetNextMountingPoint();
        }

        rand = 2;
        newMapChunk = new ChunksMap(rand, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(chunks[rand]) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            _chunks.Insert(0, newChunk);
            ClearCache();
            SetNextMountingPoint();
        }

        rand = 4;
        newMapChunk = new ChunksMap(rand, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(chunks[rand]) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            _chunks.Insert(0, newChunk);
            ClearCache();
            SetNextMountingPoint();
        }

        rand = 2;
        newMapChunk = new ChunksMap(rand, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(chunks[rand]) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            _chunks.Insert(0, newChunk);
            ClearCache();
            SetNextMountingPoint();
        }

        rand = 4;
        newMapChunk = new ChunksMap(rand, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(chunks[rand]) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            _chunks.Insert(0, newChunk);
            ClearCache();
            SetNextMountingPoint();
        }

        rand = 2;
        newMapChunk = new ChunksMap(rand, origin, direction);
        if (map.Add(newMapChunk))
        {
            newChunk = Instantiate(chunks[rand]) as GameObject;
            newChunk.transform.parent = transform;
            newChunk.transform.position = mountTransform.position;
            newChunk.transform.Rotate(new Vector3(0, rotY, 0));
            newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
            newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
            _chunks.Insert(0, newChunk);
            ClearCache();
            SetNextMountingPoint();
        }

        do
        {
            do
            {
                rand = GetRandomChunk();
                newMapChunk = new ChunksMap(rand, origin, direction);
                added = map.Add(newMapChunk);
                if (cache.Count < chunks.Length && added)
                {
                    newChunk = Instantiate(chunks[rand]) as GameObject;
                    newChunk.transform.parent = transform;
                    newChunk.transform.position = mountTransform.position;
                    newChunk.transform.Rotate(new Vector3(0, rotY, 0));
                    newChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
                    newChunk.GetComponent<Chunks>().instantiatedDirection = direction;
                    _chunks.Add(newChunk);
                    ClearCache();

                    // Update the spawnRate of the choosen Chunk to disminish its spawn probabilities in the future
                    if (newChunk.GetComponent<Chunks>().type != ChunkType.Room)
                    {
                        UpdateSpawnRate(rand);
                    }

                    if (newChunk.GetComponent<Chunks>().type == ChunkType.Corridor)
                    {
                        if(Random.value <= roomSpawnRate)
                        {
                            mountTransform = newChunk.GetComponent<Chunks>().roomPoints[0];
                            origin = newChunk.GetComponent<Chunks>().chunkMap.roomPoints[0];
                            Direction tempDir = UpdateDirection(direction, mountTransform);
                            if (map.Add(new ChunksMap(5, origin, tempDir)))
                            {
                                GameObject tempChunk = Instantiate(chunks[5]) as GameObject;
                                tempChunk.transform.parent = transform;
                                tempChunk.transform.position = mountTransform.position;
                                tempChunk.transform.Rotate(new Vector3(0, UpdateRotation(tempDir), 0));
                                tempChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
                                tempChunk.GetComponent<Chunks>().instantiatedDirection = tempDir;
                                _chunks.Insert(0, tempChunk);
                            }
                        }
                        if (Random.value <= roomSpawnRate)
                        {
                            mountTransform = newChunk.GetComponent<Chunks>().roomPoints[1];
                            origin = newChunk.GetComponent<Chunks>().chunkMap.roomPoints[1];
                            Direction tempDir = UpdateDirection(direction, mountTransform);
                            if (map.Add(new ChunksMap(5, origin, tempDir)))
                            {
                                GameObject tempChunk = Instantiate(chunks[5]) as GameObject;
                                tempChunk.transform.parent = transform;
                                tempChunk.transform.position = mountTransform.position;
                                tempChunk.transform.Rotate(new Vector3(0, UpdateRotation(tempDir), 0));
                                tempChunk.GetComponent<Chunks>().chunkMap = newMapChunk;
                                tempChunk.GetComponent<Chunks>().instantiatedDirection = tempDir;
                                _chunks.Insert(0, tempChunk);
                            }
                        }
                            
                    }

                }
                else if (cache.Count == chunks.Length)
                {
                    ClearCache();
                    break;
                }
            } while (!added);
        } while (SetNextMountingPoint() && CountNumberOfRooms() < nbOfRooms); // You can add other conditions if you like

        // if one of the condition is not fullfiled then restart the process
        if (CountNumberOfRooms() < nbOfRooms)
        {
            SceneManager.LoadScene("SampleScene");
        }
        Debug.Log(_spawnRates[0] + " " +  _spawnRates[1] + " " + _spawnRates[2] + " " + _spawnRates[3] + " " + _spawnRates[4] + " " + _spawnRates[5]);
    }

    private int CountNumberOfRooms()
    {
        int count = 0;
        foreach (GameObject obj in _chunks)
        {
            if ( obj.GetComponent<Chunks>().type == ChunkType.Room)
            {
                count++;
            }
        }
        return count;
    }

    private void GenerateRoom(int id, MapCoordinates coords, Transform roomPoint, Direction dir)
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
        }
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
                    p[i].GetComponent<MountPoint>().available = false;
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
        Direction res = Direction.East; // careful with this initialization, maybe false in some cases
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
        for (int i = 0; i< chunks.Length; i++)
        {
            spawnRates.Add(chunks[i].GetComponent<Chunks>().initialSpawnRate);
        }
        return spawnRates;
    }
}
