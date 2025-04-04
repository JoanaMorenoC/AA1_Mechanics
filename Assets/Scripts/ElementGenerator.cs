using UnityEngine;

public class ElementGenerator : MonoBehaviour
{
    //Initialization of all the variables necessaries for making the asteroids
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
        if (Input.GetMouseButtonDown(0) && simulationSettings.simulationSpeed > 0.01f) //Click left + Check speed simulation
        {
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        ElementSettingsScript.ElementSettings asteroidSettings; //Initialization asteroid with the controller of objects

        //Initialization of all the values necessaries for making the asteroids

        Vector3 position = mainCamera.transform.position + mainCamera.transform.forward * spawningPositionOffset; //Initial position in front of the camera
        asteroidSettings.startingPosition = position;
        
        float speed = Random.Range(minSpeed, maxSpeed);
        Vector3 velocity = mainCamera.transform.forward * speed; // Direction of the movement of the asteroid
        asteroidSettings.startingVelocity = velocity;

        asteroidSettings.mass = Random.Range(minMass, maxMass);

        GameObject asteroid = Instantiate(asteroidPrefab); //Creation of the object
        //Initializaction values to the asteroid
        asteroid.GetComponent<ElementSettingsScript>().SetSettings(asteroidSettings);
        asteroid.transform.SetParent(solarSystem.transform, true);
        asteroid.transform.position = asteroidSettings.startingPosition;
        asteroid.GetComponent<GravityPhysics>().enabled = false;
        asteroid.GetComponent<GravityPhysics>().enabled = true;

    }
}
