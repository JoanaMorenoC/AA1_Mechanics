using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPhysics : MonoBehaviour
{
    [SerializeField] float mass;
    public static float stepTime = 0.001f;

    Vector3 position;
    [SerializeField] Vector3 velocity;
    Vector3 acceleration;

    static float G = 39.478f;

    GravityPhysics[] otherPlanets;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position; 
        otherPlanets = transform.parent.GetComponentsInChildren<GravityPhysics>();
        Debug.Log("OtherPlanets size "+ otherPlanets.Length);
        Debug.Log("Este objeto es:"+gameObject.name + "  Ha encontrado: " + otherPlanets[0].gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = position + velocity * stepTime + acceleration * (stepTime * stepTime * 0.5f);
        Vector3 newAcceleration = CalculateAcceleration();
        Vector3 newVelocity = velocity + (acceleration + newAcceleration) * (stepTime * 0.5f);
 
        position = newPosition;
        velocity = newVelocity;
        acceleration = newAcceleration;

        transform.position = position;
    }

    Vector3 CalculateGravitationForce(GravityPhysics other)
    {
        Vector3 forceDirection = other.GetPosition() - position;
        float distance = forceDirection.magnitude;
        forceDirection = forceDirection / forceDirection.magnitude;

        float forceMagnitude = (G * other.GetMass() * mass) / (distance * distance);

        return forceMagnitude * forceDirection;
    }

    Vector3 CalculateAcceleration()
    {
        Vector3 sumForces = Vector3.zero;
        for (int i = 0; i < otherPlanets.Length; i ++)
        {
            if (otherPlanets[i] == this)
                continue;

            sumForces += CalculateGravitationForce(otherPlanets[i]);
        }

        Vector3 acceleration = sumForces / mass;

        return acceleration;
    }

    Vector3 GetPosition()
    {
        return position;
    }

    float GetMass()
    { 
        return mass;
    }
}
