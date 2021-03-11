using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    public GameObject avoiding;
    public float sightDist;

    RaycastHit2D fishSight;
    Vector2 thisPos;
    Vector2 avoidingPos;

    void Start() {

    }

    void Update() {
        thisPos = new Vector2(transform.position.x, transform.position.y);
        avoidingPos = new Vector2(avoiding.transform.position.x, avoiding.transform.position.y);

        fishSight = Physics2D.Raycast(thisPos, avoidingPos - thisPos, sightDist,  1 << LayerMask.NameToLayer("Walkable")); //continually cast ray based on player's location
        
        if(fishSight.collider != null) {
            if(fishSight.collider.tag == "Player") {
                Debug.Log("i see player");
                transform.position = Vector2.MoveTowards(transform.position, avoidingPos, .3f);
            }
            //if player doesn't have bait
            //swim away from player
            //else, swim toward player
        } 

        Debug.DrawRay(thisPos, avoidingPos - thisPos, Color.red);
    }

//need a method for fish swimmin around minding their business
//probably like... randomly generated direction change

}
