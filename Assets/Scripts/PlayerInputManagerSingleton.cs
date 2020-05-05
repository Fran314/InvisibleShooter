using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputManagerSingleton : MonoBehaviour
{
    public static PlayerInputManagerSingleton instance {get; private set; }

    public List<PlayerInput> input_list;

    void Awake()
    {
        if(instance != null)
        {

        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
            input_list = new List<PlayerInput>();
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        pi.transform.SetParent(transform);
        input_list.Add(pi);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
