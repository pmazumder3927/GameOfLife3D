using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Cell[,,] currTime = new Cell[32, 32, 32];
    private CellProperties[,,] nextBoard = new CellProperties[32, 32, 32];
    public GameObject cellPrefab;
    private int[,,] initialState = new int[32, 32, 32];
    private int[,] cellView = new int[24, 3] { { 1, 1, 1 }, { 0, 1, 1 }, { -1, 1, 1 }, { -1, 0, 1 }, { -1, -1, 1 }, { 0, -1, 1 }, { 1, -1, 1 }, { 1, 0, 1 },
    { 1, 1, 0 }, { 0, 1, 0 }, { -1, 1, 0 }, { -1, 0, 0 }, { -1, -1, 0 }, { 0, -1, 0 }, { 1, -1, 0 }, { 1, 0, 0 },
    { 1, 1, -1 }, { 0, 1, -1 }, { -1, 1, -1 }, { -1, 0, -1 }, { -1, -1, -1 }, { 0, -1, -1 }, { 1, -1, -1 }, { 1, 0, -1 }};
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < initialState.GetLength(0); ++i)
        {
            for (int j = 0; j < initialState.GetLength(1); ++j)
            {
                for (int k = 0; k < initialState.GetLength(2); ++k)
                {
                    initialState[i, j, k] = Random.Range(0,2);
                }
            }
        }

        SetupBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetupBoard()
    {
        Vector3 start = this.transform.position;
        for (int i = 0; i < initialState.GetLength(0); ++i)
        {
            for (int j = 0; j < initialState.GetLength(1); ++j)
            {
                for (int k = 0; k < initialState.GetLength(2); ++k)
                {
                    currTime[i, j, k] = GameObject.Instantiate(cellPrefab, new Vector3(start.x + i, start.y + j, start.z + k), Quaternion.identity).GetComponent<Cell>();
                    if (initialState[i,j,k] == 1)
                    {
                        currTime[i, j, k].SetProps(new CellProperties(true));
                    }
                    nextBoard[i, j, k] = currTime[i, j, k].GetProps();
                }
            }
        }
    }

    private CellProperties[,,] GetNextBoard()
    {
        for (int i = 0; i < currTime.GetLength(0); ++i)
        {
            for (int j = 0; j < currTime.GetLength(1); ++j)
            {
                for (int k = 0; k < currTime.GetLength(2); ++k)
                {
                    int neighbors = checkNeighbors(i, j, k, cellView);
                    CellProperties currCell = nextBoard[i, j, k];
                    // any live cell with fewer than 6 neighbors dies
                    if (currCell.alive && neighbors < 6)
                    {
                        currCell.alive = false;
                    }
                    // any live cell with more than 9 live neighbors dies
                    if (currCell.alive && neighbors > 9)
                    {
                        currCell.alive = false;
                    }
                    //any dead cell with 9 live neighbors comes back to life
                    if (!currCell.alive && neighbors == 9)
                    {
                        currCell.alive = true;
                    }
                    nextBoard[i, j, k].age += 1;
                }
            }
        }
        return nextBoard;
    }

    public void NextStep()
    {
        CellProperties[,,] nextBoard = GetNextBoard();
        // update curr board
        for (int i = 0; i < currTime.GetLength(0); ++i)
        {
            for (int j = 0; j < currTime.GetLength(1); ++j)
            {
                for (int k = 0; k < currTime.GetLength(2); ++k)
                {
                    currTime[i, j, k].SetProps(nextBoard[i, j, k]);
                }
            }
        }
    }

    private int checkNeighbors(int x, int y, int z, int[,] view)
    {
        int neighbors = 0;
        for(int i=0; i < view.GetLength(0); i++)
        {
            int xm = x + view[i, 0];
            int ym = y + view[i, 1];
            int zm = z + view[i, 2];
            if (xm > 0 && ym > 0 && zm > 0 && xm < 32 && ym < 32 && zm < 32)
            {
                if (currTime[xm, ym, zm].GetProps().alive) neighbors++;
            }
        }
        return neighbors;
    }
}
