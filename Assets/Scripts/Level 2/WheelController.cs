using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : Grabbable
{
    public float accel = 10f;
    public float angularVelThresh = 0.5f;
    public float lockInTime;
    public AnimationCurve lockInCurve;

    Transform holder;
    Vector3 holderVec;
    Vector3 planeNormal;
    float startZRot;
    float prevZRot;
    float angularVel;

    float goalZRot;
    float lockInTimer = 0f;
    bool lockedIn = true;

    // Start is called before the first frame update
    void Start()
    {
        planeNormal = transform.TransformVector(Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (holder)
        {
            Vector3 newVec = holder.position - transform.position;
            newVec = Vector3.ProjectOnPlane(newVec, planeNormal);
            float angle = Vector3.SignedAngle(holderVec, newVec, planeNormal);

            float xRot = transform.localEulerAngles.x;
            float yRot = transform.localEulerAngles.y;
            float zRot = startZRot + angle;
            transform.localEulerAngles = new Vector3(xRot, yRot, zRot);

            angularVel = (zRot - prevZRot) / Time.deltaTime;
            prevZRot = zRot;
        }
        else if (Mathf.Abs(angularVel) > angularVelThresh)
        {
            float xRot = transform.localEulerAngles.x;
            float yRot = transform.localEulerAngles.y;
            float zRot = transform.localEulerAngles.z + angularVel * Time.deltaTime;
            transform.localEulerAngles = new Vector3(xRot, yRot, zRot);

            if (angularVel < 0f)
            {
                angularVel += accel * Time.deltaTime;
            }
            else
            {
                angularVel -= accel * Time.deltaTime;
            }

            if (Mathf.Abs(angularVel) < Mathf.Abs(angularVelThresh))
            {
                angularVel = 0f;
                lockInTimer = lockInTime;
                startZRot = zRot;
                goalZRot = CurrentValue() * 36f;
            }
        }
        else if (lockInTimer > 0f)
        {
            lockInTimer -= Time.deltaTime;
            float t = lockInCurve.Evaluate(1f - lockInTimer / lockInTime);

            float xRot = transform.localEulerAngles.x;
            float yRot = transform.localEulerAngles.y;
            float zRot = Mathf.Lerp(startZRot, goalZRot, t);
            transform.localEulerAngles = new Vector3(xRot, yRot, zRot);

            if (lockInTimer <= 0f)
            {
                lockedIn = true;
            }
        }
    }

    public override void OnGrab(Transform grabber)
    {
        holder = grabber;
        holderVec = holder.position - transform.position;
        holderVec = Vector3.ProjectOnPlane(holderVec, planeNormal);
        startZRot = transform.localEulerAngles.z;
        prevZRot = startZRot;
        lockedIn = false;
    }

    public override void OnRelease(Transform grabber)
    {
        if (holder != grabber) return;
        holder = null;
    }

    public bool LockedIn()
    {
        return lockedIn;
    }

    public int CurrentValue()
    {
        float zRot = transform.localEulerAngles.z;
        zRot += 360f / 20f;
        while (zRot < 0f)
        {
            zRot += 360f;
        }
        return Mathf.FloorToInt(zRot / 10f);
    }
}
