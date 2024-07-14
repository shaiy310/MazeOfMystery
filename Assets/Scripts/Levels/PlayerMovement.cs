using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UISystemProfilerApi;

public class PlayerMovement : MonoBehaviour
{   
    public float speed = 0.05f;
    public Vector2 direction = Vector2.zero;
    public GameObject sampleTile;
    public GameObject hintWindow;
    public GameObject bossWindow;
    public GameObject pauseWindow;
    public GameObject inventoryWindow;
    public GameObject interactionSignal;
    public TextMeshProUGUI notification;
    public GameObject map;

    Rigidbody2D rigidBody;
    AudioSource audioSource;
    Animator animator;
    Vector3 tileSize;

    GameObject collidedObject;
    GameObject interactiveWindow;

    // Start is called before the first frame update
    void Start()
    {
        interactiveWindow = null;
        rigidBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);
        animator.SetBool("Left", false);
        animator.SetBool("Right", false);

        tileSize = sampleTile.GetComponent<Renderer>().bounds.size;
        map.SetActive(Inventory.collectedItems["Map"]);
    }

    // Update is called once per frame
    void Update()
    {
        if (interactiveWindow) {
            // Stop background sound while there is an interactive window over the game.
            if (audioSource.isPlaying) {
                audioSource.Pause();
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                Destroy(interactiveWindow);
                interactiveWindow = null;
            }
        } else {
            // Restore the sound when interactive window is closed.
            if (!audioSource.isPlaying) {
                audioSource.UnPause();
            }

            MazeMovement();
        }
    }

    void MazeMovement()
    { 
        // Up
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            direction.y = tileSize.y;
            animator.SetBool("Up", true);
        } else if (Input.GetKeyUp(KeyCode.UpArrow)) {
            // In case DownArrow was pressed before releasing the UpArrow - Don't clear it.
            if (direction.y == tileSize.y) {
                direction.y = 0;
            }
            animator.SetBool("Up", false);
        
        // Down
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            direction.y = -tileSize.y;
            animator.SetBool("Down", true);
        } else if (Input.GetKeyUp(KeyCode.DownArrow)) {
            // In case UpArrow was pressed before releasing the DownArrow - Don't clear it.
            if (direction.y == -tileSize.y) {
                direction.y = 0;
            }
            animator.SetBool("Down", false);

        // Right
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            direction.x = tileSize.x;
            animator.SetBool("Right", true);
        } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
            // In case LeftArrow was pressed before releasing the RightArrow - Don't clear it.
            if (direction.x == tileSize.x) {
                direction.x = 0;
            }
            animator.SetBool("Right", false);

        // Left
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            direction.x = -tileSize.x;
            animator.SetBool("Left", true);
        } else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            // In case RightArrow was pressed before releasing the LeftArrow - Don't clear it.
            if (direction.x == -tileSize.x) {
                direction.x = 0;
            }
            animator.SetBool("Left", false);

        // Interaction
        } else if (Input.GetKeyDown(KeyCode.E)) {
            Interaction();

        // Pause
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            interactiveWindow = Instantiate(pauseWindow);

        // Inventory
        } else if (Input.GetKeyDown(KeyCode.I)) {
            interactiveWindow = Instantiate(inventoryWindow);
        }

        rigidBody.velocity = speed * direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door")) {
            Properties doorProperties = collision.gameObject.GetComponent<Properties>();
            if (Inventory.WasKeyCollected(doorProperties.index)) {
                doorProperties.SwitchSpriteToAlternative();
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        } else if (collision.gameObject.CompareTag("Boss")) {
            ShowBoss();
        } else if (collision.gameObject.CompareTag("Stairs")) {
            GameProgress.GotoNextLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactionSignal.SetActive(true);
        collidedObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactionSignal.SetActive(false);
        collidedObject = null;
    }

    public void Interaction()
    {
        Properties properties = collidedObject.GetComponent<Properties>();
        switch (collidedObject.tag) {
            case "Key":
                ShowPuzzle(properties.index);
                break;
            case "Hint":
                ShowHint(properties.index);
                break;
            case "Artifact":
                GameProgress.CollectArtifact(properties.index);
                Destroy(collidedObject);
                //collidedObject = null;
                break;

            case "CheckPoint":
                StartCoroutine(Notify("Check Point", 1.5f));
                GameProgress.SetCheckPoint();
                break;

            case "Explosive":
            case "Hammer":
                Inventory.CollectItem(collidedObject.tag);
                break;

            case "Map":
                Inventory.CollectItem("Map");
                map.SetActive(true);
                break;

            case "Lantern":
                Inventory.CollectItem("Lantern");
                // activate lantern effect
                break;
        }
    }

    void ShowPuzzle(int key)
    {
        GameProgress.activeKeyPuzzle = key;

        string prefabPath = GameProgress.GetPuzzleResource(key);
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab != null) {
            interactiveWindow = Instantiate(prefab);
        } else {
            Debug.LogError($"Prefab with name '{prefabPath}' not found.");
            // In case there isn't a prefab, collect the key without a puzzle.
            Inventory.CollectKey(key);
        }
    }

    void ShowHint(int hint)
    {
        GameProgress.activeHint = hint;
        interactiveWindow = Instantiate(hintWindow);
    }

    void ShowBoss()
    {
        interactiveWindow = Instantiate(bossWindow);
    }

    IEnumerator Notify(string text, float duration)
    {
        notification.text = text;

        yield return new WaitForSeconds(duration);

        notification.text = "";
    }
}
