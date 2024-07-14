using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MemoryPuzzle : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject key;
    public Sprite emptyTileSprite;
    public Sprite[] sprites;

    GameObject[,] tiles;
    int[] numbers;
    int selectedX, selectedY, matchedPairs;

    // Start is called before the first frame update
    void Start()
    {
        selectedX = selectedY = -1;
        matchedPairs = 0;

        tiles = new GameObject[4, 10];
        ShuffleTiles();
        GenerateTiles();
    }

    void ShuffleTiles()
    {
        System.Random random = new();
        numbers = Enumerable.Range(0, 20).SelectMany(n => Enumerable.Repeat(n, 2)).ToArray();
        for (int i = 0; i < numbers.Length; i++) {
            int randomIndex = random.Next(i, numbers.Length);
            (numbers[randomIndex], numbers[i]) = (numbers[i], numbers[randomIndex]);
        }
    }

    void GenerateTiles()
    {
        // 10 tiles per row, with 9 spaces of 10 between them.
        // 4 tiles per column, with 3 spaces of 10 between them.
        // One tile less in each dimention cause the posistion is calculated from the center of a tile.
        // Therefore, need to decrease half a tile from each side.
        Vector2 offset = -new Vector2(55 * 9, 70 * 3) / 2;
        for (int row = 0; row < tiles.GetLength(0); ++row) {
            for (int col = 0; col < tiles.GetLength(1); ++col) {

                Vector2 pos = offset + new Vector2(55 * col, 70 * row);
                tiles[row, col] = Instantiate(tilePrefab, transform.TransformPoint(pos), Quaternion.identity, transform);
                tiles[row, col].name = $"{row}_{col}";

                // create local copies of row and col
                int localRow = row, localCol = col;
                tiles[row, col].GetComponent<Button>().onClick.AddListener(() => OnTileClick(localRow, localCol));
            }
        }
    }

    void OnTileClick(int row, int col)
    {
        int index = tiles.GetLength(1) * row + col;
        tiles[row, col].GetComponent<Image>().sprite = sprites[numbers[index]];
        
        if (selectedX == -1) {
            selectedX = col;
            selectedY = row;
        } else {
            int SelectedIndex = tiles.GetLength(1) * selectedY + selectedX;
            // if the same tile is clicked twice, ignore the second click.
            if (index == SelectedIndex) {
                return;
            }

            GameObject tileA = tiles[row, col];
            GameObject tileB = tiles[selectedY, selectedX];

            if (numbers[SelectedIndex] == numbers[index]) {
                // Pair matches
                tileA.GetComponent<Button>().interactable = false;
                tileB.GetComponent<Button>().interactable = false;
                ++matchedPairs;

                // Check win
                if (matchedPairs == sprites.Length) {
                    key.SetActive(true);
                    key.transform.SetAsLastSibling();
                }
            } else {
                // Pair doesn't match
                StartCoroutine(FlipTiles(tileA, tileB));
            }

            selectedX = selectedY = -1;
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

    IEnumerator FlipTiles(GameObject tileA, GameObject tileB)
    {
        yield return new WaitForSeconds(1);

        tileA.GetComponent<Image>().sprite = emptyTileSprite;
        tileB.GetComponent<Image>().sprite = emptyTileSprite;
    }
}
