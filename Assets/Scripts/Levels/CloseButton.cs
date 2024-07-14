using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseButton : MonoBehaviour
{
    public void OnCloseClick()
    {
        if (GameProgress.mode == GameProgress.GameMode.MiniGame) {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        if (Input.GetKey(KeyCode.LeftControl)) {
            // Cheats are awesome
            Inventory.CollectKey(GameProgress.activeKeyPuzzle);
        }
        Destroy(transform.parent.gameObject);
    }
}
