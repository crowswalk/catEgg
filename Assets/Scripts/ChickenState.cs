using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenState : MonoBehaviour
{
    public GameObject bubble;
    private SpriteRenderer bubbleRenderer;
    public Sprite[] bubbleSprites = new Sprite[2];
    public enum objState {hungry, happy};
    public objState currentState;
    public Sprite[] sideSprite = new Sprite[2];

    public GameObject egg;
    public float eggLayOffset;
    private bool laid; //whether egg has been laid yet

    public int moveTimer;
    public float speed;
    public float framerate; //frames per second (multiplied by deltaTime)
    private int timer;
    public int chickenDir;
    private SpriteRenderer chickSprite;

    public Sprite[] chickSprites;
    private Sprite currentSprite; //currently displayed sprite
    private Sprite stillSprite; //sprite to use when you stop walking
    private float currentFrame; //current frame (for fps/animation)

    public BoxCollider2D penBounds; //box collider of chicken pen boundaries
    private BoxCollider2D chickCollider;
    private float xMin, xMax, yMin, yMax, chickX, chickY, colliderWidth, colliderHeight; //minmax chicken pen, chicken position

    void Start() {
        xMin = penBounds.bounds.min.x; //left side of pond
        xMax = penBounds.bounds.max.x; //right side of pond
        yMin = penBounds.bounds.min.y; //top of pond
        yMax = penBounds.bounds.max.y; //bottom of pond

        currentState = objState.hungry;
        bubbleRenderer = bubble.GetComponent<SpriteRenderer>();
        bubbleRenderer.sprite = bubbleSprites[0];
        laid = false;
        timer = 0;
        chickenDir = 0;
        chickSprite = this.GetComponent<SpriteRenderer>();
        stillSprite = sideSprite[0];

        chickCollider = this.GetComponent<BoxCollider2D>();
        colliderWidth = chickCollider.size.x; //collider dimensions converted to world scale
        colliderHeight = chickCollider.size.y;
    }

    void Update() {
        chickSprite.sprite = currentSprite;
        switch (currentState) {
            case objState.hungry:
                bubbleRenderer.sprite = bubbleSprites[0];
                break;
            case objState.happy:
                bubbleRenderer.sprite = bubbleSprites[1];
                if (!laid) {
                    layEgg();
                }
                break;
            default:
                break;
            }
        movement();
    }

    void layEgg() { //instantiate new egg
        Vector3 eggPos = new Vector3(transform.position.x + eggLayOffset, transform.position.y + eggLayOffset); //start egg at this location
        GameObject newEgg = Instantiate(egg, eggPos, transform.rotation);
        laid = true;
    }

    void movement() {
        timer++;
        if (timer >= moveTimer) {
            timer = 0;
            chickenDir = UnityEngine.Random.Range(1, 7);
        }

        Vector2 destination = chickenMove(chickenDir);

        chickX = Mathf.Clamp(destination.x, xMin + colliderWidth, xMax - colliderWidth); //keep x position inside pond bounds
        chickY = Mathf.Clamp(destination.y, yMin + colliderHeight, yMax - colliderHeight); //keep y position inside pond bounds
        transform.position = new Vector2(chickX, chickY); //camera transform is at smoothed position
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

    Vector2 chickenMove(int dir) {

        Vector2 prevPos = transform.position;
        Vector2 normPos = prevPos;
        if (dir == 1) { //up
            normPos += Vector2.up * Time.deltaTime * speed;
            walkCycle(sideSprite);
        } else if (dir == 2) { //down
            normPos += Vector2.down * Time.deltaTime * speed;
            walkCycle(sideSprite);
        } else if (dir == 3) { //left
            normPos += Vector2.left * Time.deltaTime * speed;
            walkCycle(sideSprite);
            chickSprite.flipX = false;
        } else if (dir == 4) { //right
            normPos += Vector2.right * Time.deltaTime * speed;
            walkCycle(sideSprite);
            chickSprite.flipX = true;
        } else {
            chickSprite.sprite = stillSprite;
        }
        return normPos;
    }
}
