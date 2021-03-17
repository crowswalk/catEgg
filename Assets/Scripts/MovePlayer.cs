using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This Script is responsible for moving the player and showing their walking sprites

public class MovePlayer : MonoBehaviour
{
    public float speed;
    public float framerate; //frames per second (multiplied by deltaTime)

    public Sprite[] downSprite = new Sprite[1]; //set sprite arrays
    public Sprite[] upSprite = new Sprite[1];
    public Sprite[] sideSprite = new Sprite[1];
    public Sprite[] fishSprites = new Sprite[3];

    private Sprite currentSprite; //currently displayed sprite
    private Sprite stillSprite; //sprite to use when you stop walking
    private float currentFrame; //current frame (for fps/animation)

    private directionState currentState; //inital direction state is none
    private SpriteRenderer sprRenderer; //to access & change sprite renderer
    private BoxCollider2D playerCollider;
    private collisionDir currentDir;
    private bool nearPond;
    private Sprite fishingSprite;

    private enum directionState { //Direction states for translation
        up, down, left, right, none
    }

    private enum collisionDir { //relative position of last collision
        up, down, left, right, none
    }

    void Start() { //called before the first frame update
        sprRenderer = GetComponent < SpriteRenderer > ();
        playerCollider = GetComponent < BoxCollider2D > ();
        currentState = directionState.none;
        stillSprite = downSprite[0];
        currentSprite = stillSprite;
        currentFrame = 0;
        nearPond = false;
        fishingSprite = fishSprites[0];
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
            if (nearPond && GetComponent<PlayerTriggers>().hasRod) {
                currentSprite = fishingSprite;
            } else {
                currentSprite = stillSprite;
            }
        }
    }

/***
*walkCycle animates sprite.
*adds to a counter that determines when to change the sprite (this is convoluted, I'll simplify it later)
*trying to find the best way to be able to set framerate without it skipping certain sprites at some framerates.
*/
    void walkCycle(Sprite[] dir) { 
        stillSprite = dir[0];
        if ((int)currentFrame < dir.Length) {
            currentSprite = dir[(int)currentFrame]; //change sprite
        } else {
            currentFrame = 0;
        }
        currentFrame += Time.deltaTime * framerate; //add frames per second
    }

/***
*moveMe: tracks directionState, translates player based on this, and uses animation script
*/
    void moveMe() { //Check movement state & move character, animate sprite
        switch (currentState) {
            case directionState.up:
                transform.Translate(Vector3.up * Time.deltaTime * speed);
                walkCycle(upSprite);
                fishingSprite = fishSprites[1];
                break;
            case directionState.down:
                transform.Translate(Vector3.down * Time.deltaTime * speed);
                walkCycle(downSprite);
                fishingSprite = fishSprites[0];
                break;
            case directionState.left:
                transform.Translate(Vector3.left * Time.deltaTime * speed);
                walkCycle(sideSprite);
                sprRenderer.flipX = true; 
                fishingSprite = fishSprites[2];
                break;
            case directionState.right:
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                walkCycle(sideSprite);
                sprRenderer.flipX = false;
                fishingSprite = fishSprites[2];
                break;
            default:
                break;
        }
    }

/***
*Tracks which direction the player was going in before collision (determines direction of collision pushback)
*/
    void OnCollisionEnter2D(Collision2D other) { //determine relative position of collision
        switch (currentState) {
            case directionState.up:
                currentDir = collisionDir.up;
            break;
            case directionState.down:
                currentDir = collisionDir.down;
            break;
            case directionState.left:
                currentDir = collisionDir.left;
            break;
            case directionState.right:
                currentDir = collisionDir.right;
            break;
            default:
            break;
        }
    }

/***
*keeps track of collision dir set in oncollisionEnter, pushes back against player's normal transform
*/
    void OnCollisionStay2D(Collision2D other) { //push back on collisions based on relative position
                switch (currentDir) {
            case collisionDir.up:
                transform.Translate(Vector3.down * Time.deltaTime * speed);
                break;
            case collisionDir.down:
                transform.Translate(Vector3.up * Time.deltaTime * speed);
                break;
            case collisionDir.left:
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                break;
            case collisionDir.right:
                transform.Translate(Vector3.left * Time.deltaTime * speed);
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "HoldRod") {
            nearPond = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.name == "HoldRod") {
            nearPond = false;
        }
    }

}
