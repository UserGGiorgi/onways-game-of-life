namespace GameOfLife;

/// <summary>
/// Represents Conway's Game of Life in a parallel version.
/// The class provides methods to simulate the game's evolution based on simple rules.
/// </summary>
public sealed class GameOfLifeParallelVersion
{
    private bool[,] grid;
    private bool[,] initialGrid;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameOfLifeParallelVersion"/> class with the specified number of rows and columns of the grid. The initial state of the grid is randomly set with alive or dead cells.
    /// </summary>
    /// <param name="rows">The number of rows in the grid.</param>
    /// <param name="columns">The number of columns in the grid.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of rows or columns is less than or equal to 0.</exception>
    public GameOfLifeParallelVersion(int rows, int columns)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rows);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(columns);

        this.grid = new bool[rows, columns];
        this.initialGrid = new bool[rows, columns];

        var random = new Random();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                this.grid[i, j] = random.Next(2) == 0;
                this.initialGrid[i, j] = this.grid[i, j];
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameOfLifeParallelVersion"/> class with the given grid.
    /// </summary>
    /// <param name="grid">The 2D array representing the initial state of the grid.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <param name="grid"/> is null.</exception>
    public GameOfLifeParallelVersion(bool[,] grid)
    {
        ArgumentNullException.ThrowIfNull(grid);
        this.grid = (bool[,])grid.Clone();
        this.initialGrid = (bool[,])grid.Clone();
    }

    /// <summary>
    /// Gets the current generation grid as a separate copy.
    /// </summary>
    public bool[,] CurrentGeneration
    {
        get
        {
            return (bool[,])this.grid.Clone();
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
        this.grid = (bool[,])this.initialGrid.Clone();
        this.Generation = 0;
    }

    /// <summary>
    /// Advances the game to the next generation based on the rules of Conway's Game of Life.
    /// </summary>
    public void NextGeneration()
    {
        int rows = this.grid.GetLength(0);
        int columns = this.grid.GetLength(1);
        var newGrid = new bool[rows, columns];

        _ = Parallel.For(0, rows, i =>
        {
            for (int j = 0; j < columns; j++)
            {
                int aliveNeighbors = this.CountAliveNeighbors(i, j);
                bool isAlive = this.grid[i, j];

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
        });

        this.grid = newGrid;
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
        int rows = this.grid.GetLength(0);
        int columns = this.grid.GetLength(1);
        int count = 0;

        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = column - 1; j <= column + 1; j++)
            {
                if (i == row && j == column)
                {
                    continue;
                }

                int wrappedRow = (i + rows) % rows;
                int wrappedColumn = (j + columns) % columns;

                if (this.grid[wrappedRow, wrappedColumn])
                {
                    count++;
                }
            }
        }

        return count;
    }
}
