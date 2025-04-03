using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SimulationSettings : MonoBehaviour
{
    public static SimulationSettings Instance { get; private set; }
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
        if (Instance != null && Instance != this)
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


    public float GetStepTime()
    {
        return baseStepTime * simulationSpeed;
    }

    void AssignTrailSize()
    {
        if (mainCamera.transform.position.magnitude > distanceToChangeTrails && !bigTrailsActivated)
        {
            bigTrailsActivated = true;
            for (int i = 0; i < planetTrails.Count; i++)
            {
                AnimationCurve curve = new AnimationCurve(planetTrails[i].widthCurve.keys);

                if (curve.keys.Length > 0 && curve.keys[0].value < minimumSizeToChangeTrailSize)
                {
                    curve.RemoveKey(0);
                    curve.AddKey(0f, newTrailSize);

                    planetTrails[i].widthCurve = curve;
                }
            }
        }
        else if (mainCamera.transform.position.magnitude <= distanceToChangeTrails && bigTrailsActivated)
        {
            bigTrailsActivated = false;
            for (int i = 0; i < planetTrails.Count; i++)
            {
                AnimationCurve curve = new AnimationCurve(planetTrails[i].widthCurve.keys);

                if (curve.keys.Length > 0 && curve.keys[0].value == newTrailSize)
                {
                    curve.RemoveKey(0);
                    curve.AddKey(0f, planetTrails[i].transform.localScale.x);

                    planetTrails[i].widthCurve = curve;
                }
            }
        }
    }
}
