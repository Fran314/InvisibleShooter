using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] players;
    [SerializeField]
    private GameObject[] players_UI;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private Transform[] players_spawn;

    private int players_count = 0;
    private PlayerInputHandler[] players_input;
    private PlayerScript[] players_script;
    private Text[] players_scoreboard_text;
    private Text[] players_scoreboard_score;
    private Image scoreboard_background;
    private int[] players_score;
    private Image[] hearts;

    public Sprite full_heart, empty_heart;

    [SerializeField]
    private int game_state = 1;

    private bool listen_to_input = true;

    void Start()
    {
        hearts = new Image[12];
        players_script = new PlayerScript[4];
        players_scoreboard_text = new Text[4];
        players_scoreboard_score = new Text[4];
        players_score = new int[4];
        scoreboard_background = canvas.transform.Find("Scoreboard").transform.Find("BackgroundBottom").GetComponent<Image>();

        for (int i = 0; i < 4; i++)
        {
            players_scoreboard_text[i] = canvas.transform.Find("Scoreboard").transform.Find("Player" + (i + 1) + "Text").GetComponent<Text>();
            players_scoreboard_score[i] = canvas.transform.Find("Scoreboard").transform.Find("Player" + (i + 1) + "Score").GetComponent<Text>();
            players_score[i] = 0;
            for (int j = 0; j < 3; j++)
            {
                hearts[i * 3 + j] = players_UI[i].transform.GetChild(j).GetComponent<Image>();
            }
            players_script[i] = players[i].GetComponent<PlayerScript>();
        }

        if (PlayerInputManagerSingleton.instance == null) return;
        players_count = PlayerInputManagerSingleton.instance.transform.childCount;
        players_input = new PlayerInputHandler[players_count];

        scoreboard_background.GetComponent<RectTransform>().localScale = new Vector3(1, players_count, 1);

        if (players_count == 2)
        {
            players_UI[1].GetComponent<RectTransform>().anchorMin = players_UI[2].GetComponent<RectTransform>().anchorMin;
            players_UI[1].GetComponent<RectTransform>().anchorMax = players_UI[2].GetComponent<RectTransform>().anchorMax;
            players_UI[1].GetComponent<RectTransform>().anchoredPosition = players_UI[2].GetComponent<RectTransform>().anchoredPosition;
            players_spawn[1].transform.position = players_spawn[2].transform.position;
        }
        for (int i = 0; i < players_count; i++)
        {
            players_UI[i].SetActive(true);
        }
        for (int i = players_count; i < 4; i++)
        {
            players_UI[i].SetActive(false);
            players_scoreboard_score[i].gameObject.SetActive(false);
            players_scoreboard_text[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < players_count; i++)
        {
            players_input[i] = PlayerInputManagerSingleton.instance.transform.GetChild(i).GetComponent<PlayerInputHandler>();
            players[i].SetActive(true);
            players_script[i].Reset(players_spawn[i]);
            players_input[i].SetPlayerScript(players_script[i].GetComponent<PlayerScript>());
            players_input[i].game_state = 1;
        }
    }

    void Update()
    {
        for (int i = 0; i < players_count; i++)
        {
            players_input[i].game_state = game_state;
            if(players_script[i].health < 3)
            {
                hearts[i * 3 + 2].sprite = empty_heart;
            }
            if (players_script[i].health < 2)
            {
                hearts[i * 3 + 1].sprite = empty_heart;
            }
            if (players_script[i].health < 1)
            {
                players_script[i].Reset(players_spawn[i]);
                hearts[i * 3 + 2].sprite = full_heart;
                hearts[i * 3 + 1].sprite = full_heart;
            }

            players_scoreboard_score[i].text = players_score[i].ToString();
        }
    }

    public Vector3 GetClosestPlayerPositionExcept(Vector3 position, int exception)
    {
        float min_dist = Mathf.Infinity;
        int min_index = 0;
        for(int i = 1; i < 4; i++)
        {
            if(i != exception && players[i].activeSelf == true)
            {
                float curr_dist = (position - players[i].transform.position).magnitude;
                if (curr_dist < min_dist)
                {
                    min_index = i;
                    min_dist = curr_dist;
                }
            }
        }

        return players[min_index].transform.position;
    }

    public int GetClosestPlayerIndexExcept(Vector3 position, int exception)
    {
        float min_dist = Mathf.Infinity;
        int min_index = 0;
        for (int i = 1; i < 4 && i != exception; i++)
        {
            if (players[i].activeSelf == true)
            {
                float curr_dist = (position - players[i].transform.position).magnitude;
                if (curr_dist < min_dist)
                {
                    min_index = i;
                    min_dist = curr_dist;
                }
            }
        }

        return min_index;
    }

    public bool DamagePlayer(int index, int damage)
    {
        return players_script[index].Damage(damage);
    }

    public void IncreaseScore(int index)
    {
        players_score[index]++;
        if(players_score[index] >= 5)
        {
            game_state = 2;
            for (int i = 0; i < players_count; i++)
            {
                players_script[i].SetMoveInput(Vector2.zero);
                players_script[i].SetShootingInput(Vector2.zero);
            }
            canvas.transform.Find("GameOverMenu").gameObject.SetActive(true);
            canvas.transform.Find("GameOverMenu").Find("TopText").GetComponent<Text>().text = "PLAYER " + (index+1) + " WON!";
        }
    }

    public void Select(int index)
    {
        if(listen_to_input)
        {
            if (game_state == 2)
            {
                int load_level_type = PlayerInputManagerSingleton.instance.GetComponent<LoadLevelType>().load_level_type;
                string[] levels = PlayerInputManagerSingleton.instance.GetComponent<LoadLevelType>().levels;

                if (load_level_type == 0) SceneManager.LoadScene(levels[UnityEngine.Random.Range(0, levels.Length)]);
                else SceneManager.LoadScene(levels[load_level_type - 1]);
            }
            else if (game_state == 3)
            {
                game_state = 1;
                canvas.transform.Find("PauseMenu").gameObject.SetActive(false);
            }
        }
    }

    public void GoBack(int index)
    {
        if(listen_to_input)
        {
            if (game_state == 1)
            {
                game_state = 3;
                for (int i = 0; i < players_count; i++)
                {
                    players_script[i].SetMoveInput(Vector2.zero);
                    players_script[i].SetShootingInput(Vector2.zero);
                }
                canvas.transform.Find("PauseMenu").gameObject.SetActive(true);
                listen_to_input = false;
                Invoke("ListenToInput", 0.5f);
            }
            else if (game_state == 2)
            {
                Destroy(PlayerInputManagerSingleton.instance.gameObject);
                SceneManager.LoadScene("PlayerSelectScene");
            }
            else if (game_state == 3)
            {
                Destroy(PlayerInputManagerSingleton.instance.gameObject);
                SceneManager.LoadScene("PlayerSelectScene");
            }
        }
    }

    public void ListenToInput()
    {
        listen_to_input = true;
    }
}
