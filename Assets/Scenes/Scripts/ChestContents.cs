using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestContents : MonoBehaviour
{
    public PlayerTriggers player;
    void Update() {
        if(player.currentState != PlayerTriggers.playerState.needRod && player.currentState != PlayerTriggers.playerState.needBag) {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
