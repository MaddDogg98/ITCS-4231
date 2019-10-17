using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque;     // maximum torque the motor can apply to the wheel
    public float maxSteeringAngle;   // maximum steer angle the wheel can have

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider) {

        if(collider.transform.childCount == 0) {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate() {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach(AxleInfo axleinfo in axleInfos) {
            if(axleinfo.steering) {
                axleinfo.leftWheel.steerAngle = steering;
                axleinfo.rightWheel.steerAngle = steering;
            }
            if(axleinfo.motor) {
                axleinfo.leftWheel.motorTorque = motor;
                axleinfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleinfo.leftWheel);
            ApplyLocalPositionToVisuals(axleinfo.rightWheel);
        }
    }
}
 
[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
