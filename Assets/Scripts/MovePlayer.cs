using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This Script is responsible for moving the player and showing their walking sprites

public class MovePlayer : MonoBehaviour
{
    public float speed; //walking speed
    public float framerate; //frames per second (multiplied by deltaTime)

    public Sprite[] downSprite = new Sprite[1]; //set sprite arrays
    public Sprite[] upSprite = new Sprite[1];
    public Sprite[] sideSprite = new Sprite[1];

    private Sprite currentSprite; //currently displayed sprite
    private Sprite stillSprite; //sprite to use when you stop walking
    private float currentFrame; //current frame (for fps/animation)

    private directionState currentState; //inital direction state is none
    private SpriteRenderer sprRenderer; //to access & change sprite renderer
    private BoxCollider2D playerCollider;

    private enum directionState { //Direction states for translation
        up, down, left, right, none
    }

    void Start() { //called before the first frame update
        sprRenderer = GetComponent < SpriteRenderer > ();
        playerCollider = GetComponent < BoxCollider2D > ();
        currentState = directionState.none;
        stillSprite = downSprite[0];
        currentSprite = stillSprite;
        currentFrame = 0;
    }

    void FixedUpdate() { //called once per frame
        sprRenderer.sprite = currentSprite; //show sprite that was calculated in walkCycle
        checkKey(); //check key input
        moveMe(); //move/change sprite based on key input
    }

    void checkKey() { //Check input & set movement state
        if (Input.GetKey(KeyCode.W)) { //UP
            currentState = directionState.up;
        } else if (Input.GetKey(KeyCode.S)) { //DOWN
            currentState = directionState.down;
        } else if (Input.GetKey(KeyCode.A)) { //LEFT
            currentState = directionState.left;
        } else if (Input.GetKey(KeyCode.D)) { //RIGHT
            currentState = directionState.right;
        } else { //STILL
            currentState = directionState.none;
            currentSprite = stillSprite;
        }
    }

    void walkCycle(Sprite[] dir) {
        stillSprite = dir[0];
        if ((int)currentFrame < dir.Length) {
            currentSprite = dir[(int)currentFrame];
        } else {
            currentFrame = 0;
        }
        currentFrame += Time.deltaTime * framerate; //add frames per second
    }

    void moveMe() { //Check movement state & move character, animate sprite
        switch (currentState) {
            case directionState.up:
                transform.Translate(Vector3.up * Time.deltaTime * speed);
                walkCycle(upSprite);
                break;
            case directionState.down:
                transform.Translate(Vector3.down * Time.deltaTime * speed);
                walkCycle(downSprite);
                break;
            case directionState.left:
                transform.Translate(Vector3.left * Time.deltaTime * speed);
                walkCycle(sideSprite);
                sprRenderer.flipX = false;
                break;
            case directionState.right:
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                walkCycle(sideSprite);
                sprRenderer.flipX = true;
                break;
            default:
                break;
        }
    }
}
