using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public new Rigidbody rigidbody;
    private float medianRPM = 500f;
    private float maxRPM = 1000f;

    public Transform centerOfGravity;

    public WheelCollider frontRight;
    public WheelCollider frontLeft;
    public WheelCollider rearRight;
    public WheelCollider rearLeft;

    private float turnRadius = 5f;
    private float torque = 500f;
    private float brakeTorque = 500f;

    private float antiRoll = 40000.0f;

    public enum DriveMode
    {
        Front,
        Rear,
        All
    };

    public DriveMode driveMode = DriveMode.Rear;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centerOfGravity.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CarMovement();
    }

    public float Speed()
    {
        return rearRight.radius * Mathf.PI * rearRight.rpm * 60f / 1000f;
    }

    public float Rpm()
    {
        return rearLeft.rpm;
    }

    private void CarMovement()
    {

        //print(Speed().ToString());

        float scaledTorque = Input.GetAxis("Vertical") * torque;

        if (rearLeft.rpm < medianRPM)
            scaledTorque = Mathf.Lerp(scaledTorque / 10f, scaledTorque, rearLeft.rpm / medianRPM);
        else
            scaledTorque = Mathf.Lerp(scaledTorque, 0, (rearLeft.rpm - medianRPM) / (maxRPM - medianRPM));

        DoRollBar(frontRight, frontLeft);
        DoRollBar(rearRight, rearLeft);

        frontRight.steerAngle = Input.GetAxis("Horizontal") * turnRadius;
        frontLeft.steerAngle = Input.GetAxis("Horizontal") * turnRadius;

        frontRight.motorTorque = driveMode == DriveMode.Rear ? 0 : scaledTorque;
        frontLeft.motorTorque = driveMode == DriveMode.Rear ? 0 : scaledTorque;
        rearRight.motorTorque = driveMode == DriveMode.Front ? 0 : scaledTorque;
        rearLeft.motorTorque = driveMode == DriveMode.Front ? 0 : scaledTorque;

        if (Input.GetKey(KeyCode.Space))
        {

            frontRight.brakeTorque = brakeTorque;
            frontLeft.brakeTorque = brakeTorque;
            rearRight.brakeTorque = brakeTorque;
            rearLeft.brakeTorque = brakeTorque;
        }
        else
        {

            frontRight.brakeTorque = 0;
            frontLeft.brakeTorque = 0;
            rearRight.brakeTorque = 0;
            rearLeft.brakeTorque = 0;
        }
    }

    void DoRollBar(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = leftWheel.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;

        bool groundedR = rightWheel.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;

        float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
            rigidbody.AddForceAtPosition(leftWheel.transform.up * -antiRollForce,
                                            leftWheel.transform.position);
        if (groundedR)
            rigidbody.AddForceAtPosition(rightWheel.transform.up * -antiRollForce,
                                            rightWheel.transform.position);
    }
}
