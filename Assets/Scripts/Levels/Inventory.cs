using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory
{
    public static Dictionary<string, bool> collectedItems = new() {
            { "Hammer", false},
            { "Lantern", false},
            { "Map", false},
            { "Explosive", false},
            //{ "Image (5)", false},
            { "Key0", false},
            { "Key1", false},
            { "Key2", false},
            { "Key3", false},
        };

    public static Dictionary<string, GameObject> Items = new() {
        { "Hammer", null},
        { "Lantern", null},
        { "Map", null},
        { "Explosive", null},
        //{ "Image (5)", null},
        { "Key0", null},
        { "Key1", null},
        { "Key2", null},
        { "Key3", null},
    };
    

    public static void CollectItem(string itemName)
    {
        collectedItems[itemName] = true;
        GameObject.Destroy(Items[itemName]);
        Items[itemName] = null;
    }
    public static void CollectKey(int key)
    {
        CollectItem($"Key{key}");
    }

    public static bool WasKeyCollected(int key)
    {
        return collectedItems[$"Key{key}"];
    }

    public static void ClearInventory()
    {
        foreach (string item in collectedItems.Keys.ToList()) {
            collectedItems[item] = false;
        }
    }

    public static void ClearKeys()
    {
        for (int i = 0; i < 4; i++) {
            collectedItems[$"Key{i}"] = false;
        }
    }
}

/* TODO:
 * P1:
 * add question sounds
 * 
 * P2:
 * add dark maze mask to block part of the maze
 * add levels & bosses
 * add puzzles
 */
