using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waver : MonoBehaviour
{
    private Vector3 original_transform;
    public float wobble_factor = 10f;
    public float wobble_speed = 1f;

    void Start()
    {
        original_transform = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = original_transform + new Vector3(0f, Mathf.Abs(Mathf.Sin(Time.time * wobble_speed)) * wobble_factor, 0f);
    }
}
