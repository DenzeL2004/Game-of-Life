using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private Slider slider;
    private GameOfLifeController gameController;

    void Start()
    {
        slider = GetComponent<Slider>();
        gameController = FindAnyObjectByType<GameOfLifeController>(); 
        
        if (slider == null)
        {
            Debug.LogError("Slider component not found!");
            return;
        }
        
        if (gameController == null)
        {
            Debug.LogError("GameOfLifeController not found in scene!");
            return;
        }

        slider.minValue = 0.1f;
        slider.maxValue = 10f;
        slider.value = 1f;
        slider.wholeNumbers = false;

        slider.onValueChanged.AddListener(OnSliderValueChanged);
        
        Debug.Log("SpeedSliderController initialized successfully");
    }

    void OnSliderValueChanged(float value)
    {
        if (gameController != null)
        {
            gameController.SetSimulationSpeed(value);
        }
    }

    void OnDestroy()
    {
        if (slider != null)
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}
