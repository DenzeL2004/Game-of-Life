using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartStopButton : MonoBehaviour
{
    [Header("References")]
    public GameOfLifeController gameController;
    
    [Header("Button Texts")]
    public string startText = "Start";
    public string stopText = "Stop";
    
    private Button button;
    private TextMeshProUGUI buttonText;
    private bool lastSimulationState;

    void Start()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>(); 
        
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameOfLifeController>();
        }
        
        if (gameController == null)
        {
            Debug.LogError("GameOfLifeController not found!", this);
            return;
        }
        
        if (button == null)
        {
            Debug.LogError("Button component not found!", this);
            return;
        }
        
        if (buttonText == null)
        {
            Debug.LogError("TextMeshPro component not found in button!", this);
            return;
        }
        
        button.onClick.AddListener(OnButtonClick);
        
        lastSimulationState = gameController.IsSimulationRunning;
        UpdateButtonText();
        
        Debug.Log("StartStopButton initialized successfully ");
    }

    void Update()
    {
        if (gameController != null)
        {
            bool currentState = gameController.IsSimulationRunning;
            
            if (currentState != lastSimulationState)
            {
                lastSimulationState = currentState;
                UpdateButtonText();
            }
        }
    }

    void OnButtonClick()
    {
        if (gameController != null) 
        {
            gameController.ToggleSimulation();
        }
    }

    void UpdateButtonText()
    {
        if (buttonText != null && gameController != null) 
        {
            buttonText.text = gameController.IsSimulationRunning ?  startText : stopText;
        }
    }

    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
}