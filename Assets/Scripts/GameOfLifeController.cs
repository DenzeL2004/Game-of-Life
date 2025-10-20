using System.Collections;
using UnityEngine;

public class GameOfLifeController : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public GameObject cellPrefab;
    public float cellSpacing = 0.1f;
    public float cellScale = 0.5f;
    public Vector2 gridOffset = new Vector2(0f, 0.5f);

    [Header("Simulation Settings")]
    public float simulationSpeed = 1f;
    private bool isSimulationRunning = false;

    private GameOfLifeCell[,] grid;
    public bool IsSimulationRunning => isSimulationRunning;

    void Start()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        if (grid != null)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        grid = new GameOfLifeCell[width, height];

        float gridWidth = width * (cellScale + cellSpacing) - cellSpacing;
        float gridHeight = height * (cellScale + cellSpacing) - cellSpacing;
        Vector2 startPosition = new Vector2(-gridWidth / 2f, -gridHeight / 2f) + gridOffset;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(
                    startPosition.x + x * (cellScale + cellSpacing),
                    startPosition.y + y * (cellScale + cellSpacing),
                    0
                );

                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity);
                cellObj.transform.parent = transform;
                cellObj.transform.localScale = new Vector3(cellScale, cellScale, 1f);

                GameOfLifeCell cell = cellObj.GetComponent<GameOfLifeCell>();
                cell.GridX = x;
                cell.GridY = y;
                cell.Controller = this;
                
                grid[x, y] = cell;
            }
        }
    }

    private int CountAliveNeighbors(int x, int y)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int neighborX = x + i;
                int neighborY = y + j;

                if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
                    if (grid[neighborX, neighborY].IsAlive)
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    private void CalculateNextGeneration()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int aliveNeighbors = CountAliveNeighbors(x, y);
                bool currentlyAlive = grid[x, y].IsAlive;

                if (currentlyAlive && (aliveNeighbors == 2 || aliveNeighbors == 3))
                {
                    grid[x, y].SetNextState(true);
                }
                else if (!currentlyAlive && aliveNeighbors == 3)
                {
                    grid[x, y].SetNextState(true);
                }
                else
                {
                    grid[x, y].SetNextState(false);
                }
            }
        }
    }

    private void ApplyNextGeneration()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].ApplyNextState();
            }
        }
    }

    private IEnumerator SimulationCoroutine()
    {
        while (isSimulationRunning)
        {
            yield return new WaitForSeconds(1f / simulationSpeed);
            
            CalculateNextGeneration();
            ApplyNextGeneration();
        }
    }

    public void ToggleSimulation()
    {
        isSimulationRunning = !isSimulationRunning;

        if (isSimulationRunning)
        {
            StartCoroutine(SimulationCoroutine());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void SetSimulationSpeed(float speed)
    {
        simulationSpeed = speed;
        
        if (isSimulationRunning)
        {
            StopAllCoroutines();
            StartCoroutine(SimulationCoroutine());
        }
    }

    public void ClearGrid()
    {
        if (isSimulationRunning) ToggleSimulation();

        foreach (var cell in grid)
        {
            cell.SetNextState(false);
            cell.ApplyNextState();
        }
    }

    public void RandomizeGrid()
    {
        if (isSimulationRunning) ToggleSimulation();

        foreach (var cell in grid)
        {
            cell.SetNextState(Random.value > 0.7f);
            cell.ApplyNextState();
        }
    }
}