using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private int player_index;
    [SerializeField]
    private Material player_material;
    [SerializeField]
    private GameObject bullet_prefab;
    [SerializeField]
    private int max_health = 3;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float shooting_cooldown = 0.5f;
    [SerializeField]
    private float time_before_shooting_exposure = 2f;
    [SerializeField]
    private float bullet_delta_alpha = 0.01f;
    [SerializeField]
    private float disappearing_delta_alpha = -2f;
    [SerializeField]
    private float appearing_delta_alpha = 2f;
    [SerializeField]
    private float disappearing_after_hit = 1f;

    public int health = 0;
    private float last_shoot = 0f;
    private float last_hit = 0f;

    private float curr_alpha = 0f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 shootDirection = Vector3.zero;
    private Vector2 inputMove = Vector2.zero;
    private Vector2 inputShoot = Vector2.zero;

    private WallsDetector walls_detector;

    private void Awake()
    {
        //Reset();
        walls_detector = GetComponentInChildren<WallsDetector>();
    }

    public void Reset(Transform spawn)
    {
        transform.position = spawn.position;
        transform.rotation = Quaternion.identity;

        health = max_health;
        last_shoot = 0f;
        curr_alpha = 1f;
    }

    public int GetPlayerIndex()
    {
        return player_index;
    }

    public void SetPlayerIndex(int i)
    {
        player_index = i;
    }

    public void SetMoveInput(Vector2 direction)
    {
        inputMove = direction;
    }

    public void SetShootingInput(Vector2 direction)
    {
        if(direction.magnitude <= 0.9f)
        {
            inputShoot = Vector2.zero;
        }
        else
        {
            inputShoot = direction;
        }
    }

    public bool Damage(int damage)
    {
        last_hit = disappearing_after_hit;
        health -= damage;
        if (health <= 0) return true;
        else return false;
    }

    void Update()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        if(last_hit >= 0)
        {
            curr_alpha += appearing_delta_alpha * Time.deltaTime;
        }
        else if(walls_detector.is_coliding > 0)
        {
            curr_alpha += appearing_delta_alpha * Time.deltaTime;
        }
        else if (last_shoot <= -time_before_shooting_exposure)
        {
            curr_alpha += bullet_delta_alpha * Time.deltaTime;
        }
        else
        {
            curr_alpha += disappearing_delta_alpha * Time.deltaTime;
        }

        curr_alpha = Mathf.Clamp(curr_alpha, 0f, 1f);
        player_material.color = new Color(player_material.color.r, player_material.color.g, player_material.color.b, curr_alpha);
        moveDirection = new Vector3(inputMove.x, 0, inputMove.y);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        transform.Translate(moveDirection * Time.deltaTime);

        if(inputShoot.magnitude > 0.1f && last_shoot <= 0f)
        {
            shootDirection = new Vector3(inputShoot.x, 0, inputShoot.y).normalized;
            double forward_angle = Mathf.Acos(Vector3.Dot(shootDirection, Vector3.forward)), right_dot = Vector3.Dot(shootDirection, Vector3.right);
            if(right_dot >= 0f)
            {
                if (forward_angle <= Mathf.PI / 8)
                    shootDirection = new Vector3(0, 0, 1);
                else if (forward_angle <= 3 * Mathf.PI / 8)
                    shootDirection = new Vector3(1, 0, 1);
                else if (forward_angle <= 5 * Mathf.PI / 8)
                    shootDirection = new Vector3(1, 0, 0);
                else if (forward_angle <= 7 * Mathf.PI / 8)
                    shootDirection = new Vector3(1, 0, -1);
                else
                    shootDirection = new Vector3(0, 0, -1);
            }
            else
            {
                if (forward_angle <= Mathf.PI / 8)
                    shootDirection = new Vector3(0, 0, 1);
                else if (forward_angle <= 3 * Mathf.PI / 8)
                    shootDirection = new Vector3(-1, 0, 1);
                else if (forward_angle <= 5 * Mathf.PI / 8)
                    shootDirection = new Vector3(-1, 0, 0);
                else if (forward_angle <= 7 * Mathf.PI / 8)
                    shootDirection = new Vector3(-1, 0, -1);
                else
                    shootDirection = new Vector3(0, 0, -1);
            }
            shootDirection = shootDirection.normalized;
            GameObject bullet = Instantiate(bullet_prefab, transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().SetDirection(shootDirection);
            bullet.GetComponent<BulletScript>().SetIndex(player_index);
            bullet.GetComponentInChildren<Light>().color = new Color(player_material.color.r, player_material.color.g, player_material.color.b);

            last_shoot = shooting_cooldown;
        }

        last_shoot -= Time.deltaTime;
        last_hit -= Time.deltaTime;
    }
}
