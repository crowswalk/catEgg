using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    private SpriteRenderer sprRenderer; //to access & change sprite renderer
    public int sorter; //variable determines sprite's order in layer
    private float ypos;

    void Start() {
        sprRenderer = GetComponent<SpriteRenderer>();
        sprRenderer.sortingOrder = sorter;
        sorter = -(int)(transform.position.y * 10.0);
    }

    void FixedUpdate() {
        ypos = transform.position.y;
        if (gameObject.tag == "Player") {
            ypos -= .05f;
        }
        sorter = -(int)(ypos * 10.0);
        sprRenderer.sortingOrder = sorter;
    }
}
