using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform pointFrom;
    public Transform pointTo;
    public Transform player;

    Vector3 offset;

    private void Start()
    {
        offset = pointTo.position - pointFrom.position;
    }

    public void TelelportPlayer()
    {
        player.transform.position += offset;
    }
}
