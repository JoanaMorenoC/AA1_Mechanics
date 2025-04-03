using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class CameraMovement : MonoBehaviour
{
    public float rotationSensibility;
    public float movementSpeed;
    public float transitionSpeed = 5f;
    public float zoomSpeed;

    Vector2 rotation;

    bool isInTransition = false;
    Vector3 targetPosition;
    Quaternion targetRotation;
    float lookAtAccuracyMargin = 0.001f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isInTransition)
        {
            HandleTransition();
        }
        else
        {
            HandleRotation();
            HandleMovement();
        }

        HandleInput();
    }

    void HandleTransition()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * transitionSpeed);

        if (Vector3.Distance(transform.position, targetPosition) < lookAtAccuracyMargin && Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            Vector3 angles = transform.eulerAngles;
            rotation.x = angles.y;
            rotation.y = angles.x;
            isInTransition = false;
        }
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSensibility;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSensibility;

        if (transform.up.y >= 0f)
            rotation.x += mouseX;
        else
            rotation.x -= mouseX;

        rotation.y -= mouseY;

        transform.rotation = Quaternion.Euler(rotation.y, rotation.x, 0);
    }

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

        Vector3 movement = Vector3.zero;
        movement.x = movementDirection.x * movementSpeed * Time.deltaTime;
        movement.y = movementDirection.y * movementSpeed * Time.deltaTime;
        movement.z = movementDirection.z * movementSpeed * Time.deltaTime;

        transform.position += movement;

        HandleZoom();
    }

    void HandleZoom()
    {
        float zoom = 0.0f;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            zoom += zoomSpeed;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            zoom -= zoomSpeed;

        Vector3 zoomVector = transform.forward * zoom;
        transform.position += zoomVector;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isInTransition)
            LookAt(Vector3.zero);

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isInTransition)
            SetPerspective(new Vector3(0, 3, 0), Vector3.zero);
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isInTransition)
            SetPerspective(new Vector3(-2, 2.5f, -4.5f), Vector3.zero);
    }

    void LookAt(Vector3 target)
    {
        targetRotation = Quaternion.LookRotation(target - transform.position);
        isInTransition = true;
        targetPosition = transform.position;
    }

    void SetPerspective(Vector3 position, Vector3 lookAtPoint)
    {
        targetPosition = position;
        targetRotation = Quaternion.LookRotation(lookAtPoint - position);
        isInTransition = true;
    }

    Vector3 GetMovementDirection(Vector3 input)
    {
        Vector3 movementDirection = Vector3.zero;

        movementDirection = transform.forward * input.z + transform.right * input.x + transform.up * input.y;
        movementDirection = movementDirection.normalized;

        return movementDirection;
    }
}
