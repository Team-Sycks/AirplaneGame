using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;

public class OpenSkyNetwork : MonoBehaviour
{
    private String api = "https://opensky-network.org/api/flights/arrival?airport=EFHK&begin={0}&end={1}";
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
        long yesterday = new DateTimeOffset(System.DateTime.Now.AddDays(-1)).ToUnixTimeSeconds();
        long currentTime = new DateTimeOffset(System.DateTime.Now).ToUnixTimeSeconds();

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(api, yesterday, currentTime));
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

        Debug.Log(System.DateTime.Now + "");

        foreach (Plane plane in planes)
        {
            Debug.Log(plane.callsign + ", " + UnixTimeStampToDateTime(plane.firstSeen) + ", " + UnixTimeStampToDateTime(plane.lastSeen) +  ", " +plane.estDepartureAirport + ", " + plane.estArrivalAirport);
        }
        return planes;
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }

    [Serializable]
    public class PlaneList
    {
        public List<Plane> planes;
    }
    [Serializable]
    public class Plane
    {
        public String callsign;
        public int firstSeen;
        public int lastSeen;
        public String estDepartureAirport;
        public String estArrivalAirport;
    }
}
