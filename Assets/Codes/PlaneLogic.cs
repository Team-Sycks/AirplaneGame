using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneLogic : MonoBehaviour
{
    public long departureTime;
    public long arrivalTime;

    public float longitude;
    public float latitude;

    public Vector3 departureCoordinates;
    public Vector3 arrivalCoordinates;

    public float flightPercentage = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        float latScale = 2000 / 180.0f;  //latitude goes from 0(Npole) to 180(Spole)
        float longiScale = 4000 / 360.0f; //longitude goes from 0 to 360 (Greenwich)
        float z = latitude * latScale;
        float x = longitude * longiScale;

        this.transform.position = new Vector3(x, transform.position.y, z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
