using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingNav : MonoBehaviour
{
    public GameObject RigRef;
    public GameObject HeadRef;
    public float speed = 1f;

    Vector3 rigposition;
    Vector3 rigrotation;
    Vector3 newposition;
    Vector3 newrotation;
    float reductionfactor;
    bool collided;
    bool upAngle;
    bool downAngle;

    // Start is called before the first frame update
    void Start()
    {
        rigposition = RigRef.transform.position;
        rigrotation = RigRef.transform.eulerAngles;
        newposition = new Vector3(0, 0, 0);
        newrotation = new Vector3(0, 0, 0);
        reductionfactor = 0.1f;
        collided = false;
        upAngle = false;
        downAngle = false;
    }



    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("collided with wall");
        if (collider.isTrigger)
        {
            collided = true;
        }
        
        if (collider.gameObject.name == "blue wall nav trigger")
        {
            upAngle = true;
        }

        if (collider.gameObject.name == "red wall nav trigger")
        {
            downAngle = true;
        }

        if (collider.gameObject.name == "blue wall nav trigger (1)")
        {
            upAngle = false;
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

        Vector3 forward = HeadRef != null ? Vector3.ProjectOnPlane(HeadRef.transform.forward, Vector3.up) : RigRef.transform.forward;
        Vector3 right = HeadRef != null ? Vector3.ProjectOnPlane(HeadRef.transform.right, Vector3.up) : RigRef.transform.right;

        if (!collided)
        {
            newposition = new Vector3(newx, rigposition.y, newz);
            RigRef.transform.position += forward * (thumbstickpos.y * speed * Time.deltaTime);
            RigRef.transform.position += right * (thumbstickpos.x * speed * Time.deltaTime);
        }

        //Debug.Log(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick));
        Vector2 thumbstickrot = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        rigrotation = RigRef.transform.eulerAngles;
        float newry = rigrotation.y + (thumbstickrot.x * 2);

        if (upAngle)
        {
            newrotation = new Vector3(-37.0f, newry, rigrotation.z);

        } else if (downAngle) {

            newrotation = new Vector3(50.0f, newry, rigrotation.z);

        } else {

            newrotation = new Vector3(0.0f, newry, rigrotation.z);

        }
        
        //RigRef.transform.rotation = Quaternion.Euler(newrx, RigRef.transform.rotation.y, newrz);
        RigRef.transform.eulerAngles = newrotation;

    }
}
