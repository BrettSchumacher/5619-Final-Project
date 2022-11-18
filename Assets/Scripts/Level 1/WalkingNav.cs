using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingNav : MonoBehaviour
{
    public GameObject RigRef;
    public float speed = 1f;

    Vector3 rigposition;
    Vector3 rigrotation;
    Vector3 newposition;
    Vector3 newrotation;
    float reductionfactor;
    bool collided;

    // Start is called before the first frame update
    void Start()
    {
        rigposition = RigRef.transform.position;
        rigrotation = RigRef.transform.eulerAngles;
        newposition = new Vector3(0, 0, 0);
        newrotation = new Vector3(0, 0, 0);
        reductionfactor = 0.1f;
        collided = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("collided with wall");
        if (collider.isTrigger)
        {
            collided = true;
        }
        
    }

    void OnTriggerExit(Collider collider)
    {
        collided = false;
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();

        //Debug.Log(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
        Vector2 thumbstickpos = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        rigposition = transform.TransformDirection(RigRef.transform.position);
        float newx = rigposition.x + (thumbstickpos.x * reductionfactor);
        float newz = rigposition.z + (thumbstickpos.y * reductionfactor);

        if (!collided)
        {
            newposition = new Vector3(newx, rigposition.y, newz);
            RigRef.transform.position += RigRef.transform.forward * (thumbstickpos.y * speed * Time.deltaTime);
            RigRef.transform.position += RigRef.transform.right * (thumbstickpos.x * speed * Time.deltaTime);
        }

        //Debug.Log(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick));
        Vector2 thumbstickrot = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        rigrotation = RigRef.transform.eulerAngles;
        float newry = rigrotation.y + (thumbstickrot.x * 2);

        newrotation = new Vector3(rigrotation.x, newry, rigrotation.z);
        //RigRef.transform.rotation = Quaternion.Euler(newrx, RigRef.transform.rotation.y, newrz);
        RigRef.transform.eulerAngles = newrotation;

    }
}
