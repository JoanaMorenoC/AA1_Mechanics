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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        ElementSettingsScript.ElementSettings asteroidSettings;

        asteroidSettings.startingPosition = mainCamera.transform.position;
        
        float speed = Random.Range(minSpeed, maxSpeed);
        Vector3 velocity = mainCamera.transform.forward * speed;
        asteroidSettings.startingVelocity = velocity;

        asteroidSettings.mass = Random.Range(minMass, maxMass);

        GameObject asteroid = Instantiate(asteroidPrefab);
        asteroid.transform.SetParent(solarSystem.transform, true);
        asteroid.transform.position = asteroidSettings.startingPosition;
        Debug.Log("Posicion seteada: " + asteroid.transform.position);
        asteroid.GetComponent<ElementSettingsScript>().SetSettings(asteroidSettings);
        asteroid.GetComponent<GravityPhysics>().enabled = false;
        asteroid.GetComponent<GravityPhysics>().enabled = true;

    }
}
