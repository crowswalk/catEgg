using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour { //camera following behavior, doesn't cross world bounds
    public Transform followTransform; //transform of object to follow
    public BoxCollider2D worldBounds; //box collider of world boundary
    public float smoothRate;
    
    private float xMin, xMax, yMin, yMax, camX, camY, camRatio, camSize; //minmax world coordinates, camera position and dimensions
    Camera mainCam;
    Vector3 smoothPos;

    void Start() {
        mainCam = gameObject.GetComponent<Camera>();
        xMin = worldBounds.bounds.min.x; //left
        xMax = worldBounds.bounds.max.x; //right
        yMin = worldBounds.bounds.min.y; //top
        yMax = worldBounds.bounds.max.y; //bottom
        camSize = mainCam.orthographicSize; //half of camera height
        camRatio = camSize * mainCam.aspect; //half of camera width
    }

    void FixedUpdate() {
        camY = Mathf.Clamp(followTransform.position.y, yMin + camSize, yMax - camSize); //keep y position inside world bounds
        camX = Mathf.Clamp(followTransform.position.x, xMin + camRatio, xMax - camRatio); //keep x position inside world bounds
        smoothPos = Vector3.Lerp(gameObject.transform.position, new Vector3(camX, camY, gameObject.transform.position.z), smoothRate); //lerp position towards target
        gameObject.transform.position = smoothPos; //camera transform is at smoothed position
    }
}
