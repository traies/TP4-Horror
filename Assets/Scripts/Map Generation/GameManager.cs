using UnityEngine;

public enum Difficulty { Easy, Normal, Hard };

public class GameManager : MonoBehaviour
{
    private LevelGenerator _levelGenerator;
    private ItemGenerator _itemGenerator;
    public Difficulty difficulty;


    [ContextMenu("Generate Level")]
    void generateLevel ()
    {
        _levelGenerator = FindObjectOfType<LevelGenerator>();
        _itemGenerator = FindObjectOfType<ItemGenerator>();
        _levelGenerator.GenerateMap();
        _itemGenerator.GenerateItems();
	}
}
