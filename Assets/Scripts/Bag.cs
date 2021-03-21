using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour {
    public PlayerTriggers player;
    public Color hoverColor;

    SpriteRenderer renderer;
    Color originalColor;

    public ChestState chest;

    void Start() {
        renderer = this.GetComponent<SpriteRenderer>();
        originalColor = Color.white;
    }

    void OnMouseOver() {
        renderer.color = hoverColor;
        chest.currentSprite = chest.chestSprites[1];
        if (Input.GetMouseButtonDown(0)) {
            player.hasBag = true;
            renderer.enabled = false;
            chest.currentSprite = chest.chestSprites[0];
            Destroy(this);
        }
    }

    void OnMouseExit() {
        renderer.color = originalColor;
        chest.currentSprite = chest.chestSprites[0];
    }
}
