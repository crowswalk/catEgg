using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    public GameObject avoiding; //object to avoid (insert player)
    public float sightDist; //distance to cast the ray
    public float[] speedRange = new float[2];
    private float moveSpeed; //speed at which fish moves to its destination
    public float swimSpeed; // speed at which fish moves when it doesn't see player

    RaycastHit2D fishSight; //ray from fish to an object it is avoiding
    Vector2 thisPos; //position of fish (start of ray)
    Vector2 avoidingPos; //position of what the fish is avoiding (direction of ray)

    public BoxCollider2D pondBounds; //box collider of fish pond boundaries
    private float xMin, xMax, yMin, yMax, fishX, fishY; //minmax fish pond, fish position

    private BoxCollider2D fishCollider; //box collider of fish
    private float colliderHeight, colliderWidth; //height and width of fish box collider

    public int swimTime; //length of time between swim direction changes
    private int swimTimer; //timer for swim direction changes
    private int swimDir; //direction fish swims when it doesnt see player

    public Sprite[] dirs = new Sprite[3];
    private SpriteRenderer fishSprite;

    void Start() {
        xMin = pondBounds.bounds.min.x; //left side of pond
        xMax = pondBounds.bounds.max.x; //right side of pond
        yMin = pondBounds.bounds.min.y; //top of pond
        yMax = pondBounds.bounds.max.y; //bottom of pond

        fishSprite = this.GetComponent<SpriteRenderer>();
        fishCollider = this.GetComponent<BoxCollider2D>();
        colliderWidth = fishCollider.size.x; //collider dimensions converted to world scale
        colliderHeight = fishCollider.size.y;
        moveSpeed = UnityEngine.Random.Range(speedRange[0], speedRange[1]);
        swimTimer = 0;
        swimDir = 0;
    }

    void FixedUpdate() {
        Vector2 destination = seePlayer(); //base intended direction on raycast to player
        fishX = Mathf.Clamp(destination.x, xMin + colliderWidth, xMax - colliderWidth); //keep x position inside pond bounds
        fishY = Mathf.Clamp(destination.y, yMin + colliderHeight, yMax - colliderHeight); //keep y position inside pond bounds
        transform.position = new Vector2(fishX, fishY); //camera transform is at smoothed position
    }

/*-----seePlayer
*Casts Ray from fish to player. Sees if player is within sightDist. If so, do the following:
*If the player has the rod:
*   If the rod has bait attached: move toward the player.
*   Else: move away from the player.
*
*Else, move normally.
*
*@return intended location for fish to move to
*/
    Vector2 seePlayer() { 
        Vector2 newPos = transform.position; //position to return
        thisPos = new Vector2(transform.position.x, transform.position.y); //position of fish
        avoidingPos = new Vector2(avoiding.transform.position.x, avoiding.transform.position.y); //position of avoiding
        fishSight = Physics2D.Raycast(thisPos, avoidingPos - thisPos, sightDist,  1 << LayerMask.NameToLayer("Walkable")); //continually cast ray based on avoiding's location
        PlayerTriggers playerStates = avoiding.GetComponent<PlayerTriggers>(); //track avoiding's states to see if they have rod or bait
        if(fishSight.collider != null) {
            if(fishSight.collider.tag == "Player") { //if fish sees player
                if (playerStates.hasRod) { //check if the player has fishing rod
                    if(playerStates.hasBait) { //check if the player has fish bait
                        newPos = Vector2.MoveTowards(transform.position, avoidingPos, moveSpeed); //fish are attracted to rod with bait
                    } else {
                        newPos = Vector2.MoveTowards(transform.position, avoidingPos, -moveSpeed); //fish are scared away by rod
                    }
                    fishSprite.sprite = dirs[0];
                    Quaternion rotation = Quaternion.LookRotation //rotate to direction of player
                    (avoidingPos - thisPos, transform.TransformDirection(Vector3.up));
                    transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
                }
            }
        } else { //fish doesn't see anything
            swimTimer++;
            if (swimTimer >= swimTime) {
                swimTimer = 0;
                swimDir = UnityEngine.Random.Range(1, 7);
            }
            newPos = normalMove(swimDir); 
        }
        Debug.DrawRay(thisPos, avoidingPos - thisPos, Color.red); //fish line of sight
        return newPos;
    }

    void OnCollisionStay2D(Collision2D other) { //when fish overlap, they begin to separate
        if (other.gameObject.tag == "Fish") {
            Vector2 otherPos = other.gameObject.transform.position;
            Vector2 thisPos = transform.position;
            Vector2 newPos = thisPos - otherPos;

            transform.position = Vector2.Lerp(thisPos, thisPos + newPos, moveSpeed * 5);
            Debug.Log("Other fish: " + otherPos + " This Fish: " + thisPos + "\nNew Position: " + newPos);
        }
    }

/*-----normalMove
*Called from within seePlayer, when the fish doesn't see the player.
*
*Before calling, create a timer, and once the timer is complete, generate a number (1-4)
*
*@param dir, integer that indicates which direction to move in
*/
    Vector2 normalMove(int dir) {
        transform.rotation = new Quaternion(0, 0, 0, 0);
        Vector2 prevPos = transform.position;
        Vector2 normPos = prevPos;
        if (dir == 1) { //up
            normPos += Vector2.up * Time.deltaTime * swimSpeed;
            fishSprite.sprite = dirs[2];
        } else if (dir == 2) { //down
            normPos += Vector2.down * Time.deltaTime * swimSpeed;
            fishSprite.sprite = dirs[0];
        } else if (dir == 3) { //left
            normPos += Vector2.left * Time.deltaTime * swimSpeed;
            fishSprite.sprite = dirs[1];
            fishSprite.flipX = false;
        } else if (dir == 4) { //right
            normPos += Vector2.right * Time.deltaTime * swimSpeed;
            fishSprite.sprite = dirs[1];
            fishSprite.flipX = true;
        } else {
            fishSprite.sprite = dirs[0];
        }
        return normPos;
    }
}