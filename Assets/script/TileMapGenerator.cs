using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapGenerator : MonoBehaviour
{
    int day = 0;
    public int defaultX;
    public int defaultY;
    public bool isWall;
    [SerializeField] LayerMask solidObjectLayer;


    public Tilemap tilemap;
    public TileBase[] tileFloor = new TileBase[4];
    public TileBase[] tileWall = new TileBase[4];

    // Start is called before the first frame update

    private void Awake()
    {

    }
    void Start()
    {
        if (GameController.Instance.dayCount > day)
        {
            day = GameController.Instance.dayCount;
            if (LayerMask.LayerToName(gameObject.layer) == LayerMask.LayerToName((int)Math.Log(solidObjectLayer.value, 2)))
            {
                isWall = true;
            }
            else
            {
                isWall = false;
            }
            Debug.Log(Maze.Instance.maze.Length);
            GenerateTile(Maze.Instance.maze, isWall, Maze.mazeRow, Maze.mazeCol);
        }

    }

    private void GenerateTile(int[,] maze, bool isWall, int mazeRow, int mazeCol)
    {
        for (int rowNum = 0; rowNum < mazeRow; rowNum += 2)
        {
            for (int colNum = 0; colNum < mazeCol; colNum += 2)
            {
                Vector3Int tilePosition = new Vector3Int(defaultX - colNum, defaultY - rowNum, 0);
                if (maze[rowNum / 2, colNum / 2] == 1 && isWall)
                {
                    tilemap.SetTile(tilePosition, tileWall[0]);
                    tilemap.SetTile(tilePosition - new Vector3Int(1, 0, 0), tileWall[1]);
                    tilemap.SetTile(tilePosition - new Vector3Int(0, 1, 0), tileWall[2]);
                    tilemap.SetTile(tilePosition - new Vector3Int(1, 1, 0), tileWall[3]);
                }
                else if (maze[rowNum / 2, colNum / 2] == 0 && !isWall)
                {
                    tilemap.SetTile(tilePosition, tileFloor[0]);
                    tilemap.SetTile(tilePosition - new Vector3Int(1, 0, 0), tileFloor[1]);
                    tilemap.SetTile(tilePosition - new Vector3Int(0, 1, 0), tileFloor[2]);
                    tilemap.SetTile(tilePosition - new Vector3Int(1, 1, 0), tileFloor[3]);
                }
            }
        }
        return;
    }

}
