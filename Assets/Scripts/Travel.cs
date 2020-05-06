using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Travel : MonoBehaviour
{
    public float max_x = 100, max_z = 100, min_x = 0, min_z = 0;
    public float initial_speed = 3f;
    public float initial_angle = 0f;

    private Vector3 speed;
    void Start()
    {
        speed = new Vector3(Mathf.Sin(initial_angle), 0, Mathf.Cos(initial_angle)) * initial_speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime);
        if(transform.position.x < min_x)
        {
            speed = new Vector3(-speed.x, 0, speed.z);
            while (transform.position.x < min_x) transform.Translate(speed * Time.deltaTime);
        }
        else if(transform.position.x > max_x)
        {
            speed = new Vector3(-speed.x, 0, speed.z);
            while (transform.position.x > max_x) transform.Translate(speed * Time.deltaTime);
        }

        if(transform.position.z < min_z)
        {
            speed = new Vector3(speed.x, 0, -speed.z);
            while (transform.position.z < min_z) transform.Translate(speed * Time.deltaTime);
        }
        else if(transform.position.z > max_z)
        {
            speed = new Vector3(speed.x, 0, -speed.z);
            while (transform.position.z > max_z) transform.Translate(speed * Time.deltaTime);
        }
    }
}
