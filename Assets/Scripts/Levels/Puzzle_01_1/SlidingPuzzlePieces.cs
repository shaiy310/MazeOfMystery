using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlidingPuzzlePieces : MonoBehaviour, IDragHandler, IEndDragHandler
{
    readonly Vector2 redTarget = new(200, 25);
    
    public GameObject key;
    public RectTransform frame;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Vector2 gridOffset;
    private Rect frameRect;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        gridOffset = new Vector2(originalPosition.x % 50, originalPosition.y % 50);
        
        frameRect = GetRect(frame);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 v = eventData.delta;
        if (gameObject.name[0] == 'V') {
            v.x = 0;
        } else {
            v.y = 0;
        }
        rectTransform.anchoredPosition += v;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Get the RectTransform of the current object
        ToFixedGrid(rectTransform);
        Rect myRect = GetRect(rectTransform);

        if (gameObject.name == "Red") {
            if (rectTransform.anchoredPosition == redTarget) {
                key.SetActive(true);
                return;
            }
        }

        if (IsOutsideBox(myRect) || IsColliding(myRect)) {
            // If overlap is detected, snap back to original position
            rectTransform.anchoredPosition = originalPosition;
        } else {
            originalPosition = rectTransform.anchoredPosition;
        }
    }

    public void OnKeyClick()
    {
        if (GameProgress.mode == GameProgress.GameMode.MiniGame) {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        Inventory.CollectKey(GameProgress.activeKeyPuzzle);
        Destroy(transform.parent.gameObject);
    }

    bool IsOutsideBox(Rect rect)
    {
        return !(frameRect.Contains(rect.min) && frameRect.Contains(rect.max));
    }

    bool IsColliding(Rect rect)
    {
        // Check for overlap with other images
        RectTransform[] allRectTransforms = transform.parent.GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform otherRectTransform in allRectTransforms) {
            // Skip if it's the same RectTransform
            if (otherRectTransform == rectTransform)
                continue;

            // Ignore the Canvas and the frame's background
            if (otherRectTransform.name.StartsWith("KeyPuzzle") || otherRectTransform.name == "Box") {
                //print($"ignore collision with: {otherRectTransform.name}");
                continue;
            }

            // Get the Rect of the other RectTransform
            Rect otherRect = GetRect(otherRectTransform);

            // Check for overlap between the two Rects
            if (rect.Overlaps(otherRect)) {
                print($"colliding with: {otherRectTransform.name}");
                return true;
            }
        }

        return false;
    }

    void ToFixedGrid(RectTransform rect)
    {
        Vector2 v = math.round((rect.anchoredPosition - gridOffset ) / 50);
        rect.anchoredPosition = 50 * v + gridOffset;
    }

    Rect GetRect(RectTransform rect)
    {
        return new Rect(rect.position.x - rect.rect.width / 2,
                        rect.position.y - rect.rect.height / 2,
                        rect.rect.width,
                        rect.rect.height);
    }
}
