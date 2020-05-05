using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private int playerIndex;
    [SerializeField]
    private Color playerColor;
    [SerializeField]
    private Transform spawn;
    [SerializeField]
    private int max_health = 3;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float shooting_cooldown = 0.5f;
    [SerializeField]
    private float bullet_offset = 1f;

    public GameObject bullet_prefab;

    private int health = 0;
    private float last_shoot = 0f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 shootDirection = Vector3.zero;
    private Vector2 inputMove = Vector2.zero;
    private Vector2 inputShoot = Vector2.zero;

    private void Awake()
    {
        Reset();
    }

    private void Reset()
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
        if(health <= 0)
        {
            Reset();
        }
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
            GameObject bullet = Instantiate(bullet_prefab, transform.position + shootDirection * bullet_offset, Quaternion.LookRotation(shootDirection));
            bullet.GetComponent<BulletScript>().SetIndex(playerIndex);
            last_shoot = shooting_cooldown;
        }

        last_shoot -= Time.deltaTime;
    }
}
