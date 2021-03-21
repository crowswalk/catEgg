using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestState : MonoBehaviour
{
    public Sprite[] chestSprites = new Sprite[2];
    SpriteRenderer sprRenderer;
    public Sprite currentSprite;

    void Start() {
        sprRenderer = GetComponent<SpriteRenderer>();
        currentSprite = chestSprites[0];
    }

    void Update() {
        sprRenderer.sprite = currentSprite;
    }
}
