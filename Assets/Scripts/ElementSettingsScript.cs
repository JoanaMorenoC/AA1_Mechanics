using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSettingsScript : MonoBehaviour
{
    public GravityPhysics physics;

    float massToSizeMultiplier = 0.01f;

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
        float size = 0.01f;//mass * massToSizeMultiplier;

        transform.localScale = Vector3.one * size;
    }
}
