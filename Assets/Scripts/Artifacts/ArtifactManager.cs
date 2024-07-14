using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArtifactManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (KeyValuePair<string, bool> artifact in GameProgress.LocatedArtifact) {
            if (!artifact.Value) {
                GameObject.Find(artifact.Key).SetActive(artifact.Value);
            }
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
