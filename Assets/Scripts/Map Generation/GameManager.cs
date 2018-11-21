using UnityEngine;

public enum Difficulty { Easy, Normal, Hard };

public class GameManager : MonoBehaviour
{
    private LevelGenerator _levelGenerator;
    private ItemGenerator _itemGenerator;
    private EnemyGenerator _enemyGenerator;
    private Generator _generator;
    public Difficulty difficulty;
    public bool inGame;


    [ContextMenu("Generate Level")]
    void GenerateLevel ()
    {
        _levelGenerator = FindObjectOfType<LevelGenerator>();
        _itemGenerator = FindObjectOfType<ItemGenerator>();
        _enemyGenerator = FindObjectOfType<EnemyGenerator>();
        _generator = FindObjectOfType<Generator>();
        _levelGenerator.GenerateMap();
	}

    [ContextMenu("Generate Items")]
    void GenerateItems()
    {
        _generator.Generate(ObjectType.Item, _itemGenerator.items, _itemGenerator.NbItemsEasy, _itemGenerator.NbItemsNormal,
            _itemGenerator.NbItemsHard, _itemGenerator.decreaseRate);
    }

    [ContextMenu("Generate Enemies")]
    void GenerateEnemies()
    {
        _generator.Generate(ObjectType.Enemy, _enemyGenerator.enemies, _enemyGenerator.NbItemsEasy, _enemyGenerator.NbItemsNormal,
            _enemyGenerator.NbItemsHard, _enemyGenerator.decreaseRate);
    }
}
