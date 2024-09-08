namespace GameOfLife;

/// <summary>
/// Represents Conway's Game of Life in a sequential version.
/// The class provides methods to simulate the game's evolution based on simple rules.
/// </summary>
public sealed class GameOfLifeSequentialVersion
{
    private readonly bool[,] initialGrid;
    private bool[,] currentGrid;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameOfLifeSequentialVersion"/> class with the specified number of rows and columns. The initial state of the grid is randomly set with alive or dead cells.
    /// </summary>
    /// <param name="rows">The number of rows in the grid.</param>
    /// <param name="columns">The number of columns in the grid.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of rows or columns is less than or equal to 0.</exception>
    public GameOfLifeSequentialVersion(int rows, int columns)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rows);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(columns);

        this.initialGrid = new bool[rows, columns];
        this.currentGrid = new bool[rows, columns];
        Random random = new Random();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                this.currentGrid[i, j] = random.Next(2) == 0;
            }
        }

        Array.Copy(this.currentGrid, this.initialGrid, this.currentGrid.Length);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameOfLifeSequentialVersion"/> class with the given grid.
    /// </summary>
    /// <param name="grid">The 2D array representing the initial state of the grid.</param>
    /// <exception cref="ArgumentNullException">Thrown when the input grid is null.</exception>
    public GameOfLifeSequentialVersion(bool[,] grid)
    {
        ArgumentNullException.ThrowIfNull(grid);

        int rows = grid.GetLength(0);
        int columns = grid.GetLength(1);
        this.initialGrid = new bool[rows, columns];
        this.currentGrid = new bool[rows, columns];

        Array.Copy(grid, this.currentGrid, grid.Length);
        Array.Copy(grid, this.initialGrid, grid.Length);
    }

    /// <summary>
    /// Gets the current generation grid as a separate copy.
    /// </summary>
    public bool[,] CurrentGeneration
    {
        get
        {
            return (bool[,])this.currentGrid.Clone();
        }
    }

    /// <summary>
    /// Gets the current generation number.
    /// </summary>
    public int Generation { get; private set; }

    /// <summary>
    /// Restarts the game by resetting the current grid to the initial state.
    /// </summary>
    public void Restart()
    {
        Array.Copy(this.initialGrid, this.currentGrid, this.initialGrid.Length);
        this.Generation = 0;
    }

    /// <summary>
    /// Advances the game to the next generation based on the rules of Conway's Game of Life.
    /// </summary>
    public void NextGeneration()
    {
        int rows = this.currentGrid.GetLength(0);
        int columns = this.currentGrid.GetLength(1);
        var newGrid = new bool[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int aliveNeighbors = this.CountAliveNeighbors(i, j);
                bool isAlive = this.currentGrid[i, j];

                if (isAlive && (aliveNeighbors < 2 || aliveNeighbors > 3))
                {
                    newGrid[i, j] = false;
                }
                else if (!isAlive && aliveNeighbors == 3)
                {
                    newGrid[i, j] = true;
                }
                else
                {
                    newGrid[i, j] = isAlive;
                }
            }
        }

        this.currentGrid = newGrid;
        this.Generation++;
    }

    /// <summary>
    /// Counts the number of alive neighbors for a given cell in the grid.
    /// </summary>
    /// <param name="row">The row index of the cell.</param>
    /// <param name="column">The column index of the cell.</param>
    /// <returns>The number of alive neighbors for the specified cell.</returns>
    private int CountAliveNeighbors(int row, int column)
    {
        int rows = this.currentGrid.GetLength(0);
        int columns = this.currentGrid.GetLength(1);
        int count = 0;

        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = column - 1; j <= column + 1; j++)
            {
                if (i >= 0 && i < rows && j >= 0 && j < columns && (i != row || j != column) && this.currentGrid[i, j])
                {
                    count++;
                }
            }
        }

        return count;
    }
}
