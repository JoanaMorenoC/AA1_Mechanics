using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float rotationSensibility;
    public float movementSpeed;

    Vector2 rotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        ManageRotation();
        ManageMovement();
    }

    void ManageRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSensibility;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSensibility;

        rotation.x += mouseX;
        rotation.y -= mouseY;

        transform.rotation = Quaternion.Euler(rotation.y, rotation.x, 0);
    }

    void ManageMovement()
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

        Vector3 movementDirection = transform.forward * input.z + transform.right * input.x + transform.up * input.y;
        movementDirection = movementDirection.normalized;

        Vector3 movement = Vector2.zero;
        movement.x = movementDirection.x * movementSpeed * Time.deltaTime;
        movement.y = movementDirection.y * movementSpeed * Time.deltaTime;
        movement.z = movementDirection.z * movementSpeed * Time.deltaTime;


        transform.position = new Vector3(transform.position.x + movement.x, transform.position.y + movement.y, transform.position.z + movement.z);
    }
}
