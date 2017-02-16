using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north;        //1
        public GameObject east;         //2
        public GameObject west;         //3
        public GameObject south;        //4
    }

    public GameObject wallPrefab;

    public float wallLength = 1.0f;
    [Range(1, 100)]
    public int width = 15;
    [Range(1, 100)]
    public int height = 15;
    public static int xSize;        //should be equal to width
    public static int ySize;        //should be equal to height

    Vector3 initialPos;

    public static GameObject wallContainer;

    Cell[] cells;

    List<int> lastCells;

    int currentCell = 0;
    int totalCells;
    int visitedCells = 0;    
    int currentNeighbor = 0;    
    int backingUp = 0;
    int wallToBreak = 0;

    bool startedBuilding = false;

    // Use this for initialization
    void Start () {
        xSize = width;
        ySize = height;
        CreateWalls();
    }

    public void GenerateMaze()
    {
        Reset();

        CreateWalls();
    }

    void Reset()
    {
        Destroy(wallContainer);

        currentCell = 0;
        totalCells = 0;
        visitedCells = 0;
        currentNeighbor = 0;
        backingUp = 0;
        wallToBreak = 0;

        startedBuilding = false;
    }

    void CreateWalls()
    {
        wallContainer = new GameObject();
        wallContainer.name = "MazeWalls";
        totalCells = xSize * ySize;        
        lastCells = new List<int>();
        initialPos = new Vector3((-xSize/2) + wallLength/2, 0f, (-ySize/2) + wallLength/2);
        Vector3 myPos = initialPos;
        GameObject clone;

        //x axis
        for(int i = 0; i < ySize; i++)
        {
            for(int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2, 0f, initialPos.z + (i * wallLength) - wallLength / 2);
                clone = Instantiate(wallPrefab, myPos, Quaternion.identity) as GameObject;
                clone.transform.parent = wallContainer.transform;
                clone.name = "Wall";
            }
        }

        //y axis
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength), 0f, initialPos.z + (i * wallLength) - wallLength);
                clone = Instantiate(wallPrefab, myPos, Quaternion.Euler(0, 90, 0)) as GameObject;
                clone.transform.parent = wallContainer.transform;
                clone.name = "Wall";
            }
        }

        CreateCells();
    }

    void CreateCells()
    {
        int children = wallContainer.transform.childCount;
        GameObject[] allWalls = new GameObject[children];
        cells = new Cell[totalCells];
        int eastWestProcess = 0;
        int childProcess = 0;
        int termCount = 0;

        //Gets all of the children
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallContainer.transform.GetChild(i).gameObject;
        }

        //Assigns walls to the cells
        for(int cellProcess = 0; cellProcess < cells.Length; cellProcess++)
        {
            if (termCount == xSize)
            {
                eastWestProcess++;
                termCount = 0;
            }

            cells[cellProcess] = new Cell();
            cells[cellProcess].west = allWalls[eastWestProcess];
            cells[cellProcess].south = allWalls[childProcess + (xSize + 1) * ySize];

            eastWestProcess++;

            termCount++;
            childProcess++;

            cells[cellProcess].east = allWalls[eastWestProcess];
            cells[cellProcess].north = allWalls[(childProcess + (xSize + 1) * ySize) + xSize - 1];
        }

        CreateMaze();
    }

    void CreateMaze()
    {
        while (visitedCells < totalCells)
        {
            if(startedBuilding)
            {
                GetNeighbor();
                if(!cells[currentNeighbor].visited && cells[currentCell].visited)
                {
                    BreakWall();
                    cells[currentNeighbor].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbor;
                    if(lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else 
            {
                currentCell = Random.Range(0, totalCells);
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }
        }

        Debug.Log("Finished");
    }

    void GetNeighbor()
    {
        int length = 0;
        int[] neighbors = new int[4];
        int[] connectingWall = new int[4];
        int check = 0;
        check = (currentCell + 1) / xSize;
        check -= 1;
        check *= xSize;
        check += xSize;

        //east
        if(currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if(!cells[currentCell + 1].visited)
            {
                neighbors[length] = currentCell + 1;
                connectingWall[length] = 2;
                length++;
            }
        }

        //west
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (!cells[currentCell - 1].visited)
            {
                neighbors[length] = currentCell - 1;
                connectingWall[length] = 3;
                length++;
            }
        }

        //north
        if (currentCell + xSize < totalCells)
        {
            if (!cells[currentCell + xSize].visited)
            {
                neighbors[length] = currentCell + xSize;
                connectingWall[length] = 1;
                length++;
            }
        }

        //south
        if (currentCell - xSize >= 0)
        {
            if (!cells[currentCell - xSize].visited)
            {
                neighbors[length] = currentCell - xSize;
                connectingWall[length] = 4;
                length++;
            }
        }

        if(length != 0)
        {
            int chosenCell = Random.Range(0, length);
            currentNeighbor = neighbors[chosenCell];
            wallToBreak = connectingWall[chosenCell];
        }
        else
        {
            if(backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }
    }

    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1:
                Destroy(cells[currentCell].north);
                break;
            case 2:
                Destroy(cells[currentCell].east);
                break;
            case 3:
                Destroy(cells[currentCell].west);
                break;
            case 4:
                Destroy(cells[currentCell].south);
                break;
        }
    }   
}
