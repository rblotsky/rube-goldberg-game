using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingHologram : MonoBehaviour
{
    // DATA //
    // Cached References
    public SpriteRenderer holoRenderer;


    // FUNCTIONS //
    // Unity Defaults
    private void Awake()
    {
        holoRenderer = GetComponent<SpriteRenderer>();
    }


    // External Management
    public void ToggleHologram(bool status)
    {
        gameObject.SetActive(status);
    }
 
    public void UpdateSprite(Sprite newSprite)
    {
        holoRenderer.sprite = newSprite;
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void RotateClockwise(float incrementAmount)
    {
        transform.Rotate(Vector3.forward, incrementAmount);
    }
}
