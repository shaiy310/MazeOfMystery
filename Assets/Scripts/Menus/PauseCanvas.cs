using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCanvas : MonoBehaviour
{
    public void OnPauseClick(string button)
    {
        if (button == "Yes") {
            ExitToMenu();
        } else {
            Destroy(gameObject);
        }
    }
    private void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
