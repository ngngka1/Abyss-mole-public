using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Door
{
    [SerializeField] public GameObject destinationDoor;
    public Room room;
    public Vector3 doorPos;
    public Vector3 landingPos;
    public float width;
    public float height;
    public enum Facing { FACING_LEFT, FACING_RIGHT, FACING_UP, FACING_DOWN }
    public Facing facingDirection = Facing.FACING_UP;



}
