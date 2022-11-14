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

    bool mainWasDown = false;
    bool altWasDown = false;

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        if (!mainWasDown && (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)))
        {
            print("down main hand");
            mainWasDown = true;
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
        
        if (!altWasDown && (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)))
        {
            print("doen alt hand");
            altWasDown = true;
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

        if (mainWasDown && !(OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)))
        {
            print("up main hand");
            mainWasDown = false;
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

        if (altWasDown && !(OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) || OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)))
        {
            print("up alt hand");
            altWasDown = false;
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
            print("grabbing " + col.name);
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
