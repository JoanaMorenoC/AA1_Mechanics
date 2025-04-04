using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class CameraMovement : MonoBehaviour
{
    enum Planet { MERCURY, VENUS, EARTH, MARS, JUPITER, SATURN, URANUS, NEPTUNE, NONE = -1 }; // Enumeration for planets
    Planet focusedPlanet = Planet.NONE;

    // Camera and movement settings
    public float rotationSensibility;
    public float movementSpeed;
    public float transitionSpeed = 5f;
    public float zoomSpeed;

    Vector2 rotation; // Stores camera rotation values

    bool isInTransition = false;
    Vector3 targetPosition;
    Quaternion targetRotation;
    float lookAtAccuracyMargin = 0.001f; // Threshold to determine when transition ends

    [SerializeField] Camera cameraComponent; 
    [SerializeField] List<GameObject> planets;

    void Start()
    {
        // Lock and hide the cursor for better camera control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (focusedPlanet != Planet.NONE)
        {
            // Focus the camera on a specific planet
            int index = (int)focusedPlanet;
            float fieldOfView = planets[index].transform.localScale.x * 1.5f / 0.003f; // Adjust FOV based on planet size
            cameraComponent.fieldOfView = fieldOfView;
            transform.position = planets[index].transform.position + new Vector3(0.0f, 0.3f, -0.3f);
            transform.rotation = Quaternion.LookRotation(planets[index].transform.position - transform.position);
        }
        else
        {
            cameraComponent.fieldOfView = 60f; // Default FOV when not focusing on a planet

            if (isInTransition)
            {
                HandleTransition(); // Smooth transition to a new position
            }
            else
            {
                HandleRotation();
                HandleMovement();
            }
        }

        HandleInput();
    }

    // Smoothly transitions the camera to a new position and orientation
    void HandleTransition()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * transitionSpeed);

        // End transition if the camera is close enough to the target position and rotation
        if (Vector3.Distance(transform.position, targetPosition) < lookAtAccuracyMargin &&
            Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            Vector3 angles = transform.eulerAngles;
            rotation.x = angles.y;
            rotation.y = angles.x;
            isInTransition = false;
        }
    }

    // Handles camera rotation based on mouse movement
    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSensibility;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSensibility;

        // Adjust rotation direction based on camera orientation
        if (transform.up.y >= 0f)
            rotation.x += mouseX;
        else
            rotation.x -= mouseX;

        rotation.y -= mouseY;

        transform.rotation = Quaternion.Euler(rotation.y, rotation.x, 0);
    }

    // Handles movement based on keyboard input
    void HandleMovement()
    {
        Vector3 input = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            input.z += 1;
        if (Input.GetKey(KeyCode.S))
            input.z -= 1;
        if (Input.GetKey(KeyCode.A))
            input.x -= 1;
        if (Input.GetKey(KeyCode.D))
            input.x += 1;
        if (Input.GetKey(KeyCode.Space))
            input.y += 1;
        if (Input.GetKey(KeyCode.LeftShift))
            input.y -= 1;

        Vector3 movementDirection = GetMovementDirection(input);

        Vector3 movement = movementDirection * movementSpeed * Time.deltaTime;
        transform.position += movement;

        HandleZoom();
    }

    // Handles zooming in and out using the mouse scroll wheel
    void HandleZoom()
    {
        float zoom = 0.0f;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            zoom += zoomSpeed;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            zoom -= zoomSpeed;

        transform.position += transform.forward * zoom;
    }

    // Handles key inputs for setting perspectives or switching focus
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isInTransition) // Center view in direction to the sun
            LookAt(Vector3.zero);

        // Change perspective based on number keys
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isInTransition)
        {
            SetPerspective(new Vector3(0, 3, 0), Vector3.zero);
            focusedPlanet = Planet.NONE;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isInTransition)
        {
            SetPerspective(new Vector3(-2, 2.5f, -4.5f), Vector3.zero);
            focusedPlanet = Planet.NONE;
        }

        // Cycle through planet views
        if (Input.GetKeyDown(KeyCode.Alpha3) && !isInTransition)
        {
            focusedPlanet = (focusedPlanet == Planet.NEPTUNE) ? 0 : focusedPlanet + 1;
        }
    }

    // Smoothly rotates the camera to look at a target point
    void LookAt(Vector3 target)
    {
        targetRotation = Quaternion.LookRotation(target - transform.position);
        isInTransition = true;
        targetPosition = transform.position;
    }

    // Sets the camera position and orientation for a fixed perspective
    void SetPerspective(Vector3 position, Vector3 lookAtPoint)
    {
        targetPosition = position;
        targetRotation = Quaternion.LookRotation(lookAtPoint - position);
        isInTransition = true;
    }

    // Converts input into a movement direction
    Vector3 GetMovementDirection(Vector3 input)
    {
        Vector3 movementDirection = transform.forward * input.z + transform.right * input.x + transform.up * input.y;
        return movementDirection.normalized;
    }
}
