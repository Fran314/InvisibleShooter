using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float initial_speed = 15f;
    [SerializeField]
    private float acceleration_factor = 15f;
    [SerializeField]
    private int damage = 1;
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
        Vector3 curr_acceleration = game_manager.GetClosestPlayerPositionExcept(transform.position, index) - transform.position;
        curr_acceleration *= (1 / (curr_acceleration.magnitude * curr_acceleration.magnitude * curr_acceleration.magnitude * curr_acceleration.magnitude));
        curr_acceleration *=  acceleration_factor;
        velocity = velocity + (curr_acceleration * Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);
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
        Debug.Log("WELP");
        if (collision.collider.tag == "Player")
        {
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
