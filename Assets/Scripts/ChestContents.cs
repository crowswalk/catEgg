using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestContents : MonoBehaviour
{
    public PlayerTriggers player;
    void Update() {
        if(player.hasRod && player.hasBag) {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
