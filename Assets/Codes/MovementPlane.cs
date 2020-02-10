using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlane : MonoBehaviour
    
{
    public Transform target;
    Vector3[] points;
    int index;
    int i;
    public float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[4];
        points[0] = new Vector3(8, 0, 5);
        points[1] = new Vector3(4, 0, 9);
    }
    
    // Update is called once per frame
    void Update()
    {
        //Change Time.deltaTime *
        transform.position = Vector3.MoveTowards(transform.position,
                                                          target.position,speed * Time.deltaTime * 20);
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;

        transform.rotation = rotation * Quaternion.Euler(0, 360, 0);
    }
}
