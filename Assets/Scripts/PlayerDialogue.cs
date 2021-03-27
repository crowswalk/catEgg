using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDialogue : MonoBehaviour
{
    public GameObject thoughtBubble, speechBubble;

    public TMP_Text speechText; //access textmeshpro text components
    public TMP_Text thoughtText;

    PlayerTriggers states; //track player states
    public int speechIndex, thoughtIndex;

    public string[] catTalk, catTalk2, gateHint, acornHint, rodHint, baitHint, eggHint;
    public string[] currentSpeech, currentThought;

    void Start() {
        states = gameObject.GetComponent<PlayerTriggers>();
        speechIndex = 0;
        thoughtIndex = 0;
        currentSpeech = catTalk;
        currentThought = gateHint;
    }

    void Update() {
        speechText.text = currentSpeech[speechIndex];
        thoughtText.text = currentThought[thoughtIndex];
    }

    public void IncreaseIndex() { //increase current index of speech/thought (called from clicking arrow)
        if (speechIndex < currentSpeech.Length - 1) {
            speechIndex++;
        } else {
            speechIndex = 0;
            speechBubble.SetActive(false);
            states.inDialogue = false;
        }
    }

    public void IncreaseThought() {
        if (thoughtIndex < currentThought.Length - 1) {
            thoughtIndex++;
        } else {
            thoughtIndex = 0;
            thoughtBubble.SetActive(false);
            states.inDialogue = false;
        }
    }

}
