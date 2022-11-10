using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public float grabRadius = 1f;
    public bool gizmos = false;

    bool rightHanded;

    Grabbable rightHandObj;
    Grabbable leftHandObj;

    // Start is called before the first frame update
    void Start()
    {
        rightHanded = OVRInput.GetDominantHand() == OVRInput.Handedness.RightHanded;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Grabbable temp = TryGrab(rightHanded ? rightHand : leftHand);
            if (rightHanded)
            {
                rightHandObj = temp;
            }
            else
            {
                leftHandObj = temp;
            }
        }
        
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Grabbable temp = TryGrab(rightHanded ? leftHand : rightHand);
            if (rightHanded)
            {
                leftHandObj = temp;
            }
            else
            {
                rightHandObj = temp;
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (rightHanded && rightHandObj)
            {
                rightHandObj.OnRelease(rightHand);
                rightHandObj = null;
            }
            else if (!rightHanded && leftHandObj)
            {
                leftHandObj.OnRelease(leftHand);
                leftHandObj = null;
            }    
        }

        if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
        {
            if (rightHanded && leftHandObj)
            {
                leftHandObj.OnRelease(leftHand);
                leftHandObj = null;
            }
            else if (!rightHanded && rightHandObj)
            {
                rightHandObj.OnRelease(rightHand);
                rightHandObj = null;
            }
        }
    }

    Grabbable TryGrab(Transform hand)
    {
        Grabbable closestObj = null;
        float closestDist = Mathf.Infinity;

        Collider[] cols = Physics.OverlapSphere(hand.position, grabRadius);

        foreach (Collider col in cols)
        {
            Grabbable tempGrab = col.GetComponent<Grabbable>();
            if (!tempGrab) continue;

            float dist = Vector3.Distance(hand.position, tempGrab.transform.position);
            if (dist < closestDist)
            {
                closestObj = tempGrab;
                closestDist = dist;
            }
        }

        if (closestObj)
        {
            closestObj.OnGrab(hand);
            return closestObj;
        }

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!gizmos) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(leftHand.position, grabRadius);
        Gizmos.DrawWireSphere(rightHand.position, grabRadius);

    }
}
