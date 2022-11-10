using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<PuzzleElement> pieces;
    public DoorController door;

    bool solved = false;

    // Update is called once per frame
    void Update()
    {
        if (solved) return;

        foreach (PuzzleElement piece in pieces)
        {
            if (!piece.IsCorrect()) return;
        }

        solved = true;
        door.OpenDoor();
    }
}
