using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI simulationSpeedText;

    // Start is called before the first frame update
    void Start()
    {
        simulationSpeedText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();    
    }

    // Update is called once per frame
    void Update()
    {
        simulationSpeedText.text = "\nSimulation speed: " + SimulationSettings.Instance.simulationSpeed.ToString("F1") + "    ";
    }
}
