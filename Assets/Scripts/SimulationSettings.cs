using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimulationSettings : MonoBehaviour
{
    public static SimulationSettings Instance { get; private set; }
    public float simulationSpeed = 1.0f;
    [SerializeField] GameObject mainCamera;
    [SerializeField] List<TrailRenderer> planets;
    float baseStepTime = 0.0001f;
    float distanceToChangeTrails = 4f;

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


    }


    public float GetStepTime()
    {
        return baseStepTime * simulationSpeed;
    }
}
