using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEngine.UISystemProfilerApi;

public class LoadLevel : MonoBehaviour
{
    public float speed = 0.05f;
    public GameObject player;
    
    public GameObject wallTile, stairsTile, doorTile, keyTile, hintTile;
    public GameObject mapTile, lanternTile, hammerTile, explosiveTile;
    public GameObject checkPointTile, artifactTile, bossTile;

    Vector3 tileSize;
    Vector2 mapSize;

    void Awake()
    {
        print($"Loading level: {GameProgress.level}");
        GameProgress.mode = GameProgress.GameMode.Campaign;
        Inventory.ClearKeys();

        tileSize = wallTile.GetComponent<Renderer>().bounds.size;
        
        char[,] map = LoadMaze(GameProgress.GetResourcePath("Maze"));
        mapSize = new Vector2(map.GetLength(1), map.GetLength(0));
        CreateTiles(map);
    }

    char[,] LoadMaze(string filePath)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        string[] lines = textAsset.text.Split("\r\n");

        char[,] map = new char[lines.Length, lines[0].Length];
        
        for (int r = 0; r < map.GetLength(0); ++r) {
            for (int c = 0; c < map.GetLength(1); ++c) {
                map[r, c] = lines[r][c];
            }
        }

        return map;
    }

    /// <summary>
    /// Create tiles in scene according to map.<br/>
    /// Codex:
    /// <br/>* #   => Wall
    /// <br/>* A-D => Doors
    /// <br/>* a-d => Keys
    /// <br/>* ?   => Hints - Canceled (?)
    /// 
    /// <br/>* S   => Starting point
    /// <br/>* E   => End point (Stairs)
    /// 
    /// <br/>* M   => Map
    /// <br/>* L   => Lantern
    /// <br/>* H   => Hammer
    /// <br/>* !   => Explosive
    /// 
    /// <br/>* P   => Check point
    /// <br/>* 1-8 => Artifact
    /// <br/>* X   => Boss
    /// 
    /// </summary>
    /// <param name="map">ascii map of the maze.</param>
    void CreateTiles(char[,] map)
    {
        int index;
        for (int r = 0; r < mapSize.y; ++r) {
            for (int c = 0; c < mapSize.x; ++c) {
                switch (map[r, c]) {
                    case '#':
                        CreateTile(wallTile, r, c);
                        break;

                    case char ch when 'A' <= ch && ch <= 'D':   // Doors
                        index = ch - 'A';
                        CreateTile(doorTile, r, c, index);
                        break;

                    case char ch when 'a' <= ch && ch <= 'd':   // Keys
                        index = ch - 'a';
                        Inventory.Items[$"Key{index}"] = CreateTile(keyTile, r, c, index);
                        break;

                    case '?':   // Hints
                        index = 0;
                        CreateTile(hintTile, r, c, index);
                        break;

                    case char ch when '1' <= ch && ch <= '8':   // Artifacts
                        index = ch - '1';
                        if (!GameProgress.WasArtifactCollected(index)) {
                            CreateTile(artifactTile, r, c, index);
                        }
                        break;

                    case 'S':
                        player.transform.position = new Vector3(c * tileSize.x, (mapSize.y - r) * tileSize.y, 0);
                        break;

                    case 'E':
                        CreateTile(stairsTile, r, c);
                        break;

                    case 'M':
                        Inventory.Items["Map"] = CreateTile(mapTile, r, c);
                        break;
                    
                    case 'L':
                        Inventory.Items["Lantern"] = CreateTile(lanternTile, r, c);
                        break;

                    case 'H':
                        Inventory.Items["Hammer"] = CreateTile(hammerTile, r, c);
                        break;

                    case '!':
                        Inventory.Items["Explosive"] = CreateTile(explosiveTile, r, c);
                        break;

                    case 'P':
                        CreateTile(checkPointTile, r, c);
                        break;

                    case 'X':
                        CreateTile(bossTile, r, c);
                        break;
                }
            }
        }
    }

    GameObject CreateTile(GameObject sampleTile, int row, int col)
    {
        GameObject dummy = Instantiate(sampleTile,
                    new Vector3(col * tileSize.x, (mapSize.y - row) * tileSize.y, 0),
                    transform.rotation,
                    transform
                );
        dummy.name = $"{sampleTile.name}_{row}x{col}";

        return dummy;
    }
    GameObject CreateTile(GameObject sampleTile, int row, int col, int index)
    {
        GameObject dummy = CreateTile(sampleTile, row, col);
        dummy.GetComponent<Properties>().index = index;

        return dummy;
    }
}
