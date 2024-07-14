using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour
{
    public string type;
    public int index;

    public Sprite[] sprites;
    public Sprite[] alternatives;
    

    // Start is called before the first frame update
    void Start()
    {
        if (index < sprites.Length) {
            GetComponent<SpriteRenderer>().sprite = sprites[index];
        }
    }

    public void SwitchSpriteToAlternative()
    {
        GetComponent<SpriteRenderer>().sprite = alternatives[index];
    }
}
