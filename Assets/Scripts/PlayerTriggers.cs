using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    SpriteRenderer sprRenderer; //Renderer of other object

    public GameObject thoughtBubble, speechBubble, chestBubble, neighborBubble, baitBubble;

    PlayerDialogue dialogue;

    int acornCount; //how many acorns the player has picked up
    int eggCount; //how many eggs player has picked up
    int fishCount;

    public bool hasKey, hasBag, hasRod, hasBait, hasCoal, inDialogue;
    public bool meow, gateLocked, needBait, neighborPermit;
    //meow: whether player has talked to cat in the beginning (explains the mission of the game)
    //gateLocked: after the player has talked to cat, this is set to true. when the player tries to go through the gate, they will see that it is locked.
    //needBait: determines whether the player realized they need bait
    //neighbortalked: whether the neighbor has given the player a hint about how to get bait

    void Start() {
        dialogue = this.GetComponent<PlayerDialogue>();
        inDialogue = false;

        acornCount = 0;
        eggCount = 0;
        fishCount = 0;

        hasKey = false;
        hasBag = false;
        hasRod = false;
        hasBait = false;
        hasCoal = false;

        meow = false;
        gateLocked = false;
        needBait = false;
        neighborPermit = false;
    }

    void Update() {
        Debug.Log("Egg Count: " + eggCount);
        Debug.Log("Acorn Count: " + acornCount);
    }

    void OnTriggerStay2D(Collider2D other) {
        GameObject otherObj = other.gameObject;
        sprRenderer = otherObj.GetComponent<SpriteRenderer>();
        if (otherObj.tag == "Bubble") { //Hint will be shown when player is close
                if (otherObj.name == "ChestContents") { //if the Bubble is for Bag and player already got it
                    if (hasBag && hasRod) {
                        sprRenderer.enabled = false;
                        chestBubble.SetActive(false);
                    } else {
                        chestBubble.SetActive(true);
                    }

                } else {
                    sprRenderer.enabled = true; //show Bubble
                }
        }

        if(otherObj.name == "CatBubble") { //near cat
            if (!meow && Input.GetKey(KeyCode.F)) { //pressing f to talk to cat at the beginning
                meow = true;
                inDialogue = true;
                speechBubble.SetActive(true);
                dialogue.currentSpeech = dialogue.catTalk;
                dialogue.speechIndex = 0;
                sprRenderer.enabled = false; //disable cat hint when talking to it
                
            } else if (meow && !hasKey && gateLocked && Input.GetKey(KeyCode.F) && !inDialogue) { //after player sees that the gate is locked
                hasKey = true;
                inDialogue = true;
                speechBubble.SetActive(true);
                dialogue.currentSpeech = dialogue.catTalk2;
                dialogue.speechIndex = 0;
                sprRenderer.enabled = false;

            } else if (hasKey){
                GameObject Cat = otherObj.transform.parent.gameObject;
                Cat.GetComponent<SpriteRenderer>().sprite = Cat.GetComponent<CatState>().noKeySprite;
            }
        }

        if(otherObj.name == "NeighborBubble") {
            NeighborDialogue neighbor = otherObj.transform.parent.gameObject.GetComponent<NeighborDialogue>();

            if (Input.GetKey(KeyCode.F) && !inDialogue) { //pressing f to talk to neighbor
                inDialogue = true;
                neighborBubble.SetActive(true);
                neighbor.speechIndex = 0;
                if (needBait) { //after you try to fish without bait, player gives hint that you need eggs for bait
                    neighbor.currentSpeech = neighbor.eggHint;
                }
                if (eggCount > 0 && !hasBait && !neighborPermit) { //if you have eggs but no coal, neighbor will give you permission to use coal
                    neighborPermit = true;
                    neighbor.currentSpeech = neighbor.permission;
                }
            }
        }
     }

    void OnTriggerEnter2D(Collider2D other) {
        GameObject otherObj = other.gameObject;
        sprRenderer = otherObj.GetComponent<SpriteRenderer>();
        if (otherObj.tag == "Acorn") {
            if (hasBag) { //pick up the acorn if the player has bag
                thoughtBubble.SetActive(false);
                acornCount++;
                Destroy(otherObj);
            } else {
                thoughtBubble.SetActive(true);
                dialogue.currentThought = dialogue.acornHint;
                dialogue.thoughtIndex = 0;
            }
        }
        if(otherObj.name == "HoldRod") {
            if(hasRod) {
                if (!hasBait) {
                    needBait = true;
                    speechBubble.SetActive(true);
                    dialogue.currentSpeech = dialogue.baitHint;
                    dialogue.speechIndex = 0;
                }
            } else {
                thoughtBubble.SetActive(true);
                dialogue.currentThought = dialogue.rodHint;
                dialogue.thoughtIndex = 0;
            }
        }
        if (otherObj.tag == "Egg") {
            sprRenderer.enabled = false;
            eggCount++;
            Destroy(otherObj);
        }
        if (otherObj.tag == "FishCollider" && hasBait) {
            fishCount++;
            Destroy(otherObj.transform.parent.gameObject);
        }
     }

     void OnTriggerExit2D(Collider2D other) {
        GameObject otherObj = other.gameObject;
        sprRenderer = otherObj.GetComponent<SpriteRenderer>();
        if (otherObj.tag == "Bubble") { //Bubble stops showing when player leaves
            sprRenderer.enabled = false;
        }
        speechBubble.SetActive(false);
        thoughtBubble.SetActive(false);
        chestBubble.SetActive(false);
        neighborBubble.SetActive(false);
     }

    void OnCollisionEnter2D(Collision2D other) {
         GameObject otherObj = other.gameObject;
         sprRenderer = otherObj.GetComponent<SpriteRenderer>();
         if (otherObj.name == "GateDoor") { //colliding w/gate door before it's destroyed
             if(!hasKey && meow && !gateLocked) { //before the player has the key, after the player has talked to the cat once
                gateLocked = true;
                speechBubble.SetActive(true);
                dialogue.speechIndex = 0;
                dialogue.currentSpeech = dialogue.gateHint;
                inDialogue = true;
             } else if(hasKey) {
                 Destroy(otherObj);
             }
         }
         if (otherObj.tag == "Chicken") {
            ChickenState thisChicken = otherObj.GetComponent<ChickenState>();
            thisChicken.chickenDir = 0;
            if (acornCount > 0) { //colliding w/chickens
                if (thisChicken.currentState == ChickenState.objState.hungry) {
                    acornCount--;
                    thisChicken.currentState = ChickenState.objState.happy;
                }
            }
         }

        if (otherObj.name == "Cat") { //colliding w/cat
            if (eggCount > 0) {
                CatState thisCat = otherObj.GetComponent<CatState>();
                thisCat.currentState = CatState.objState.happy;
            }
        }
        if (otherObj.name == "CoalBag" && neighborPermit) {
            hasCoal = true;
            Debug.Log("Has Coal");
        }
        if (otherObj.name == "Furnace" && hasCoal) {
            if(eggCount >= 3) {
                baitBubble.SetActive(true);
            } else {
                thoughtBubble.SetActive(true);
                dialogue.currentThought = dialogue.eggHint;
                dialogue.thoughtIndex = 0;
            }
            
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
