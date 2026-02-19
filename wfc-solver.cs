// WFC Solver for Mobile Terrain Forge
// Implements Wave Function Collapse for procedural terrain generation
// Optimized for mobile: low memory, fast backtracking

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MobileTerrainForge
{
    public class WFCSolver
    {
        // Tile data structure
        public struct Tile
        {
            public int id;
            public int[] rotations; // 0, 90, 180, 270 degrees
            public Dictionary<string, bool>[] connectors; // Per-rotation constraints

            public Tile(int id, int[] rotations, Dictionary<string, bool>[] connectors)
            {
                this.id = id;
                this.rotations = rotations;
                this.connectors = connectors;
            }
        }

        // WFC state
        private Tile[] tiles;
        private Dictionary<int, int>[] tileCompatibility; // Precomputed compatibility
        private int width, height;
        private int[] grid; // Each cell = tile ID (or -1 for collapsed/empty)
        private bool[] collapsed;
        private Dictionary<int, List<int>>[] possibleTiles; // Each cell = list of possible tile IDs

        // Performance tracking
        public int lastBacktrackCount { get; private set; }
        public float lastSolveTime { get; private set; }

        public WFCSolver(Tile[] tiles, int width, int height)
        {
            this.tiles = tiles;
            this.width = width;
            this.height = height;

            // Initialize grid
            grid = new int[width * height];
            collapsed = new bool[width * height];
            possibleTiles = new Dictionary<int, List<int>>[width * height];

            // Precompute compatibility
            PrecomputeCompatibility();
            InitializePossibilities();
        }

        // Precompute tile-to-tile compatibility for all rotations
        private void PrecomputeCompatibility()
        {
            int tileCount = tiles.Length;
            tileCompatibility = new Dictionary<int, int>[tileCount];

            for (int i = 0; i < tileCount; i++)
            {
                tileCompatibility[i] = new Dictionary<int, int>();

                for (int j = 0; j < tileCount; j++)
                {
                    // Check if tile i can be adjacent to tile j in any rotation
                    if (AreCompatible(tiles[i], tiles[j]))
                    {
                        tileCompatibility[i][j] = 1;
                    }
                }
            }
        }

        // Check if two tiles can be adjacent (considering all rotations)
        private bool AreCompatible(Tile a, Tile b)
        {
            // For each rotation of a, check if any rotation of b is compatible
            for (int rotA = 0; rotA < a.rotations.Length; rotA++)
            {
                for (int rotB = 0; rotB < b.rotations.Length; rotB++)
                {
                    // Check connector compatibility
                    // This is simplified - actual implementation would check
                    // north/south/east/west connectors match
                    if (ConnectorsMatch(a.connectors[rotA], b.connectors[rotB]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ConnectorsMatch(Dictionary<string, bool> a, Dictionary<string, bool> b)
        {
            // Simplified connector matching logic
            // Real implementation would check specific edge constraints
            return true;
        }

        private void InitializePossibilities()
        {
            for (int i = 0; i < width * height; i++)
            {
                possibleTiles[i] = new Dictionary<int, List<int>>();

                // Initially, all tiles are possible
                for (int j = 0; j < tiles.Length; j++)
                {
                    possibleTiles[i][j] = new List<int> { j };
                }
            }
        }

        // Main solve method with backtracking
        public bool Solve()
        {
            float startTime = Time.realtimeSinceStartup;
            lastBacktrackCount = 0;

            bool result = SolveRecursive(0);

            lastSolveTime = Time.realtimeSinceStartup - startTime;
            return result;
        }

        private bool SolveRecursive(int cellIndex)
        {
            // Base case: all cells assigned
            if (cellIndex == width * height)
            {
                return true;
            }

            // Find cell with minimum remaining values (MRV heuristic)
            int bestCell = FindMRVCell(cellIndex);

            if (bestCell == -1)
            {
                return false; // No valid cell found
            }

            // Try each possible tile for this cell
            List<int> possible = GetPossibleTiles(bestCell);

            // Sort by least constraining value (LCV heuristic)
            possible = SortByLCV(possible, bestCell);

            foreach (int tileId in possible)
            {
                // Place tile
                grid[bestCell] = tileId;
                collapsed[bestCell] = true;

                // Propagate constraints
                if (PropagateConstraints(bestCell, tileId))
                {
                    // Recurse
                    if (SolveRecursive(cellIndex + 1))
                    {
                        return true;
                    }
                }

                // Backtrack
                grid[bestCell] = -1;
                collapsed[bestCell] = false;
                lastBacktrackCount++;

                // Restore possibilities (simplified - real implementation would track changes)
                RestorePossibilities(bestCell);
            }

            return false;
        }

        // Find cell with minimum remaining values (MRV heuristic)
        private int FindMRVCell(int startIndex)
        {
            int bestCell = -1;
            int minCount = int.MaxValue;

            for (int i = startIndex; i < width * height; i++)
            {
                if (!collapsed[i])
                {
                    int count = GetPossibleTiles(i).Count;
                    if (count < minCount)
                    {
                        minCount = count;
                        bestCell = i;

                        // If we find a cell with only 1 option, take it immediately
                        if (count == 1)
                        {
                            break;
                        }
                    }
                }
            }

            return bestCell;
        }

        private List<int> GetPossibleTiles(int cellIndex)
        {
            // Simplified - get list of still-possible tiles for this cell
            List<int> result = new List<int>();

            for (int i = 0; i < tiles.Length; i++)
            {
                if (!collapsed[cellIndex] || grid[cellIndex] == i)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        // Sort by least constraining value (LCV heuristic)
        private List<int> SortByLCV(List<int> tiles, int cellIndex)
        {
            // Sort tiles by how many options they leave for neighbors
            return tiles.OrderBy(tileId =>
            {
                int constrainedCount = 0;

                // Count how many neighbor possibilities this tile would eliminate
                foreach (int neighbor in GetNeighbors(cellIndex))
                {
                    if (!collapsed[neighbor])
                    {
                        constrainedCount += CountEliminatedNeighbors(cellIndex, neighbor, tileId);
                    }
                }

                return constrainedCount;
            }).ToList();
        }

        private List<int> GetNeighbors(int cellIndex)
        {
            List<int> neighbors = new List<int>();
            int x = cellIndex % width;
            int y = cellIndex / width;

            // 4-way adjacency
            if (x > 0) neighbors.Add(cellIndex - 1); // West
            if (x < width - 1) neighbors.Add(cellIndex + 1); // East
            if (y > 0) neighbors.Add(cellIndex - width); // North
            if (y < height - 1) neighbors.Add(cellIndex + width); // South

            return neighbors;
        }

        private int CountEliminatedNeighbors(int cellIndex, int neighborIndex, int tileId)
        {
            // Simplified - count how many tile options would be eliminated
            // Real implementation would use the compatibility table
            return 0;
        }

        // Propagate constraints after placing a tile
        private bool PropagateConstraints(int cellIndex, int tileId)
        {
            // For each neighbor, remove incompatible tiles
            foreach (int neighbor in GetNeighbors(cellIndex))
            {
                if (!collapsed[neighbor])
                {
                    List<int> toRemove = new List<int>();

                    foreach (int neighborTileId in possibleTiles[neighbor].Keys)
                    {
                        if (!AreTilesCompatible(tileId, neighborTileId, cellIndex, neighbor))
                        {
                            toRemove.Add(neighborTileId);
                        }
                    }

                    // Check if we eliminated all possibilities for neighbor
                    if (toRemove.Count == possibleTiles[neighbor].Count)
                    {
                        return false; // Conflict detected
                    }

                    // Remove incompatible tiles
                    foreach (int removeId in toRemove)
                    {
                        possibleTiles[neighbor].Remove(removeId);
                    }
                }
            }

            return true;
        }

        private bool AreTilesCompatible(int tileA, int tileB, int cellA, int cellB)
        {
            // Use precomputed compatibility table
            return tileCompatibility[tileA].ContainsKey(tileB);
        }

        private void RestorePossibilities(int cellIndex)
        {
            // Simplified - restore all possibilities for neighbors
            // Real implementation would use a stack to track changes
            foreach (int neighbor in GetNeighbors(cellIndex))
            {
                if (!collapsed[neighbor])
                {
                    possibleTiles[neighbor] = new Dictionary<int, List<int>>();
                    for (int i = 0; i < tiles.Length; i++)
                    {
                        possibleTiles[neighbor][i] = new List<int> { i };
                    }
                }
            }
        }

        // Get generated terrain data
        public int[] GetTerrain()
        {
            return grid;
        }

        // Get terrain as Unity mesh data (simplified)
        public Mesh GenerateMesh()
        {
            Mesh mesh = new Mesh();
            // TODO: Implement mesh generation from WFC grid
            return mesh;
        }
    }
}