using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RoomController : MonoBehaviour
{
    public List<PuzzleElement> pieces;
    public DoorController entrance_door;
    public DoorController exit_door;
    public RoomController previousRoom;
    public bool activeOnStart = false;

    bool solved = false;
    bool puzzleStarted = false;
    bool puzzleEnded { get; set; }

    private void Start()
    {
        puzzleStarted = activeOnStart;
        puzzleEnded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleEnded || !puzzleStarted) return;

        foreach (PuzzleElement piece in pieces)
        {
            if (!piece.IsCorrect())
            {
                if (solved)
                {
                    solved = false;
                    exit_door.CloseDoor();
                }
                return;
            }
        }

        if (!solved)
        {
            solved = true;
            exit_door.OpenDoor();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            entrance_door.CloseDoor();
            puzzleStarted = true;
            if (previousRoom) previousRoom.puzzleEnded = true;
            GetComponent<Collider>().enabled = false;
        }
    }
}
