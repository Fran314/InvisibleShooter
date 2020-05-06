using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private int playerIndex;
    [SerializeField]
    private Color player_color;
    [SerializeField]
    private int max_health = 3;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float shooting_cooldown = 0.5f;
    [SerializeField]
    private float bullet_offset = 1f;
    [SerializeField]
    private GameObject bullet_prefab;

    public int health = 0;
    private float last_shoot = 0f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 shootDirection = Vector3.zero;
    private Vector2 inputMove = Vector2.zero;
    private Vector2 inputShoot = Vector2.zero;

    private void Awake()
    {
        //Reset();
    }
    public void Reset(Transform spawn)
    {
        transform.position = spawn.position;
        transform.rotation = Quaternion.identity;

        health = max_health;
        last_shoot = 0f;
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public void SetPlayerIndex(int i)
    {
        playerIndex = i;
    }

    public void SetInputVector(Vector2 direction)
    {
        inputMove = direction;
    }

    public void Shoot(Vector2 direction)
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

    public void Damage(int damage)
    {
        health -= damage;
    }

    void Update()
    {
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
            GameObject bullet = Instantiate(bullet_prefab, transform.position + shootDirection * bullet_offset, Quaternion.LookRotation(shootDirection));
            bullet.GetComponent<BulletScript>().SetIndex(playerIndex);
            bullet.GetComponentInChildren<Light>().color = player_color;
            last_shoot = shooting_cooldown;
        }

        last_shoot -= Time.deltaTime;
    }
}
