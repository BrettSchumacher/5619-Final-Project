using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public List<Transform> panels;
    public List<Vector3> panelOffsets;
    public float openingTime;
    public AnimationCurve openingCurve;

    public bool gizmos;
    List<Vector3> starts;

    float timer = 0;
    bool opening = false;
    bool closing = false;

    bool debugToggle = false;

    private void Start()
    {
        starts = new List<Vector3>(panels.Count);

        for (int i = 0; i < panels.Count; i++)
        {
            starts.Add(panels[i].position);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (debugToggle)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
            debugToggle = !debugToggle;
        }

        if (!opening && !closing) return;

        timer += opening ?  Time.deltaTime : -Time.deltaTime;
        float t = openingCurve.Evaluate(timer / openingTime);

        for (int i = 0; i < panels.Count; i++)
        {
            Vector3 from = starts[i];
            Vector3 to = starts[i] + panelOffsets[i];

            panels[i].position = Vector3.Lerp(from, to, t);
        }

        if (opening && timer >= openingTime)
        {
            opening = false;
        }

        if (closing && timer <= 0)
        {
            closing = false;
        }
    }

    public void OpenDoor()
    {
        opening = true;
        closing = false;
        timer = Mathf.Max(timer, 0f); ;
    }

    public void CloseDoor()
    {
        closing = true;
        opening = false;
        timer = Mathf.Min(timer, openingTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (!gizmos) return;

        if (panels.Count != panelOffsets.Count) return;

        for (int i = 0; i < panels.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(panels[i].position, 0.1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(panels[i].position, panels[i].position + panelOffsets[i]);
            Gizmos.DrawSphere(panels[i].position + panelOffsets[i], 0.1f);
        }
    }
}
