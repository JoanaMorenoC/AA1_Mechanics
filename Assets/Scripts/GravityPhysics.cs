using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPhysics : MonoBehaviour
{
    [SerializeField] float massInSolarMassUnits;

    Vector3 position; // Position in astronomical units
    [SerializeField] Vector3 velocityInAuPerYear;

    static float G = 39.478f; // Gravitational constant in astronomical units

    GravityPhysics[] otherPlanets; // Other planets inside the solar system, needed to calculate all interactions

    void Start()
    {
        position = transform.position;
        otherPlanets = transform.parent.GetComponentsInChildren<GravityPhysics>(); 
    }

    void Update()
    {
        RungeKuttaIntegration(SimulationSettings.Instance.GetStepTime());

        transform.position = position;
    }

    //4th order Runge-Kutta (RK4)
    void RungeKuttaIntegration(float h)
    {
        Vector3 k1_v = velocityInAuPerYear;
        Vector3 k1_a = CalculateAccelerationAt(position);

        Vector3 k2_v = velocityInAuPerYear + k1_a * (h * 0.5f);
        Vector3 k2_a = CalculateAccelerationAt(position + k1_v * (h * 0.5f));

        Vector3 k3_v = velocityInAuPerYear + k2_a * (h * 0.5f);
        Vector3 k3_a = CalculateAccelerationAt(position + k2_v * (h * 0.5f));

        Vector3 k4_v = velocityInAuPerYear + k3_a * h;
        Vector3 k4_a = CalculateAccelerationAt(position + k3_v * h);

        velocityInAuPerYear += (k1_a + 2f * k2_a + 2f * k3_a + k4_a) * (h / 6f);
        position += (k1_v + 2f * k2_v + 2f * k3_v + k4_v) * (h / 6f);
    }

    Vector3 CalculateAccelerationAt(Vector3 pos)
    {
        // Get the sum of forces that others planets of the solar system apply to this planet
        Vector3 sumForces = Vector3.zero;
        for (int i = 0; i < otherPlanets.Length; i++)
        {
            if (otherPlanets[i] == this)
                continue;

            sumForces += CalculateGravitationForce(otherPlanets[i], pos);
        }

        Vector3 acceleration = sumForces / massInSolarMassUnits; // Calculate the acceleration from the sum of gravitational forces

        return acceleration;
    }

    Vector3 CalculateGravitationForce(GravityPhysics other, Vector3 pos)
    {
        Vector3 forceDirection = other.GetPosition() - pos;
        float distance = forceDirection.magnitude;
        forceDirection = forceDirection / distance;

        float forceMagnitude = (G * other.GetMass() * massInSolarMassUnits) / (distance * distance); // Gravitational force formula

        return forceMagnitude * forceDirection;
    }

    Vector3 GetPosition()
    {
        return position;
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = position;
    }

    float GetMass()
    {
        return massInSolarMassUnits;
    }

    public void SetMass(float newMass)
    {
        massInSolarMassUnits = newMass;
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        velocityInAuPerYear = newVelocity;
    }

}
