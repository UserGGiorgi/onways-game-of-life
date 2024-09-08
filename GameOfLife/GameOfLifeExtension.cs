namespace GameOfLife;

/// <summary>
/// Provides extension methods for simulating Conway's Game of Life on different versions.
/// </summary>
public static class GameOfLifeExtension
{
    /// <summary>
    /// Simulates the evolution of Conway's Game of Life for a specified number of generations using the sequential version.
    /// The result is written to the provided <see cref="TextWriter"/> using the specified characters for alive and dead cells.
    /// </summary>
    /// <param name="game">The sequential version of the Game of Life.</param>
    /// <param name="generations">The number of generations to simulate.</param>
    /// <param name="writer">The <see cref="TextWriter"/> used to output the simulation result.</param>
    /// <param name="aliveCell">The character representing an alive cell.</param>
    /// <param name="deadCell">The character representing a dead cell.</param>
    /// <exception cref="ArgumentNullException">Thrown when <param name="game"/> parameters is null.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <param name="writer"/> parameters is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <param name="generations"/> is less than or equal to 0.</exception>
    public static void Simulate(this GameOfLifeSequentialVersion? game, int generations, TextWriter? writer, char aliveCell, char deadCell)
    {
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(generations);

        for (int gen = 0; gen < generations; gen++)
        {
            var currentGrid = game.GetCurrentGeneration();
            int rows = currentGrid.GetLength(0);
            int columns = currentGrid.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    writer.Write(currentGrid[i, j] ? aliveCell : deadCell);
                }

                writer.WriteLine();
            }

            writer.WriteLine($"Generation {game.Generation}");
            writer.WriteLine(new string('-', columns));

            game.NextGeneration();
        }
    }

    /// <summary>
    /// Asynchronously simulates the evolution of Conway's Game of Life for a specified number of generations using the parallel version.
    /// The result is written to the provided TextWriter using the specified characters for alive and dead cells.
    /// </summary>
    /// <param name="game">The parallel version of the Game of Life.</param>
    /// <param name="generations">The number of generations to simulate.</param>
    /// <param name="writer">The <see cref="TextWriter"/> used to output the simulation result.</param>
    /// <param name="aliveCell">The character representing an alive cell.</param>
    /// <param name="deadCell">The character representing a dead cell.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <param name="game"/> parameters is null.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <param name="writer"/> parameters is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <param name="generations"/> is less than or equal to 0.</exception>
    public static async Task SimulateAsync(this GameOfLifeParallelVersion? game, int generations, TextWriter? writer, char aliveCell, char deadCell)
    {
        ValidateSimulateAsyncParams(game, generations, writer);

        // Ensure game and writer are not null before passing them to SimulateGameAsync
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(writer);

        await SimulateGameAsync(game, generations, writer, aliveCell, deadCell);
    }

    // Parameter validation method
    private static void ValidateSimulateAsyncParams(GameOfLifeParallelVersion? game, int generations, TextWriter? writer)
    {
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(generations);
    }

    // Simulation logic in its own method
    private static async Task SimulateGameAsync(GameOfLifeParallelVersion game, int generations, TextWriter writer, char aliveCell, char deadCell)
    {
        for (int gen = 0; gen < generations; gen++)
        {
            var currentGrid = game.GetCurrentGeneration();
            int rows = currentGrid.GetLength(0);
            int columns = currentGrid.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    await writer.WriteAsync(currentGrid[i, j] ? aliveCell : deadCell);
                }

                await writer.WriteLineAsync();
            }

            await writer.WriteLineAsync($"Generation {game.Generation}");
            await writer.WriteLineAsync(new string('-', columns));
        }
    }
}
