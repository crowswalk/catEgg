using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    SpriteRenderer sprRenderer; //Renderer of other object
    SpriteRenderer thoughtRenderer; //Renderer of player's thought bubble

    GameObject thoughtBubble;

    bool hasBag; //whether the player has picked up acorn-carrying bag
    int acornCount; //how many acorns the player has picked up
    int eggCount; //how many eggs player has picked up

    void Start() {
        thoughtBubble = transform.GetChild(0).gameObject;
        thoughtRenderer = thoughtBubble.GetComponent<SpriteRenderer>();
        hasBag = false;
        acornCount = 0;
    }

    void OnTriggerStay2D(Collider2D other) {
        GameObject otherObj = other.gameObject;
        sprRenderer = otherObj.GetComponent<SpriteRenderer>();
        if (otherObj.tag == "Bubble") { //Hint will be shown when player is close
                if (otherObj.name == "BagBubble" && hasBag) { //if the Bubble is for Bag and player already got it
                    sprRenderer.enabled = false;
                } else {
                    sprRenderer.enabled = true; //show Bubble
                }
        }
     }

    void OnTriggerEnter2D(Collider2D other) {
        GameObject otherObj = other.gameObject;
        sprRenderer = otherObj.GetComponent<SpriteRenderer>();
            if (otherObj.tag == "Acorn") {
                if (hasBag) { //pick up the acorn if the player has bag
                    sprRenderer.enabled = false;
                    acornCount++;
                    Destroy(otherObj);
                } else {
                    thoughtRenderer.enabled = true;
                }
            }
            if (otherObj.tag == "Egg") {
                sprRenderer.enabled = false;
                eggCount++;
                Destroy(otherObj);
            }
     }

     void OnTriggerExit2D(Collider2D other) {
        GameObject otherObj = other.gameObject;
        sprRenderer = otherObj.GetComponent<SpriteRenderer>();
        if (otherObj.tag == "Bubble") { //Bubble stops showing when player leaves
            sprRenderer.enabled = false;
        }
        if (otherObj.tag == "Acorn") {
            thoughtRenderer.enabled = false;
        }
     }

    void OnCollisionEnter2D(Collision2D other) {
         GameObject otherObj = other.gameObject;
         sprRenderer = otherObj.GetComponent<SpriteRenderer>();
        if (otherObj.name == "Chest") { //touching the chest opens it
            hasBag = true;
            ChestState thisChest = otherObj.GetComponent<ChestState>();
            thisChest.currentSprite = thisChest.chestSprites[1];
        }
        if (otherObj.tag == "Chicken" && acornCount > 0) {
            ChickenState thisChicken = otherObj.GetComponent<ChickenState>();
            thisChicken.currentState = ChickenState.objState.happy;
            acornCount--;
        }
        if (otherObj.name == "Cat" && eggCount > 0) {
            CatState thisCat = otherObj.GetComponent<CatState>();
            thisCat.currentState = CatState.objState.happy;
        }
     }

    void OnCollisionExit2D(Collision2D other) {
         GameObject otherObj = other.gameObject;
         sprRenderer = otherObj.GetComponent<SpriteRenderer>();
         if (otherObj.name == "Chest") { //leaving the chest closes it
            ChestState thisChest = otherObj.GetComponent<ChestState>();
            thisChest.currentSprite = thisChest.chestSprites[0];
         }
     }

}
