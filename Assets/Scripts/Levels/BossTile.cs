using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sprite boss = Resources.Load<Sprite>(GameProgress.GetResourcePath("BossTile"));
        GetComponent<SpriteRenderer>().sprite = boss;
    }
}
