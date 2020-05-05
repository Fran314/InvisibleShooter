using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerScript : MonoBehaviour
{
    public GameObject canvas;
    public GameObject player_icon_prefab;
    public Sprite keyboard, gamepad;
    public float hold_time = 1f;

    private int players_count = 0;
    private List<bool> readys;
    private List<float> holds;

    void Start()
    {
        readys = new List<bool>();
        holds = new List<float>();
    }

    void Update()
    {
        for(int i = 0; i < players_count; i++)
        {
            holds[i] += Time.deltaTime;
        }
        if (PlayerInputManagerSingleton.instance == null) return;
        int new_players_count = PlayerInputManagerSingleton.instance.transform.childCount;
        if(new_players_count != players_count)
        {
            for(int i = players_count; i < new_players_count; i++)
            {
                GameObject new_icon = Instantiate(player_icon_prefab, canvas.transform);
                if(PlayerInputManagerSingleton.instance.transform.GetChild(i).GetComponent<PlayerInput>().devices[0].displayName == "Keyboard")
                {
                    new_icon.transform.GetChild(0).GetComponent<Image>().sprite = keyboard;
                }
                new_icon.GetComponent<RectTransform>().anchoredPosition = new Vector3(100*i, -64, 0);
                new_icon.transform.GetChild(1).GetComponent<Text>().text = "Player " + (i + 1).ToString() + " Joined!";
                readys.Add(false);
                holds.Add(0f);
            }

            players_count = new_players_count;
        }

        if(readys.Count >= 2 && readys.All(b => b == true))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
