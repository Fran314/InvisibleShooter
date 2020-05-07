using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    private float hold_time = 0.5f;
    [SerializeField]
    private float wait_before_loading = 1f;
    [SerializeField]
    private float delta_volume = 1f;

    [SerializeField]
    private GameObject[] main_menu_cubes;
    [SerializeField]
    private GameObject[] select_level_cubes;
    [SerializeField]
    private GameObject quit_cube;
    [SerializeField]
    private Material selected_material;
    [SerializeField]
    private Material unselected_material;

    [SerializeField]
    private GameObject player_input_manager_prefab;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private string[] levels;

    private int players_count = 0;
    private List<bool> readys;
    private List<float> holds;
    private List<GameObject> icons;
    private Color[] players_color = { new Color(0.140995f, 0.865f, 0.865f), new Color(0.78f, 0.18252f, 0.78f), new Color(0.901f, 0.7020628f, 0.06937696f), new Color(0.07378396f, 0.802f, 0.07378396f)};

    private bool loading_level = false;

    private int menu_state = 0;
    private int selected_button = -1;

    void Start()
    {
        readys = new List<bool>();
        holds = new List<float>();
        icons = new List<GameObject>();

        GameObject pim = GameObject.Find("PlayerInputManager");
        if(pim == null)
        {
            Instantiate(player_input_manager_prefab);
        }

        PlayerInputManagerSingleton.instance.GetComponent<LoadLevelType>().levels = levels;
    }

    public void SetReady(int index)
    {
        readys[index] = true;
        icons[index].transform.Find("Info").GetComponent<Text>().text = "";
        icons[index].transform.Find("Comunication").GetComponent<Text>().text = "Ready!";

        if(players_count >= 2 && readys.All(b => b == true))
        {
            menu_state = 5;
            canvas.transform.GetChild(1).gameObject.SetActive(false);
            canvas.transform.GetChild(5).gameObject.SetActive(true);
            select_level_cubes[0].transform.parent.gameObject.SetActive(true);

            selected_button = 0;
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
                    icons[i].transform.Find("Image").GetComponent<Image>().sprite = keyboard;
                    icons[i].transform.Find("Info").GetComponent<Text>().text = "Press ENTER\nto set Ready";
                }
                else
                {
                    icons[i].transform.Find("Info").GetComponent<Text>().text = "Press (A)\nto set Ready";
                }
                icons[i].transform.Find("Image").GetComponent<Waver>().offset = i;
                icons[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(128 * i - 192, -64, 0);
                icons[i].transform.Find("Comunication").GetComponent<Text>().text = "Player " + (i + 1).ToString() + " Joined!";
                icons[i].transform.Find("Comunication").GetComponent<Text>().color = players_color[i];
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
                main_menu_cubes[selected_button].GetComponent<MeshRenderer>().material = selected_material;
                for(int i = 0; i < main_menu_cubes.Length; i++)
                {
                    if (i != selected_button) main_menu_cubes[i].GetComponent<MeshRenderer>().material = unselected_material;
                }
            }
        }
        else if(menu_state == 1)
        {
        }
        else if(menu_state == 5)
        {
            select_level_cubes[selected_button].GetComponent<MeshRenderer>().material = selected_material;
            for (int i = 0; i < select_level_cubes.Length; i++)
            {
                if (i != selected_button) select_level_cubes[i].GetComponent<MeshRenderer>().material = unselected_material;
            }
            if (loading_level)
            {
                canvas.transform.GetChild(5).Find("MainComunication").GetComponent<Text>().text = "Game Starting...";
                audio.volume -= delta_volume * Time.deltaTime;
                wait_before_loading -= Time.deltaTime;

                if (wait_before_loading <= 0f)
                {
                    if(selected_button == 0) SceneManager.LoadScene(levels[UnityEngine.Random.Range(0, levels.Length)]);
                    else SceneManager.LoadScene(levels[selected_button - 1]);
                }
            }
        }
    }

    public void Move(int index, Vector2 direction)
    {
        if(direction.magnitude >= 0.7f)
        {
            if (index >= players_count) return;
            if (holds[index] <= hold_time) return;

            holds[index] = 0f;

            if (menu_state == 0)
            {
                if (Vector2.Dot(Vector2.up, direction) <= 0) selected_button += 1;
                else selected_button -= 1;
                if (selected_button <= -1) selected_button = main_menu_cubes.Length - 1;
                else if (selected_button >= main_menu_cubes.Length) selected_button = 0;
            }
            else if (menu_state == 5)
            {
                if (!loading_level)
                {
                    float up_dot = Vector2.Dot(Vector2.up, direction);
                    if (up_dot >= 0.7f)
                    {
                        if (selected_button < 3) selected_button += 3;
                        else selected_button -= 3;
                    }
                    else if (up_dot <= -0.7f)
                    {
                        if (selected_button < 3) selected_button += 3;
                        else selected_button -= 3;
                    }
                    else
                    {
                        if (Vector2.Dot(Vector2.right, direction) >= 0)
                        {
                            if(selected_button == 2 || selected_button == 5)
                            {
                                selected_button -= 2;
                            }
                            else
                            {
                                selected_button++;
                            }
                        }
                        else
                        {
                            if(selected_button == 0 || selected_button == 3)
                            {
                                selected_button += 2;
                            }
                            else
                            {
                                selected_button--;
                            }
                        }
                    }
                }
            }
        }
    }

    public void Select(int index)
    {
        if (index >= players_count) return;
        if (holds[index] <= hold_time) return;

        holds[index] = 0f;

        if (menu_state == 0)
        {
            main_menu_cubes[0].transform.parent.gameObject.SetActive(false);
            if (selected_button == 0)
            {
                menu_state = 1;
                canvas.transform.GetChild(0).gameObject.SetActive(false);
                canvas.transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (selected_button == 1)
            {
                menu_state = 2;
                canvas.transform.GetChild(0).gameObject.SetActive(false);
                canvas.transform.GetChild(2).gameObject.SetActive(true);
            }
            else if (selected_button == 2)
            {
                menu_state = 3;
                canvas.transform.GetChild(0).gameObject.SetActive(false);
                canvas.transform.GetChild(3).gameObject.SetActive(true);
            }
            else if (selected_button == 3)
            {
                menu_state = 4;
                canvas.transform.GetChild(0).gameObject.SetActive(false);
                canvas.transform.GetChild(4).gameObject.SetActive(true);
            }
        }
        else if (menu_state == 1)
        {
            SetReady(index);
        }
        else if (menu_state == 2)
        {

        }
        else if (menu_state == 3)
        {

        }
        else if (menu_state == 4)
        {

        }
        else if (menu_state == 5)
        {
            loading_level = true;
            PlayerInputManagerSingleton.instance.GetComponent<LoadLevelType>().load_level_type = selected_button;
        }
        else if (menu_state == 6)
        {
            menu_state = 0;
            canvas.transform.GetChild(6).gameObject.SetActive(false);
            canvas.transform.GetChild(0).gameObject.SetActive(true);
            quit_cube.transform.parent.gameObject.SetActive(false);
            main_menu_cubes[0].transform.parent.gameObject.SetActive(true);
        }
    }

    public void GoBack(int index)
    {
        if (index >= players_count) return;
        if (holds[index] <= hold_time) return;

        holds[index] = 0f;

        if (menu_state == 0)
        {
            menu_state = 6;
            canvas.transform.GetChild(6).gameObject.SetActive(true);
            canvas.transform.GetChild(0).gameObject.SetActive(false);
            quit_cube.transform.parent.gameObject.SetActive(true);
            main_menu_cubes[0].transform.parent.gameObject.SetActive(false);
        }
        else if (menu_state == 1)
        {
            Destroy(PlayerInputManagerSingleton.instance.gameObject);
            SceneManager.LoadScene("PlayerSelectScene");
        }
        else if (menu_state == 2)
        {
            menu_state = 0;
            canvas.transform.GetChild(0).gameObject.SetActive(true);
            canvas.transform.GetChild(2).gameObject.SetActive(false);
            main_menu_cubes[0].transform.parent.gameObject.SetActive(true);
        }
        else if (menu_state == 3)
        {
            menu_state = 0;
            canvas.transform.GetChild(0).gameObject.SetActive(true);
            canvas.transform.GetChild(3).gameObject.SetActive(false);
            main_menu_cubes[0].transform.parent.gameObject.SetActive(true);
        }
        else if (menu_state == 4)
        {
            menu_state = 0;
            canvas.transform.GetChild(0).gameObject.SetActive(true);
            canvas.transform.GetChild(4).gameObject.SetActive(false);
            main_menu_cubes[0].transform.parent.gameObject.SetActive(true);
        }
        else if (menu_state == 5)
        {
            menu_state = 1;
            canvas.transform.GetChild(1).gameObject.SetActive(true);
            canvas.transform.GetChild(5).gameObject.SetActive(false);
            select_level_cubes[0].transform.parent.gameObject.SetActive(false);
            for (int i = 0; i < players_count; i++)
            {
                if (PlayerInputManagerSingleton.instance.transform.GetChild(i).GetComponent<PlayerInput>().devices[0].displayName == "Keyboard")
                {
                    icons[i].transform.Find("Info").GetComponent<Text>().text = "Press ENTER\nto set Ready";
                }
                else
                {
                    icons[i].transform.Find("Info").GetComponent<Text>().text = "Press (A)\nto set Ready";
                }
                icons[i].transform.Find("Comunication").GetComponent<Text>().text = "Player " + (i + 1).ToString() + " Joined!";
                readys[i] = false;
            }
        }
        else if(menu_state == 6)
        {
            Application.Quit();
        }
    }
}
