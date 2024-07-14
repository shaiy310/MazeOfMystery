using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinGameMenu : MonoBehaviour
{
    public TMP_Dropdown menu;

    private void Start()
    {
        GameObject[] puzzles = Resources.LoadAll<GameObject>("Prefabs/Puzzles");
        foreach (var puzzle in puzzles) {
            menu.options.Add(new TMP_Dropdown.OptionData(puzzle.name));
        }
    }
    public void OnStartClick()
    {
        LoadMiniGame.puzzleName = menu.options[menu.value].text;
        SceneManager.LoadScene("MiniGame");
    }
}
