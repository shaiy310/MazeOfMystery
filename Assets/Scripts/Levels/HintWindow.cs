using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class HintWindow : MonoBehaviour
{
    public static int hint = 0;

    public TextMeshProUGUI hintTXT;
    
    // Start is called before the first frame update
    void Start()
    {
        string path = GameProgress.GetResourcePath($"hint_{hint}");
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        if (textAsset) {
            hintTXT.text = textAsset.text;
        }
    }

    public void OnClick()
    {
        Destroy(gameObject);
    }
}
