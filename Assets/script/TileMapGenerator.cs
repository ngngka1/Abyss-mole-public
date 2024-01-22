using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapGenerator : MonoBehaviour
{
    private const int mazeRow = 30;
    private const int mazeCol = 30;
    int[,] maze;
    int day = 0;
    public int defaultX;
    public int defaultY;

    public Tilemap tilemap;
    public TileBase[] tileFloor = new TileBase[4];
    public TileBase[] tileWall = new TileBase[4];

    // Start is called before the first frame update
    void Start()
    {
        if (GameController.Instance.dayCount > day)
        {
            day = GameController.Instance.dayCount;
            // GenerateMaze(ref maze, mazeRow, mazeCol);
        }
    }

    private void GenerateTile(int[,] maze, int mazeRow, int mazeCol)
    {
        for (int rowNum = 0; rowNum < mazeRow; rowNum += 2)
        {
            for (int colNum = 0; colNum < mazeCol; colNum += 2)
            {
                Vector3Int tilePosition = new Vector3Int(defaultX + colNum, defaultY + rowNum, 0);
                if (maze[rowNum, colNum] == 1)
                {
                    tilemap.SetTile(tilePosition, tileWall[0]);
                    tilemap.SetTile(tilePosition + new Vector3Int(1, 0, 0), tileWall[1]);
                    tilemap.SetTile(tilePosition + new Vector3Int(0, 1, 0), tileWall[2]);
                    tilemap.SetTile(tilePosition + new Vector3Int(1, 1, 0), tileWall[3]);
                }
                else
                {
                    tilemap.SetTile(tilePosition, tileFloor[0]);
                    tilemap.SetTile(tilePosition + new Vector3Int(1, 0, 0), tileFloor[1]);
                    tilemap.SetTile(tilePosition + new Vector3Int(0, 1, 0), tileFloor[2]);
                    tilemap.SetTile(tilePosition + new Vector3Int(1, 1, 0), tileFloor[3]);
                }
            }
        }
        return;
    }

    private void GenerateMaze(ref int[,] maze, int mazeRow, int mazeCol)
    {
        maze = new int[mazeRow, mazeCol];
        for (int rowNum = 0; rowNum < mazeRow; rowNum++)
        {
            for (int colNum = 0; colNum < mazeCol; colNum++)
            {
                maze[rowNum, colNum] = 1;
            }
        }
        Backtrack(0, 0, ref maze, mazeRow, mazeCol);
    }


    private void Backtrack(int rowNum, int colNum, ref int[,] maze, int mazeRow, int mazeCol)
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
                Backtrack(targetRow, targetCol, ref maze, mazeRow, mazeCol);
            }
        }

    }
}
