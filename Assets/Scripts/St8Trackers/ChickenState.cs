using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenState : MonoBehaviour
{
    public GameObject bubble;
    public Sprite[] bubbleSprites = new Sprite[2];
    public enum objState {hungry, happy};
    public objState currentState;

    public GameObject egg;
    public float eggLayOffset;
    private bool laid; //whether egg has been laid yet

    void Start() {
        currentState = objState.hungry;
        bubble.GetComponent<SpriteRenderer>().sprite = bubbleSprites[0];
        laid = false;
    }

    void Update() {
        switch (currentState) {
            case objState.hungry:
                bubble.GetComponent<SpriteRenderer>().sprite = bubbleSprites[0];
                break;
            case objState.happy:
                bubble.GetComponent<SpriteRenderer>().sprite = bubbleSprites[1];
                if (!laid) {
                    layEgg();
                }
                break;
            default:
                break;
            }
        }

        void layEgg() { //instantiate new egg
            Vector3 eggPos = new Vector3(transform.position.x + eggLayOffset, transform.position.y + eggLayOffset); //start egg at this location
            GameObject newEgg = Instantiate(egg, eggPos, transform.rotation);
            laid = true;
        }
}
