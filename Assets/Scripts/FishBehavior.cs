using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    public GameObject avoiding; //object to avoid (insert player)
    public float sightDist; //distance to cast the ray
    public float moveSpeed; //speed at which fish moves to its destination

    RaycastHit2D fishSight; //ray from fish to an object it is avoiding
    Vector2 thisPos; //position of fish (start of ray)
    Vector2 avoidingPos; //position of what the fish is avoiding (direction of ray)

    public BoxCollider2D pondBounds; //box collider of fish pond boundaries
    private float xMin, xMax, yMin, yMax, fishX, fishY; //minmax fish pond, fish position

    private BoxCollider2D fishCollider; //box collider of fish
    private float colliderHeight, colliderWidth; //height and width of fish box collider

    void Start() {
        xMin = pondBounds.bounds.min.x; //left side of pond
        xMax = pondBounds.bounds.max.x; //right side of pond
        yMin = pondBounds.bounds.min.y; //top of pond
        yMax = pondBounds.bounds.max.y; //bottom of pond

        fishCollider = this.GetComponent<BoxCollider2D>();
        colliderWidth = fishCollider.transform.lossyScale.x / 2; //collider dimensions converted to world scale
        colliderHeight = fishCollider.transform.lossyScale.y / 2;
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
                Debug.Log("Fish sees player");
                if (playerStates.hasRod) { //check if the player has fishing rod
                    if(playerStates.hasBait) { //check if the player has fish bait
                        newPos = Vector2.MoveTowards(transform.position, avoidingPos, moveSpeed); //fish are attracted to rod with bait
                    } else {
                        newPos = Vector2.MoveTowards(transform.position, avoidingPos, -moveSpeed); //fish are scared away by rod
                    }
                }
            }
        }
        Debug.DrawRay(thisPos, avoidingPos - thisPos, Color.red); //fish line of sight
        return newPos;
    }

//need a method for fish swimmin around minding their business
//probably like... randomly generated direction change

}
