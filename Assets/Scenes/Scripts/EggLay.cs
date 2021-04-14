using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggLay : MonoBehaviour
{
    public float laySpeed;
    public float layDist;

    private Vector3 destination;

    void Start() {
        destination = new Vector3(transform.position.x + layDist, transform.position.y);
    }

    void Update() {
        transform.position = Vector3.Lerp(transform.position, destination, laySpeed); //lerp position towards target
    }
}
