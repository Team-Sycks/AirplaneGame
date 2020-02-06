using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class new_movement : MonoBehaviour
{
    public GameObject g;
    public Transform target;
    Vector3[] points;
    int index;
    int i;
    void Start()
    {
        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.transform.position = new Vector3(-5, 0, -3);
       
        points = new Vector3[4];
        points[0] = new Vector3(3, 0, 3);
        points[2] = new Vector3(-4, 0, -4);
        points[1] = new Vector3(4, 0, 2);
        points[3] = new Vector3(2, 0, -1);
    }
    void Update()
    {
        g.transform.position = Vector3.MoveTowards(g.transform.position,
                                                          points[index], Time.deltaTime * 2);

        // Looks at points
        g.transform.LookAt(points[0]);


        // Alla olevan Linen poistaminen --> Gameobject liikkuu vain ensimmäiseen pointtiin
        if ( (g.transform.position - points[index]).sqrMagnitude < 0.01) { index++; if (index > points.Length - 1) { index = 0; } }
    
    // if (g.transform.position == points[index]) { Application.Quit(); }




}
}
