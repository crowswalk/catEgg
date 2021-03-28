using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehavior : MonoBehaviour
{
    private Texture2D cursor;
    private Texture2D hand;

    void Start() {
        cursor = Resources.Load<Texture2D>("Cursor");
        hand = Resources.Load<Texture2D>("Hand");
    }

    void OnMouseOver() {
        Cursor.SetCursor(hand, Vector2.zero, CursorMode.ForceSoftware);
    }

    void OnMouseExit() {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}
