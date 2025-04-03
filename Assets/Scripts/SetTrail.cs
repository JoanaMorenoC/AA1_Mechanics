using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrail : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TrailRenderer>().startWidth = transform.localScale.x;
        GetComponent<TrailRenderer>().endWidth = 0;

    }
}
