using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bait : MonoBehaviour
{
    public PlayerTriggers player;
    public Color hoverColor;

    SpriteRenderer renderer;
    Color originalColor;

    void Start() {
        renderer = this.GetComponent<SpriteRenderer>();
        originalColor = Color.white;
    }

    void OnMouseOver() {
        renderer.color = hoverColor;
        if (Input.GetMouseButtonDown(0)) {
            player.hasBait = true;
            renderer.enabled = false;
            Destroy(this.transform.parent.gameObject);
        }
    }

    void OnMouseExit() {
        renderer.color = originalColor;
    }
}
