using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossScenesData : MonoBehaviour
{
    public Difficulty difficulty;
    private static CrossScenesData _instance = null;

    public static CrossScenesData Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
