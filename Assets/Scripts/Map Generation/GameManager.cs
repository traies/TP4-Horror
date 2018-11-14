using UnityEngine;

public enum Difficulty { Easy, Normal, Hard };

public class GameManager : MonoBehaviour
{
    private LevelGenerator _levelGenerator;
    private ItemGenerator _itemGenerator;
    public Difficulty difficulty;

    void Start ()
    {
        _levelGenerator = FindObjectOfType<LevelGenerator>();
        _itemGenerator = FindObjectOfType<ItemGenerator>();
        _levelGenerator.Test2();
        _itemGenerator.GenerateItems();
	}
}
