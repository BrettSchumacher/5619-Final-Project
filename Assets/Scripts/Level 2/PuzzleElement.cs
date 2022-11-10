using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElement : MonoBehaviour
{
    [Range(0,9)]
    public int correctValue;
    public WheelController wheel;

    public bool IsCorrect()
    {
        return wheel.LockedIn() && (wheel.CurrentValue() == correctValue);
    }
}
