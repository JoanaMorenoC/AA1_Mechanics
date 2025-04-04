using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSettingsScript : MonoBehaviour
{
    public GravityPhysics physics; // Controller gravity

    float massToSizeMultiplier = 10000000f; //Multiplier for creating the size of the asteroids

    //Values needed for creating all the objects of the simulation
    public struct ElementSettings
    {
        public Vector3 startingPosition;
        public Vector3 startingVelocity;
        public float mass;
    }

    public void SetSettings(ElementSettings settings)
    {
        physics.SetPosition(settings.startingPosition);
        physics.SetVelocity(settings.startingVelocity);
        physics.SetMass(settings.mass);

        SetSize(settings.mass);
    }

    void SetSize(float mass)
    {
        float size = mass * massToSizeMultiplier;

        transform.localScale = Vector3.one * size;
    }
}
