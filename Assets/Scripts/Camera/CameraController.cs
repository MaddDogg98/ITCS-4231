using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject theCar;
    private float carX;
    private float carY;
    private float carZ;

    // Update is called once per frame
    void Update() {
        carX = theCar.transform.eulerAngles.x;
        carY = theCar.transform.eulerAngles.y;
        carZ = theCar.transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(carX - carX, carY, carZ - carZ);
    }
}
