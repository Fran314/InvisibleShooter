using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 15f;
    [SerializeField]
    private int damage = 1;

    public GameObject particle_prefab;

    private int index;

    void Update()
    {
        transform.Translate(Vector3.forward.normalized * speed * Time.deltaTime);
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    private void OnCollisionEnter(Collision collision)
    {
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
