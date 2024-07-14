using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlidingNumbers : MonoBehaviour
{
    public GameObject keyTile;
    public GameObject[] tiles;
    
    int freeTileX, freeTileY;
    int[,] puzzle;

    private void Start()
    {
        freeTileX = 3;
        freeTileY = 0;
        puzzle = new int[4, 4] {
            {10, 9, 3, 0},
            {2, 5, 15, 7},
            {8, 14, 1, 11},
            {12, 13, 4, 6},
        };
        
        for (int i = 0; i < tiles.Length; i++) {
            TextMeshProUGUI buttonText = tiles[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = puzzle[i / 4, i % 4].ToString();
        }
        tiles[4 * freeTileY + freeTileX].SetActive(false);
    }

    public void OnClick(string index)
    {
        int i = int.Parse(index);
        int row = i / 4;
        int col = i % 4;

        // Only adjacent tiles can move to the open space
        if ((row == freeTileY && Math.Abs(col - freeTileX) == 1) ||
            (col == freeTileX && Math.Abs(row - freeTileY) == 1)) {

            int freeIndex = 4 * freeTileY + freeTileX;
            int tileIndex = 4 * row + col;

            // Update puzzle and button text
            TextMeshProUGUI buttonText = tiles[freeIndex].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = puzzle[row, col].ToString();
            puzzle[freeTileY, freeTileX] = puzzle[row, col];
            puzzle[row, col] = 0;
            freeTileX = col;
            freeTileY = row;
            
            tiles[freeIndex].SetActive(true);
            tiles[tileIndex].SetActive(false);
            if (CheckVictory()) {
                foreach (GameObject tile in tiles) {
                    tile.GetComponent<Button>().interactable = false;
                }
                keyTile.SetActive(true);
            }
        }
    }

    public void OnKeyClick()
    {
        if (GameProgress.mode == GameProgress.GameMode.MiniGame) {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        Inventory.CollectKey(GameProgress.activeKeyPuzzle);
        Destroy(gameObject);
    }

    bool CheckVictory()
    {
        int freeTile = 4 * freeTileY + freeTileX;
        for (int i = 0; i < 16; ++i) {
            if (i != freeTile && puzzle[i / 4, i % 4] != i + 1) {
                return false;
            }
        }
        return true;
    }
}
