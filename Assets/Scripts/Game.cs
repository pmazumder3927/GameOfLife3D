using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Cell[,,] currTime = new Cell[32, 32, 32];
    private CellProperties[,,] nextBoard = new CellProperties[32, 32, 32];
    public GameObject cellPrefab;
    private int[,,] initialState = new int[32, 32, 32];
    private int[,] cellView = new int[26, 3] { { 1, 1, 1 }, { 0, 1, 1 }, { -1, 1, 1 }, { -1, 0, 1 }, { -1, -1, 1 }, { 0, -1, 1 }, { 1, -1, 1 }, { 1, 0, 1 }, {0, 0, 1},
    { 1, 1, 0 }, { 0, 1, 0 }, { -1, 1, 0 }, { -1, 0, 0 }, { -1, -1, 0 }, { 0, -1, 0 }, { 1, -1, 0 }, { 1, 0, 0 },
    { 1, 1, -1 }, { 0, 1, -1 }, { -1, 1, -1 }, { -1, 0, -1 }, { -1, -1, -1 }, { 0, -1, -1 }, { 1, -1, -1 }, { 1, 0, -1 }, {0, 0, -1} };
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
                    nextBoard[i, j, k] = currTime[i, j, k].GetProps();
                    int neighbors = checkNeighbors(i, j, k);
                    CellProperties currCell = currTime[i,j,k].GetProps();
                    CellProperties nextCell = nextBoard[i, j, k];
                    if (currCell.alive)
                    {
                        // any live cell with fewer than 6 neighbors dies
                        if (neighbors < 6)
                        {
                            nextCell.alive = false;
                        }
                        // any live cell with more than 9 live neighbors dies
                        if (neighbors > 9)
                        {
                            nextCell.alive = false;
                        }
                    }
                    else
                    {
                        //any dead cell with 9 live neighbors comes back to life
                        if (neighbors >= 9)
                        {
                            nextCell.alive = true;
                        }
                    }
                    nextCell.neighbors = neighbors;
                    nextCell.age += 1;
                }
            }
        }
        return nextBoard;
    }
    
    IEnumerator RenderNext()
    {
        // This will wait 1 second like Invoke could do, remove this if you don't need it
        yield return new WaitForSeconds(1);


        float timePassed = 0;
        while (timePassed < 100)
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
            timePassed += Time.deltaTime;

            yield return new WaitForSeconds(0.05f);
        }
    }

    public void NextStep()
    {
        StartCoroutine(RenderNext());
    }

    public int checkNeighbors(int x, int y, int z)
    {
        int neighbors = 0;
        for(int i=0; i < cellView.GetLength(0); i++)
        {
            int xm = x + cellView[i, 0];
            int ym = y + cellView[i, 1];
            int zm = z + cellView[i, 2];
            if (xm > 0 && ym > 0 && zm > 0 && xm < 32 && ym < 32 && zm < 32)
            {
                if (currTime[xm, ym, zm].GetProps().alive) neighbors++;
            }
        }
        return neighbors;
    }

    public void CreateAtCoordinate(Vector3 pos)
    {
        print(string.Format("Position hit: {0} {1} {2}", pos.x, pos.y, pos.z));
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);
        int z = Mathf.RoundToInt(pos.z);
        print(x);
        print(y);
        print(z);
        print(currTime[x, y, z].GetProps().alive);
        currTime[x, y, z].SetProps(new CellProperties(true));
    }
}
