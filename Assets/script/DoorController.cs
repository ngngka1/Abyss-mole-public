using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class DoorController : MonoBehaviour, Interactable
{
    [SerializeField] Door door;

    public LayerMask solidObjectsLayer;
    public LayerMask interactablesLayer;
    public float SpriteWidth;
    public float SpriteHeight;


    private void Start()
    {
        gameObject.GetComponent<Tilemap>().CompressBounds();
        Tilemap map = gameObject.GetComponent<Tilemap>();
        door.width = map.size.x;
        door.height = map.size.y;
        SpriteWidth = PlayerController.Instance.spriteWidth;
        SpriteHeight = PlayerController.Instance.spriteHeight;
        door.doorPos = transform.position;
        door.facingDirection = GetFacingDirection(door.doorPos);
        door.landingPos = GetLandingPos(door.doorPos, door.facingDirection);
        if (door.room.roomObject.name != GameController.Instance.currentRoom.roomObject.name)
        {
            door.room.roomObject.SetActive(false);
        }
    }

    private Door.Facing GetFacingDirection(Vector3 Pos)
    {
        if (Physics2D.OverlapCircle(Pos + new Vector3((door.width + SpriteWidth) / 2, 0f), 0.2f, solidObjectsLayer | interactablesLayer) == null) // door width / 2 because the transform position pivot of the door is at its centre
        {
            return Door.Facing.FACING_RIGHT;
        }
        else if (Physics2D.OverlapCircle(Pos - new Vector3((door.width + SpriteWidth) / 2, 0f), 0.2f, solidObjectsLayer | interactablesLayer) == null)
        {
            return Door.Facing.FACING_LEFT;
        }
        else if (Physics2D.OverlapCircle(Pos + new Vector3(0f, (door.height + SpriteHeight / 2)), 0.2f, solidObjectsLayer | interactablesLayer) == null)
        {
            return Door.Facing.FACING_UP;
        }
        else if (Physics2D.OverlapCircle(Pos - new Vector3(0f, SpriteHeight / 2), 0.2f, solidObjectsLayer | interactablesLayer) == null)
        {
            return Door.Facing.FACING_DOWN;
        }
        return Door.Facing.FACING_UP;
    }

    private Vector3 GetLandingPos(Vector3 doorPos, Door.Facing facingDirection)
    {
        return facingDirection switch
        {
            Door.Facing.FACING_RIGHT => doorPos + new Vector3((door.width + SpriteWidth) / 2, 0f),
            Door.Facing.FACING_LEFT => doorPos - new Vector3((door.width + SpriteWidth) / 2, 0f),
            Door.Facing.FACING_UP => doorPos + new Vector3(0f, (door.height + SpriteHeight / 2)),
            Door.Facing.FACING_DOWN => doorPos - new Vector3(0f, SpriteHeight / 2),
            _ => doorPos,
        };
    }
    public void Interact()
    {
        Door destinationDoor = door.destinationDoor.GetComponent<DoorController>().door;
        GameController.Room_Swap(destinationDoor.room, destinationDoor.landingPos);
    }
}
