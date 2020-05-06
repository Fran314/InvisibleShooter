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
    private GameObject[] cubes;
    [SerializeField]
    private Material selected_material;
    [SerializeField]
    private Material unselected_material;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private string[] levels;

    private int players_count = 0;
    private List<bool> readys;
    private List<float> holds;
    private List<GameObject> icons;

    private bool loading_level = false;

    private int menu_state = 0;
    private int selected_button = -1;

    void Start()
    {
        readys = new List<bool>();
        holds = new List<float>();
        icons = new List<GameObject>();
    }

    public void SetReady(int i)
    {
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
                icons.Add(Instantiate(player_icon_prefab, canvas.transform.GetChild(1)));
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

        if (menu_state == 0)
        {
            if (players_count > 0 && selected_button == -1) selected_button = 0;
            if(selected_button != -1)
            {
                cubes[selected_button].GetComponent<MeshRenderer>().material = selected_material;
                for(int i = 0; i < 4 && i != selected_button; i++)
                {
                    cubes[i].GetComponent<MeshRenderer>().material = unselected_material;
                }
            }
        }
        else if(menu_state == 1)
        {
            if (!loading_level)
            {
            }
            else
            {
                Debug.Log("AAAAAAAAA");
                canvas.transform.GetChild(1).Find("MainComunication").GetComponent<Text>().text = "Game Starting...";
                audio.volume -= delta_volume * Time.deltaTime;
                wait_before_loading -= Time.deltaTime;

                if (wait_before_loading <= 0f)
                {
                    LoadLevel();
                }
            }
        }
        else if(menu_state == 2)
        {

        }
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(levels[UnityEngine.Random.Range(0, levels.Length)]);
    }

    public void Move(int index)
    {
        if (index >= players_count) return;
        if (holds[index] <= hold_time) return;

        holds[index] = 0f;

        if (menu_state == 0)
        {
            selected_button += 1;
            if (selected_button == 4) selected_button = 0;
        }
        else if (menu_state == 1)
        {

        }
        else if (menu_state == 2)
        {

        }
    }

    public void Select(int index)
    {
        if (index >= players_count) return;
        if (holds[index] <= hold_time) return;

        holds[index] = 0f;

        if (menu_state == 0)
        {
            if(selected_button == 0)
            {
                menu_state = 1;
                canvas.transform.GetChild(0).gameObject.SetActive(false);
                canvas.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else if (menu_state == 1)
        {
            SetReady(index);
        }
        else if (menu_state == 2)
        {

        }
    }
}
