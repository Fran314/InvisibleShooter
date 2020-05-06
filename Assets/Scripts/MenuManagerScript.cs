using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject player_icon_prefab;
    [SerializeField]
    private Sprite keyboard, gamepad;
    [SerializeField]
    private float hold_time = 0.3f;
    [SerializeField]
    private float wait_before_loading = 1f;
    [SerializeField]
    private float delta_volume = 1f;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private string[] levels;

    private int players_count = 0;
    private List<bool> readys;
    private List<float> holds;
    private List<GameObject> icons;

    private bool loading_level = false;

    void Start()
    {
        readys = new List<bool>();
        holds = new List<float>();
        icons = new List<GameObject>();
    }

    public void SetReady(int i)
    {
        if (i >= players_count) return;
        if (holds[i] <= hold_time) return;

        readys[i] = true;
        icons[i].transform.GetChild(2).gameObject.SetActive(false);
        icons[i].transform.GetChild(1).GetComponent<Text>().text = "Ready!";

        if(players_count >= 2 && readys.All(b => b == true))
        {
            loading_level = true;
        }
    }

    void Update()
    {
        if(!loading_level)
        {
            for (int i = 0; i < players_count; i++)
            {
                holds[i] += Time.deltaTime;
            }
            if (PlayerInputManagerSingleton.instance == null) return;
            int new_players_count = PlayerInputManagerSingleton.instance.transform.childCount;
            if (new_players_count != players_count)
            {
                for (int i = players_count; i < new_players_count; i++)
                {
                    icons.Add(Instantiate(player_icon_prefab, canvas.transform));
                    if (PlayerInputManagerSingleton.instance.transform.GetChild(i).GetComponent<PlayerInput>().devices[0].displayName == "Keyboard")
                    {
                        icons[i].transform.GetChild(0).GetComponent<Image>().sprite = keyboard;
                    }
                    icons[i].transform.GetChild(0).GetComponent<Waver>().offset = i;
                    icons[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(128 * i - 192, -64, 0);
                    icons[i].transform.GetChild(1).GetComponent<Text>().text = "Player " + (i + 1).ToString() + " Joined!";
                    readys.Add(false);
                    holds.Add(0f);
                }

                players_count = new_players_count;
            }
        }
        else
        {
            canvas.transform.Find("MainComunication").GetComponent<Text>().text = "Game Starting...";
            audio.volume -= delta_volume * Time.deltaTime;
            wait_before_loading -= Time.deltaTime;

            if(wait_before_loading <= 0f)
            {
                LoadLevel();
            }
        }
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(levels[UnityEngine.Random.Range(0, levels.Length)]);
    }
}
