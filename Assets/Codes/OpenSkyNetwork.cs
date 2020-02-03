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

    private string GetAirPlane()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(api));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        
        return jsonResponse;
    }
}
