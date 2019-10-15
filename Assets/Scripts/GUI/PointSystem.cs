using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointSystem : MonoBehaviour
{
    // This is the text field that will tally the points each
    // time the player collides with the equivalent item.
    [SerializeField] private Text points;

    [SerializeField] private GameObject coins;
    [SerializeField] private Transform car;

    private float radiusOfSatisfaction;

    private int ptCounter;

    void Start() {
        ptCounter = 0;
        radiusOfSatisfaction = 2f;
    }

    void Update() {
        SetPoints();
        IncreasePoints();
    }

    // Set the text for the points
    private void SetPoints() {
        points.text = "Points: " + ptCounter;
    }

    // Increase the points each time the car is less than the radiusOfSatisfaction, then
    // destroy the MeshRender of the coin.
    private void IncreasePoints() {

        for (int i = 0; i < coins.transform.childCount; i++) {

            Vector3 towards = coins.transform.GetChild(i).position - car.position;

            if (towards.magnitude < radiusOfSatisfaction) {
                ptCounter++;
                Destroy(coins.transform.GetChild(i).GetComponent<MeshRenderer>());
            }
        }
    }
}
