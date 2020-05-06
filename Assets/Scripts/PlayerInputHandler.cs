using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerScript player;

    public int game_state = 0;
    private void Awake()
    {
        /*
        playerInput = GetComponent<PlayerInput>();
        var movers = FindObjectsOfType<PlayerScript>();
        var index = playerInput.playerIndex;
        player = movers.FirstOrDefault(m => m.GetPlayerIndex() == index);
        */
    }

    public void SetPlayerScript(PlayerScript arg)
    {
        player = arg;
    }

    public void OnMove(CallbackContext context)
    {
        if(game_state == 0)
        {
            GameObject menu_manager = GameObject.Find("MenuManager");
            if (menu_manager != null)
            {
                menu_manager.GetComponent<MenuManagerScript>().Move(GetComponent<PlayerInput>().playerIndex, context.ReadValue<Vector2>().normalized);
            }
        }
        else
        {
            if (player != null)
                player.SetInputVector(context.ReadValue<Vector2>());
        }
    }

    public void OnShoot(CallbackContext context)
    {
        if (game_state == 0)
        {
            GameObject menu_manager = GameObject.Find("MenuManager");
            if (menu_manager != null)
            {
                menu_manager.GetComponent<MenuManagerScript>().Move(GetComponent<PlayerInput>().playerIndex, context.ReadValue<Vector2>().normalized);
            }
        }
        else
        {
            if (player != null)
            player.Shoot(context.ReadValue<Vector2>());
        }
    }

    public void OnConfirm(CallbackContext context)
    {
        if (game_state == 0)
        {
            GameObject menu_manager = GameObject.Find("MenuManager");
            if (menu_manager != null)
            {
                menu_manager.GetComponent<MenuManagerScript>().Select(GetComponent<PlayerInput>().playerIndex);
            }
        }
    }

    public void OnGoBack(CallbackContext context)
    {
        if (game_state == 0)
        {
            GameObject menu_manager = GameObject.Find("MenuManager");
            if (menu_manager != null)
            {
                menu_manager.GetComponent<MenuManagerScript>().GoBack(GetComponent<PlayerInput>().playerIndex);
            }
        }
    }
}
