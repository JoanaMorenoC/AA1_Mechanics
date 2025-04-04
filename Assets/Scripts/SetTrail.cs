using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrail : MonoBehaviour
{
    void Start()
    {
        //Create a trail
        GetComponent<TrailRenderer>().startWidth = transform.localScale.x;
        GetComponent<TrailRenderer>().endWidth = 0;

    }
}
