using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPhysics : MonoBehaviour
{
    [SerializeField] float massInSolarMassUnits;

    Vector3 position;
    [SerializeField] Vector3 velocityInUaPerYear;

    static float G = 39.478f;

    GravityPhysics[] otherPlanets;

    void Start()
    {
        position = transform.position;
        otherPlanets = transform.parent.GetComponentsInChildren<GravityPhysics>();
        Debug.Log("OtherPlanets size " + otherPlanets.Length);
        Debug.Log("Este objeto es:" + gameObject.name + "  Ha encontrado: " + otherPlanets[0].gameObject.name);
        Debug.Log(gameObject.name + "  puesto en posición " + transform.position);

    }

    void Update()
    {
        RungeKuttaIntegration(SimulationSettings.Instance.GetStepTime());

        transform.position = position;
    }

    void RungeKuttaIntegration(float h)
    {
        Vector3 k1_v = velocityInUaPerYear;
        Vector3 k1_a = CalculateAcceleration();

        Vector3 k2_v = velocityInUaPerYear + k1_a * (h * 0.5f);
        Vector3 k2_a = CalculateAccelerationAt(position + k1_v * (h * 0.5f));

        Vector3 k3_v = velocityInUaPerYear + k2_a * (h * 0.5f);
        Vector3 k3_a = CalculateAccelerationAt(position + k2_v * (h * 0.5f));

        Vector3 k4_v = velocityInUaPerYear + k3_a * h;
        Vector3 k4_a = CalculateAccelerationAt(position + k3_v * h);

        velocityInUaPerYear += (k1_a + 2f * k2_a + 2f * k3_a + k4_a) * (h / 6f);
        position += (k1_v + 2f * k2_v + 2f * k3_v + k4_v) * (h / 6f);
    }

    Vector3 CalculateAccelerationAt(Vector3 pos)
    {
        Vector3 sumForces = Vector3.zero;
        for (int i = 0; i < otherPlanets.Length; i++)
        {
            if (otherPlanets[i] == this)
                continue;

            sumForces += CalculateGravitationForce(otherPlanets[i], pos);
        }

        Vector3 acceleration = sumForces / massInSolarMassUnits;

        return acceleration;
    }

    Vector3 CalculateGravitationForce(GravityPhysics other, Vector3 pos)
    {
        Vector3 forceDirection = other.GetPosition() - pos;
        float distance = forceDirection.magnitude;
        forceDirection = forceDirection / distance;

        float forceMagnitude = (G * other.GetMass() * massInSolarMassUnits) / (distance * distance);

        return forceMagnitude * forceDirection;
    }

    Vector3 CalculateAcceleration()
    {
        Vector3 sumForces = Vector3.zero;
        for (int i = 0; i < otherPlanets.Length; i++)
        {
            if (otherPlanets[i] == this)
                continue;

            sumForces += CalculateGravitationForce(otherPlanets[i], position);
        }

        return sumForces / massInSolarMassUnits;
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
        velocityInUaPerYear = newVelocity;
    }

}
