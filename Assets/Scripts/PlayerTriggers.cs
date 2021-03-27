using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerTriggers : MonoBehaviour
{
    SpriteRenderer sprRenderer; //Renderer of other object
    public GameObject thoughtBubble, speechBubble, chestBubble, neighborBubble, baitBubble;
    PlayerDialogue dialogue;
    int acornCount, eggCount, fishCount;
    public bool inDialogue, hasRod, hasBag, hasCoal, fishScare, fishLike;    

    public enum playerState {
        catHungry, gateLocked, needKey, hasKey, needRod, needBait, needBag, permission, hasBait
    }

    public playerState currentState;
    public Texture2D cursor;
    public Texture2D hand;

    void Start() {
        dialogue = this.GetComponent<PlayerDialogue>();
        inDialogue = false;
        acornCount = 0;
        eggCount = 0;
        fishCount = 0;
        hasRod = false;
        hasBag = false;
        hasCoal = false;
        fishScare = false;
        fishLike = false;
        currentState = playerState.catHungry;
        Cursor.SetCursor (cursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    void OnTriggerStay2D(Collider2D other) {
        GameObject otherObj = other.gameObject;
        sprRenderer = otherObj.GetComponent<SpriteRenderer>();
        if (otherObj.tag == "Bubble") { //Hint will be shown when player is close
                if (otherObj.name == "ChestContents") { //if the player is near the chest, show chest contents
                    if (currentState == playerState.needRod || currentState == playerState.needBag) {
                        chestBubble.SetActive(true);
                    }
                } else { //all other bubbles
                    sprRenderer.enabled = true; //show Bubble
                }
        }

        if(otherObj.name == "ChickenBubble") {
            Debug.Log(acornCount);
            ChickenState thisChicken = otherObj.transform.parent.GetComponent<ChickenState>();
            thisChicken.chickenDir = 0; //stop chicken moving

            if (acornCount > 0 && Input.GetMouseButtonDown(0)) { //colliding w/chickens
                if (thisChicken.currentState == ChickenState.objState.hungry) {
                    acornCount--;
                    thisChicken.currentState = ChickenState.objState.happy;
                }
            }
        }

        if(otherObj.name == "CatBubble") { //Getting close to cat
            if (inDialogue) {
                sprRenderer.enabled = false; //disable cat hint when talking to it
            }

            if(!inDialogue && Input.GetMouseButtonDown(0)) {


                if (currentState == playerState.catHungry) { //talking to cat at the beginning
                    resetDialogue();  
                    speechBubble.SetActive(true);
                    dialogue.currentSpeech = dialogue.catTalk;
                    currentState = playerState.gateLocked;

                } 
                
                if (currentState == playerState.needKey) { //after player sees that the gate is locked
                    resetDialogue();
                    speechBubble.SetActive(true);
                    dialogue.currentSpeech = dialogue.catTalk2;
                    currentState = playerState.hasKey;
                }
            }

            if (currentState == playerState.hasKey && !inDialogue) { //after the player gets the key, change the cat's sprite
                GameObject Cat = otherObj.transform.parent.gameObject;
                Cat.GetComponent<SpriteRenderer>().sprite = Cat.GetComponent<CatState>().noKeySprite;
            }
        }

        if(otherObj.name == "NeighborBubble") { //Getting close to neighbor
            NeighborDialogue neighbor = otherObj.transform.parent.gameObject.GetComponent<NeighborDialogue>();

            if (!inDialogue && Input.GetMouseButtonDown(0)) { //pressing f to talk to neighbor
                inDialogue = true;
                neighborBubble.SetActive(true);
                neighbor.speechIndex = 0;

                if (currentState == playerState.needBait) { //after you try to fish without bait, player gives hint that you need eggs for bait
                    neighbor.currentSpeech = neighbor.eggHint;
                    currentState = playerState.needBag;
                }

                if (currentState == playerState.needBag && eggCount > 0) { //if you have eggs but no coal, neighbor will give you permission to use coal
                    currentState = playerState.permission;
                    neighbor.currentSpeech = neighbor.permission;
                }
            }
        }

        if (otherObj.name == "HoldRod") { //NEAR POND
            if(!inDialogue && Input.GetMouseButtonDown(0)) {

                if (currentState == playerState.needRod && !hasRod) { //player doesn't have rod
                    resetDialogue();
                    thoughtBubble.SetActive(true);
                    dialogue.currentThought = dialogue.rodHint;
                }

                if (currentState == playerState.hasBait && Input.GetMouseButtonDown(0)) { //player has bait
                    fishLike = true;
                } else if (hasRod) { //player has rod and no bait
                    resetDialogue();
                    fishScare = true;
                    currentState = playerState.needBait;
                    speechBubble.SetActive(true);
                    dialogue.currentSpeech = dialogue.baitHint;
                }
            }

        }

        if (otherObj.name == "CoalBagTrigger" && currentState == playerState.permission) {
            if (Input.GetMouseButtonDown(0)) {
                hasCoal = true;
                Debug.Log("Has Coal: " + hasCoal);
            }
        }

        if (otherObj.name == "FurnaceTrigger") { //NEAR FURNACE
            if (Input.GetMouseButtonDown(0) && !inDialogue) {
                if (currentState == playerState.permission && hasCoal) {
                    if (eggCount >= 3) {
                        baitBubble.SetActive(true);
                    } else {
                        resetDialogue();
                        thoughtBubble.SetActive(true);
                        dialogue.currentThought = dialogue.eggHint;
                        dialogue.thoughtIndex = 0;
                    }
                }
            }
        }

     }

    void OnTriggerEnter2D(Collider2D other) {
        GameObject otherObj = other.gameObject;
        sprRenderer = otherObj.GetComponent<SpriteRenderer>();

        if (otherObj.tag == "Acorn") { //NEAR ACORN
            if (currentState == playerState.needBag) { //if the player is trying to get acorns for chickens, but doesnt have bag
                thoughtBubble.SetActive(true);
                dialogue.currentThought = dialogue.acornHint;
                dialogue.thoughtIndex = 0;
            }

            if (hasBag) { //pick up the acorn if the player has bag
                acornCount++;
                Destroy(otherObj);
            }
        }

        if (otherObj.tag == "Egg") { //NEAR EGG
            eggCount++;
            Destroy(otherObj);
        }

        if (otherObj.tag == "FishCollider" && currentState == playerState.hasBait) { //NEAR FISH
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

         if (otherObj.name == "GateDoor") { //COLLIDING W/ GATE
             if(currentState == playerState.gateLocked) { //before the player has the key, after the player has talked to the cat once
                speechBubble.SetActive(true);
                dialogue.speechIndex = 0;
                dialogue.currentSpeech = dialogue.gateHint;
                inDialogue = true;
                currentState = playerState.needKey;
             }

             if (currentState == playerState.hasKey) {
                 Destroy(otherObj);
                 currentState = playerState.needRod;
             }
         }

         if (otherObj.tag == "Chicken") { //COLLIDING W/ CHICKEN
            ChickenState thisChicken = otherObj.GetComponent<ChickenState>();
            thisChicken.chickenDir = 0; //stop chicken moving
         }
     }

     void resetDialogue() {
        dialogue.speechIndex = 0;
        dialogue.thoughtIndex = 0;
        inDialogue = true;
     }
     
}
