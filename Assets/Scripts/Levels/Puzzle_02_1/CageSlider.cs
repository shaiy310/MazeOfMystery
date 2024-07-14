using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CageSlider : MonoBehaviour
{
    public GameObject puzzleCanvas;
    public Button key;
    public Slider[] allSliders;

    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void OnCageValueChange(float value)
    {
        if (value == slider.maxValue) {
            key.interactable = true;
            foreach (var item in allSliders) {
                item.interactable = false;
            }
        }
    }

    public void OnKeyClick()
    {
        if (GameProgress.mode == GameProgress.GameMode.MiniGame) {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        Inventory.CollectKey(GameProgress.activeKeyPuzzle);
        Destroy(puzzleCanvas);
    }
}
