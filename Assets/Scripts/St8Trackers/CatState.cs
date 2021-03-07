using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatState : MonoBehaviour
{
    public GameObject bubble;
    public Sprite[] bubbleSprites = new Sprite[2];
    public enum objState {hungry, happy};
    public objState currentState;

    void Start() {
        currentState = objState.hungry;
        bubble.GetComponent<SpriteRenderer>().sprite = bubbleSprites[0];
    }

    void Update() {
        switch (currentState) {
            case objState.hungry:
                bubble.GetComponent<SpriteRenderer>().sprite = bubbleSprites[0];
                break;
            case objState.happy:
                bubble.GetComponent<SpriteRenderer>().sprite = bubbleSprites[1];
                break;
            default:
                break;
            }
        }
}

