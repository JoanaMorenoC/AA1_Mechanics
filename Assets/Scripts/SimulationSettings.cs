using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SimulationSettings : MonoBehaviour
{
    public static SimulationSettings Instance { get; private set; } // Create the instance to use this script easily in other scripts

    //Initialice values variable
    public float simulationSpeed = 1.0f;
    [SerializeField] GameObject mainCamera;
    [SerializeField] List<TrailRenderer> planetTrails;
    float baseStepTime = 0.0001f;

    float distanceToChangeTrails = 4f;
    float minimumSizeToChangeTrailSize = 0.1f;
    float newTrailSize = 0.1f;
    public bool bigTrailsActivated = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) //It is an instance so we need only one in the scene
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        //Control the speed of the simulation by changing the time that passed inside
        if (Input.GetKeyDown(KeyCode.LeftArrow) && simulationSpeed > 0)
        {
            if (simulationSpeed <= 1 )
                simulationSpeed -= 0.1f;
            else
                simulationSpeed -= 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (simulationSpeed < 1)
                simulationSpeed += 0.1f;
            else
                simulationSpeed += 0.5f;
        }

        AssignTrailSize();
    }

    //Get the time that passed in the simulation
    public float GetStepTime()
    {
        return baseStepTime * simulationSpeed;
    }

    void AssignTrailSize()
    {
        //Change the trail of the planets or the Moon depending of the position of the camera
        if (mainCamera.transform.position.magnitude > distanceToChangeTrails && !bigTrailsActivated)
        {
            //Increase the width of the trail
            bigTrailsActivated = true;
            for (int i = 0; i < planetTrails.Count; i++)
            {
                AnimationCurve curve = new AnimationCurve(planetTrails[i].widthCurve.keys);

                if (curve.keys.Length > 0 && curve.keys[0].value < minimumSizeToChangeTrailSize) //Check if we made the change of width 
                {
                    curve.RemoveKey(0); // Delete value trail
                    curve.AddKey(0f, newTrailSize); // New value trail

                    planetTrails[i].widthCurve = curve; //Change width trail
                }
            }
        }
        else if (mainCamera.transform.position.magnitude <= distanceToChangeTrails && bigTrailsActivated)
        {
            //Return to the original values of width in the trail
            bigTrailsActivated = false;
            for (int i = 0; i < planetTrails.Count; i++)
            {
                AnimationCurve curve = new AnimationCurve(planetTrails[i].widthCurve.keys);

                if (curve.keys.Length > 0 && curve.keys[0].value == newTrailSize) //Check if we made the change of width 
                {
                    curve.RemoveKey(0); // Delete value trail
                    curve.AddKey(0f, planetTrails[i].transform.localScale.x); // New value trail

                    planetTrails[i].widthCurve = curve; //Change width trail
                }
            }
        }
    }
}
