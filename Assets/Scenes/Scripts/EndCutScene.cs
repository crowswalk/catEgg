using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutScene : MonoBehaviour
{
    public SpriteRenderer ShockedEyes, FishEye, Sweat, bubble;
    public GameObject FishHands;

    public enum cutSceneStates {
        start, handover, fishOpen, catSurprised, fishTalk
    }

    private float timer;
    public float pauseTime;

    public float handOverPos;

    public cutSceneStates currentState;
    Camera mainCam;

    public bool sceneActive;

    void Start() {
        currentState = cutSceneStates.start;
        timer = 0;
        sceneActive = false;
    }

    void Update() {
        switch(currentState) {
            case cutSceneStates.start:
                sceneActive = true;
                timedInput(cutSceneStates.handover);
            break;

            case cutSceneStates.handover:
                Vector2 fishPos = FishHands.transform.position;
                Vector2 newPos = Vector2.Lerp(new Vector2(fishPos.x, fishPos.y), new Vector2(handOverPos, fishPos.y), .3f);
                FishHands.transform.position = newPos;
                timedInput(cutSceneStates.fishOpen);
            break;

            case cutSceneStates.fishOpen:
                FishEye.enabled = true;
                timedInput(cutSceneStates.catSurprised);
            break;

            case cutSceneStates.catSurprised:
                Sweat.enabled = true;
                ShockedEyes.enabled = true;
                timedInput(cutSceneStates.fishTalk);
            break;

            case cutSceneStates.fishTalk:
                bubble.enabled = true;
            break;
        }
    }

    void timedInput(cutSceneStates next) {
        timer++;
        if (timer > pauseTime && Input.GetMouseButtonDown(0)) {
            timer = 0;
            currentState = next;
        }
    }
}
