using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public GameObject[] players_UI;
    private Image[] hearts;
    public Transform[] players_spawn;
    public GameObject player_prefab;

    private int players_count = 0;
    private PlayerScript[] players;

    public Sprite full_heart, empty_heart;

    void Start()
    {
        Debug.Log("Awaking Game Manager");
        hearts = new Image[12];
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                hearts[i * 3 + j] = players_UI[i].transform.GetChild(j).GetComponent<Image>();
            }
        }

        if (PlayerInputManagerSingleton.instance == null) return;
        players_count = PlayerInputManagerSingleton.instance.transform.childCount;
        Transform[] player_inputs = new Transform[players_count];
        players = new PlayerScript[players_count];

        if (players_count == 2)
        {
            players_UI[0].SetActive(true);
            players_UI[1].SetActive(true);
            players_UI[2].SetActive(false);
            players_UI[3].SetActive(false);

            players_UI[1].GetComponent<RectTransform>().anchorMin = players_UI[2].GetComponent<RectTransform>().anchorMin;
            players_UI[1].GetComponent<RectTransform>().anchorMax = players_UI[2].GetComponent<RectTransform>().anchorMax;
            players_UI[1].GetComponent<RectTransform>().anchoredPosition = players_UI[2].GetComponent<RectTransform>().anchoredPosition;
            players_spawn[1].transform.position = players_spawn[2].transform.position;
        }
        else
        {
            for (int i = 0; i < players_count; i++)
            {
                players_UI[i].SetActive(true);
            }
            for (int i = players_count; i < 4; i++)
            {
                players_UI[i].SetActive(false);
            }
        }

        for (int i = 0; i < players_count; i++)
        {
            Debug.Log("Initializing Player " + i);
            player_inputs[i] = PlayerInputManagerSingleton.instance.transform.GetChild(i);
            players[i] = Instantiate(player_prefab).GetComponent<PlayerScript>();
            players[i].SetPlayerIndex(i);
            players[i].Reset(players_spawn[i]);
            player_inputs[i].GetComponent<PlayerInputHandler>().SetPlayerScript(players[i].GetComponent<PlayerScript>());
            player_inputs[i].GetComponent<PlayerInputHandler>().game_state = 1;
        }
    }

    void Update()
    {
        for (int i = 0; i < players_count; i++)
        {
            if(players[i].health < 3)
            {
                hearts[i * 3 + 2].sprite = empty_heart;
            }
            if (players[i].health < 2)
            {
                hearts[i * 3 + 1].sprite = empty_heart;
            }
            if (players[i].health < 1)
            {
                players[i].Reset(players_spawn[i]);
                hearts[i * 3 + 2].sprite = full_heart;
                hearts[i * 3 + 1].sprite = full_heart;
            }
        }
    }
}
