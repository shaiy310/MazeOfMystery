using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgress
{
    public enum GameMode { Campaign, MiniGame};
    
    public static GameMode mode;
    public static int level = 0;
    public static int checkPointLevel = 0;
    public static int activeKeyPuzzle = -1;
    public static int activeHint = -1;

    private static readonly Dictionary<string, bool> locatedArtifact = new() {
        {"Scarab", false},
        {"EyeOfHorus", false},
        {"AnubisSarcophagus", false},
        {"Vase", false},
        {"Minotaur", false},
        {"Medusa", false},
        {"Lamp", false},
        {"Skull", false},
    };

    public static Dictionary<string, bool> LocatedArtifact
    {
        get { return locatedArtifact; }
    }

    public static void GotoNextLevel()
    {
        ++GameProgress.level;
        SceneManager.LoadScene("Game");
    }

    public static void GotoCheckPointLevel()
    {
        GameProgress.level = GameProgress.checkPointLevel;
        SceneManager.LoadScene("Game");
    }

    public static void SetCheckPoint()
    {
        GameProgress.checkPointLevel = GameProgress.level;
        PlayerPrefs.SetInt("CheckPoint", GameProgress.checkPointLevel);
    }

    public static string GetResourcePath(string resourceName)
    {
        return $"Levels/{GameProgress.level:D2}/{resourceName}";
    }
    public static string GetPuzzleResource(int key)
    {
        return $"Prefabs/Puzzles/KeyPuzzle_{GameProgress.level:D2}_{key}";
    }

    public static void CollectArtifact(string artifact)
    {
        if (artifact == "") {
            return;
        }

        locatedArtifact[artifact] = true;
    }

    public static void CollectArtifact(int index)
    {
        string artifact = ArtifactIndexToName(index);

        CollectArtifact(artifact);
    }

    public static bool WasArtifactCollected(string artifact)
    {
        return locatedArtifact[artifact];
    }
    public static bool WasArtifactCollected(int index)
    {
        return locatedArtifact[ArtifactIndexToName(index)];
    }

    public static string ArtifactIndexToName(int index)
    {
        return (index + 1) switch {
            1 => "EyeOfHorus",
            2 => "Scarab",
            3 => "AnubisSarcophagus",

            5 => "Vase",
            6 => "Minotaur",
            7 => "Medusa",

            4 => "Lamp",
            8 => "Skull",
            _ => "",
        };
    }
}
