using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Teleport : MonoBehaviour
{
    public Transform pointFrom;
    public Transform pointTo;
    public Transform player;

    public UnityAction onTeleport;

    Vector3 offset;

    private void Start()
    {
        offset = pointTo.position - pointFrom.position;
    }

    public void TelelportPlayer()
    {
        player.transform.position += offset;
        onTeleport?.Invoke();
    }
}
