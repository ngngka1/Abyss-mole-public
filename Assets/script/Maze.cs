using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze: MonoBehaviour
{
    public static Maze Instance { get; private set; }
    public int[,] maze;
    public const int mazeRow = 23;
    public const int mazeCol = 23;


    private void Awake()
    {
        Instance = this;
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        maze = new int[mazeRow, mazeCol];
        for (int rowNum = 0; rowNum < mazeRow; rowNum++)
        {
            for (int colNum = 0; colNum < mazeCol; colNum++)
            {
                maze[rowNum, colNum] = 1;
            }
        }
        Backtrack(1, 1);

    }
    private void Backtrack(int rowNum, int colNum)
    {
        List<int[]> possiblePath = new List<int[]>();

        if (rowNum - 2 >= 0 && maze[rowNum - 2, colNum] == 1) // move up
        {
            possiblePath.Add(new int[] { rowNum - 2, colNum });
        }
        if (rowNum + 2 < mazeRow && maze[rowNum + 2, colNum] == 1) // move down
        {
            possiblePath.Add(new int[] { rowNum + 2, colNum });
        }
        if (colNum - 2 >= 0 && maze[rowNum, colNum - 2] == 1) // move left
        {
            possiblePath.Add(new int[] { rowNum, colNum - 2 });
        }
        if (colNum + 2 < mazeCol && maze[rowNum, colNum + 2] == 1) // move right
        {
            possiblePath.Add(new int[] { rowNum, colNum + 2 });
        }

        while (possiblePath.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, possiblePath.Count);
            int targetRow = possiblePath[randomIndex][0];
            int targetCol = possiblePath[randomIndex][1];
            possiblePath.RemoveAt(randomIndex);

            if (maze[targetRow, targetCol] == 1)
            {
                maze[rowNum, colNum] = 0;
                maze[(targetRow + rowNum) / 2, (targetCol + colNum) / 2] = 0;
                maze[targetRow, targetCol] = 0;
                Backtrack(targetRow, targetCol);
            }
        }

    }
}

