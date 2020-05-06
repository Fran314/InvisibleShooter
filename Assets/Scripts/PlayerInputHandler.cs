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
        else if (game_state == 1)
        {
            if (player != null)
                player.SetMoveInput(context.ReadValue<Vector2>());
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
        else if (game_state == 1)
        {
            if (player != null)
            player.SetShootingInput(context.ReadValue<Vector2>());
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
        else if (game_state == 2)
        {
            Debug.Log("Confirm");
            GameObject game_manager = GameObject.Find("GameManager");
            if (game_manager != null)
            {
                game_manager.GetComponent<GameManagerScript>().Select(GetComponent<PlayerInput>().playerIndex);
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
        else if (game_state == 2)
        {
            GameObject game_manager = GameObject.Find("GameManager");
            if (game_manager != null)
            {
                game_manager.GetComponent<GameManagerScript>().GoBack(GetComponent<PlayerInput>().playerIndex);
            }
        }
    }
}
