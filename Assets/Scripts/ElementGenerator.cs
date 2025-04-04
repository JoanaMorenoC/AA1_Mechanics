using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementGenerator : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject solarSystem;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float minMass;
    [SerializeField] float maxMass;
    [SerializeField] SimulationSettings simulationSettings;

    float spawningPositionOffset = 0.301f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && simulationSettings.GetStepTime() > 0.01f)
        {
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        ElementSettingsScript.ElementSettings asteroidSettings;

        Vector3 position = mainCamera.transform.position + mainCamera.transform.forward * spawningPositionOffset;
        asteroidSettings.startingPosition = position;
        
        float speed = Random.Range(minSpeed, maxSpeed);
        Vector3 velocity = mainCamera.transform.forward * speed;
        asteroidSettings.startingVelocity = velocity;

        asteroidSettings.mass = Random.Range(minMass, maxMass);

        GameObject asteroid = Instantiate(asteroidPrefab);
        asteroid.GetComponent<ElementSettingsScript>().SetSettings(asteroidSettings);
        asteroid.transform.SetParent(solarSystem.transform, true);
        asteroid.transform.position = asteroidSettings.startingPosition;
        Debug.Log("Posicion seteada: " + asteroid.transform.position);
        asteroid.GetComponent<GravityPhysics>().enabled = false;
        asteroid.GetComponent<GravityPhysics>().enabled = true;

    }
}
