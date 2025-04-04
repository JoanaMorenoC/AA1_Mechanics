using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI simulationSpeedText;

    void Start()
    {
        simulationSpeedText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();    
    }
    void Update()
    {
        simulationSpeedText.text = "\nSimulation speed: " + SimulationSettings.Instance.simulationSpeed.ToString("F1") + "    "; //Update visible number speed
    }
}
