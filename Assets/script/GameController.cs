using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum Gamestate {FreeRoam, Dialog, Transition}


public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public Room currentRoom;

    Gamestate state;

    public int dayCount;

    private Light2D lightGlobal;

    private void Awake()
    {
        dayCount = 1;
        lightGlobal = GetComponent<Light2D>();
        lightGlobal.intensity = currentRoom.lightIntensity;
        Instance = this;
    }
    private void Start()
    {

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = Gamestate.Dialog;
        };
        DialogManager.Instance.OnHideDialog += () =>
        {
            if (state == Gamestate.Dialog)
                state = Gamestate.FreeRoam;
        };

    }

    private void Update()
    {
        if (!currentRoom.roomObject.activeSelf)
        {
            currentRoom.roomObject.SetActive(true);
        }
        if (state == Gamestate.Transition)
       {
            return;
       }
       else if (state == Gamestate.FreeRoam)
       {
            PlayerController.Instance.HandleUpdate();
       } 
       else if (state == Gamestate.Dialog)
       {
            DialogManager.Instance.HandleUpdate();
       }
    }

    public static void Room_Swap(Room destinationRoom, Vector3 destinationDoorLandingPos)
    {
        PlayerController.Instance.disableMovement = true;
        destinationRoom.roomObject.SetActive(true);
        new WaitForEndOfFrame();
        Instance.StartCoroutine(PlayerController.Instance.Teleport(destinationDoorLandingPos));
        Instance.currentRoom.roomObject.SetActive(false); // deactivate the room that the player left
        Instance.currentRoom = destinationRoom;
        Instance.TransitionLight();
        PlayerController.Instance.disableMovement = false;

    }

    private void TransitionLight() // will add a dimming effect later
    {
        state = Gamestate.Transition;
        lightGlobal.intensity = currentRoom.lightIntensity;
        state = Gamestate.FreeRoam;
    }

}
