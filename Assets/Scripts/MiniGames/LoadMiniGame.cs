using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMiniGame : MonoBehaviour
{
    public static string puzzleName = "";

    void Awake()
    {
        GameProgress.mode = GameProgress.GameMode.MiniGame;

        if (puzzleName != "") {
            string prefabPath = $"Prefabs/Puzzles/{puzzleName}";
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            Instantiate(prefab);
        }
    }
}
