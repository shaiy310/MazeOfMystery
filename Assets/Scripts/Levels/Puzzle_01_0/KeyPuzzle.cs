using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyPuzzle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick(string button)
    {
        if (GameProgress.mode == GameProgress.GameMode.MiniGame) {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        if (button == "Red") {
            Inventory.CollectKey(GameProgress.activeKeyPuzzle);
        }

        Destroy(gameObject);
    }

    public void OnCloseClick()
    {
        if (GameProgress.mode == GameProgress.GameMode.MiniGame) {
            return;
        }

        Destroy(gameObject);
    }
}
