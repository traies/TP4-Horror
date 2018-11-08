using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LevelGenerator levelGenerator;
    private ItemGenerator itemGenerator;

    void Start ()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        itemGenerator = FindObjectOfType<ItemGenerator>();
        //levelGenerator.Test();
        levelGenerator.Test2();
        //itemGenerator.GenerateItems();
	}
}
