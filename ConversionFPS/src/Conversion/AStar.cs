using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionFPS
{
    static class AStar
    {
        class Node
        {
            public Node Parent;
            public Cube Cube;
            public int Cost;
            public int Heuristic;
            public int TotalCost
            {
                get
                {
                    return (Cost + Heuristic);
                }
            }
            public int Compare(Node otherNode)
            {
                if (TotalCost != otherNode.TotalCost)
                    return (TotalCost - otherNode.TotalCost);
                else
                    return (Heuristic - otherNode.Heuristic);
            }
        }

        public static List<Cube> FindPath(Cube startCube, Cube targetCube)
        {
            List<Node> openList = new List<Node>(); // Tiles to explore (neighbours of already explored tiles)
            List<Node> closedList = new List<Node>(); // Already explored tiles
            Node startNode = new Node() { Cube = startCube, Cost = 0 };
            openList.Add(startNode);
            while (openList.Count > 0) // While there are tiles to explore
            {
                // Get tile of lowest total cost
                openList.Sort((node1, node2) => (node1.Compare(node2)));
                Node currentNode = openList[0];

                // If we are at target position, we found the path !
                if (currentNode.Cube.Equals(targetCube))
                    return (RetracePath(startNode, currentNode));

                // Remove from open and add to closed list : we're going to explore it
                openList.RemoveAt(0);
                closedList.Add(currentNode);

                List<Cube> neighbours = Main.Maze.GetAdjacentCubes(currentNode.Cube);
                for (int i = 0; i < neighbours.Count; ++i) // For each inoccupied neighbour tile of current node
                {
                    Node inClosedSet = closedList.Find(node => node.Cube.Equals(neighbours[i]));
                    if (inClosedSet != null)
                        continue; // Skip if neighbour has already been explored

                    Node neighbour = new Node() // Set neighbour
                    {
                        Cube = neighbours[i],
                        Cost = currentNode.Cost + 1,
                        Heuristic = Main.Maze.GetDistance(neighbours[i], targetCube),
                        Parent = currentNode,
                    };

                    // If neighbour is not already set to be explored later, add it to open list
                    Node inOpenSet = openList.Find(node => node.Cube.Equals(neighbours[i]));
                    if (inOpenSet == null)
                        openList.Add(neighbour);

                    // Or if is has been set but with a higher cost, update it
                    else if (inOpenSet.TotalCost > neighbour.TotalCost)
                    {
                        inOpenSet.Cost = neighbour.Cost;
                        inOpenSet.Heuristic = neighbour.Heuristic;
                        inOpenSet.Parent = currentNode;
                    }
                }
            }
            return (null);
        }

        static List<Cube> RetracePath(Node startNode, Node endNode)
        {
            List<Cube> path = new List<Cube>();
            Node currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode.Cube);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return (path);
        }
    }
}
