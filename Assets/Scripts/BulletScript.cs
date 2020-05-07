using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float initial_speed = 40f;
    [SerializeField]
    private float speed_cap = 60f;
    [SerializeField]
    private float acceleration_factor = 15f;
    [SerializeField]
    private float acceleration_pow = 2.5f;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float delete_at_distance = 1f;
    [SerializeField]
    private GameObject particle_prefab;

    private int index;
    private GameManagerScript game_manager;
    public Vector3 velocity;

    void Start()
    {
        game_manager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    void Update()
    {
        Vector3 closest_player_position = game_manager.GetClosestPlayerPositionExcept(transform.position, index);
        int closest_player_index = game_manager.GetClosestPlayerIndexExcept(transform.position, index);
        Vector3 distance_to_player = closest_player_position - transform.position;
        Vector3 curr_acceleration = distance_to_player;
        curr_acceleration *= (1 / (Mathf.Pow(curr_acceleration.magnitude, acceleration_pow)));
        curr_acceleration *=  acceleration_factor;
        velocity += (curr_acceleration * Time.deltaTime);
        if (velocity.magnitude >= speed_cap) velocity = velocity.normalized * speed_cap;
        transform.Translate(velocity * Time.deltaTime);

        if(distance_to_player.magnitude <= delete_at_distance)
        {
            if(game_manager.DamagePlayer(closest_player_index, damage))
            {
                game_manager.IncreaseScore(index);
            }
            Instantiate(particle_prefab, closest_player_position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void SetDirection(Vector3 direction)
    {
        this.velocity = direction.normalized * initial_speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("NOPE");
            PlayerScript player = collision.collider.GetComponent<PlayerScript>();
            if (player.GetPlayerIndex() != index)
            {
                player.Damage(damage);
                Instantiate(particle_prefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
