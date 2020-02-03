using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;

public class OpenSkyNetwork : MonoBehaviour
{
    private String api = "https://opensky-network.org/api/flights/arrival?airport=EFHK&begin=1517227200&end=1517230800";
    // Start is called before the first frame update
    void Start()
    {
        GetAirPlane();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Plane> GetAirPlane()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(api));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);

        PlaneList planeList = JsonUtility.FromJson<PlaneList>("{\"planes\":" + jsonResponse + "}");

        //Remove planes that don't have Departure or Arrival airport listed
        List<Plane> planes = planeList.planes;
        for(int i = planes.Count - 1; i > -1; i--)
        {
            Plane plane = planes[i];
            if(plane.estArrivalAirport.Equals("") || plane.estDepartureAirport.Equals(""))
            {
                planes.RemoveAt(i);
            }
        }

        foreach (Plane plane in planes)
        {
            Debug.Log(plane.firstSeen + ", " + plane.lastSeen +  ", " +plane.estDepartureAirport + ", " + plane.estArrivalAirport);
        }
        return planes;
    }

    [Serializable]
    public class PlaneList
    {
        public List<Plane> planes;
    }
    [Serializable]
    public class Plane
    {
        public int firstSeen;
        public int lastSeen;
        public String estDepartureAirport;
        public String estArrivalAirport;
    }
}
