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
        if (player != null)
            player.SetInputVector(context.ReadValue<Vector2>());
    }

    public void OnShoot(CallbackContext context)
    {
        if (player != null)
            player.Shoot(context.ReadValue<Vector2>());
    }
}
