using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private Transform target;

    private void Awake()
    {
        if (!target)
            target = FindObjectOfType<Character>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = target.position;
        position.z = -25.0f;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
