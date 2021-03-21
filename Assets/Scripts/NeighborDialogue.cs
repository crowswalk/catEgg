using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NeighborDialogue : MonoBehaviour
{
    public PlayerTriggers states; //track player states
    public GameObject neighborBubble;
    public TMP_Text speechText; //access textmeshpro text components

    public int speechIndex;

    public string[] greeting, eggHint, permission;
    public string[] currentSpeech;

    void Start() {
        speechIndex = 0;
        currentSpeech = greeting;
    }

    void Update() {
        speechText.text = currentSpeech[speechIndex];
    }

    public void IncreaseIndex() { //increase current index of speech/thought (called from clicking arrow)
        if (speechIndex < currentSpeech.Length - 1) {
            speechIndex++;
        } else {
            speechIndex = 0;
            neighborBubble.SetActive(false);
            states.inDialogue = false;
        }
    }
}
