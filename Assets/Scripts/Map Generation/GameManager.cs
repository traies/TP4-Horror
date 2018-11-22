using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public enum Difficulty { Easy, Normal, Hard };

public class GameManager : MonoBehaviour
{
    private LevelGenerator _levelGenerator;
    private ItemGenerator _itemGenerator;
    private EnemyGenerator _enemyGenerator;
    private Generator _generator;
    public Difficulty difficulty;
    public bool inGame;

    private void Start()
    {
        _levelGenerator = FindObjectOfType<LevelGenerator>();
        _itemGenerator = FindObjectOfType<ItemGenerator>();
        _enemyGenerator = FindObjectOfType<EnemyGenerator>();
        _generator = FindObjectOfType<Generator>();
        difficulty = CrossScenesData.Instance.difficulty;
        Debug.Log("Difficulty: " + difficulty);
        inGame = true;
        // GenerateLevel();
        GenerateItems();
        GenerateEnemies();
    }

    private void Update()
    {
        if (!inGame)
        {
            //GameOver
        }
    }

    [ContextMenu("Generate Level")]
    void GenerateLevel ()
    {
        _levelGenerator = FindObjectOfType<LevelGenerator>();
        _levelGenerator.GenerateMap();
	}

    void GenerateItems()
    {
        _generator.Generate(ObjectType.Item, _itemGenerator.items, difficulty, _itemGenerator.NbItemsEasy, _itemGenerator.NbItemsNormal,
            _itemGenerator.NbItemsHard, _itemGenerator.decreaseRate);
    }

    void GenerateEnemies()
    {
        switch(difficulty) {
            case Difficulty.Easy:
                _enemyGenerator.enemies[0] = _enemyGenerator.EasyZombiePrefab;
                break;
            case Difficulty.Normal:
                _enemyGenerator.enemies[0] = _enemyGenerator.NormalZombiePrefab;
                break;
            case Difficulty.Hard:
                _enemyGenerator.enemies[0] = _enemyGenerator.HardZombiePrefab;
                break;
        }
        _generator.Generate(ObjectType.Enemy, _enemyGenerator.enemies, difficulty, _enemyGenerator.NbItemsEasy, _enemyGenerator.NbItemsNormal,
            _enemyGenerator.NbItemsHard, _enemyGenerator.decreaseRate);
    }
}
