using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 15f;
    [SerializeField]
    private int damage = 1;

    private int index;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
