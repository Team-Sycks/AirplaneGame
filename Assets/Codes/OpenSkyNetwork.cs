using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using System.Globalization;

public class OpenSkyNetwork : MonoBehaviour
{
    public GameObject planePrefab;
    private List<Airport> airports;

    private String api = "https://opensky-network.org/api/flights/arrival?airport=EFHK&begin={0}&end={1}";
    // Start is called before the first frame update
    void Start()
    {
        readAirportCSV();
        createPlanes(GetAirPlanesFromREST());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<Plane> GetAirPlanesFromREST()
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
        for (int i = planes.Count - 1; i > -1; i--)
        {
            Plane plane = planes[i];
            if (plane.estArrivalAirport.Equals("") || plane.estDepartureAirport.Equals(""))
            {
                planes.RemoveAt(i);
            }
        }

        return planes;
    }

    private void createPlanes(List<Plane> planes)
    {


        //Create planes on the map
        foreach (Plane plane in planes)
        {
            ///Add coordinates here
            GameObject planeObject = Instantiate(planePrefab, new Vector3(0, 0.01f, 0), Quaternion.identity);
            planeObject.transform.Rotate(-90, 0, 0);
            PlaneLogic planeLogic = planeObject.GetComponent<PlaneLogic>();
            planeLogic.departureTime = plane.firstSeen;
            planeLogic.arrivalTime = plane.lastSeen;

            foreach (Airport airport in airports)
            {
                if (airport.airportCode.Equals(plane.estDepartureAirport))
                {
                    Debug.Log(airport.airportName + ", " + airport.latitude + ", " + airport.longitude);
                    planeLogic.latitude = airport.latitude;
                    planeLogic.longitude = airport.longitude;
                    break;
                }
            }
        }
    }

    private void readAirportCSV()
    {
        String fileData = System.IO.File.ReadAllText(Application.dataPath + "/Data/airports.csv");
        String[] lines = fileData.Split(char.Parse("\n"));

        airports = new List<Airport>();

        foreach (String s in lines)
        {
            String[] sLines = s.Split(char.Parse(","));

            if (sLines.Length > 7)
            {
                String name = sLines[1].Trim('"');
                String airportCode = sLines[5].Trim('"');
                String longitude = sLines[7];
                String latitude = sLines[6];

                try
                {
                    Airport airport = new Airport();

                    airport.airportName = name;
                    airport.airportCode = airportCode;
                    airport.longitude = float.Parse(longitude, CultureInfo.InvariantCulture.NumberFormat);
                    airport.latitude = float.Parse(latitude, CultureInfo.InvariantCulture.NumberFormat);
                    airports.Add(airport);
                }
                catch (FormatException e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
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

    [Serializable]
    public class Airport
    {
        public String airportName;
        public String airportCode;
        public float longitude;
        public float latitude;
    }
}
