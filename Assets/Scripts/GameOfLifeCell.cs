using UnityEngine;

public class GameOfLifeCell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    public bool IsAlive { get; private set; }
    public bool NextState { get; set; }
    
    public int GridX { get; set; }
    public int GridY { get; set; }
    public GameOfLifeController Controller { get; set; }

    [Header("Visual Settings")]
    public Color aliveColor = Color.red;
    public Color deadColor = Color.black;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on cell object!");
        }
    }

    public void SetNextState(bool alive)
    {
        NextState = alive;
    }

    public void ApplyNextState()
    {
        IsAlive = NextState;
        UpdateVisualAppearance();
    }

    private void UpdateVisualAppearance()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = IsAlive ? aliveColor : deadColor;
        }
    }

    private void OnMouseDown()
    {
        if (Controller != null && Controller.IsSimulationRunning)
            return;

        bool newState = !IsAlive;
        SetNextState(newState);
        ApplyNextState();
    }
}