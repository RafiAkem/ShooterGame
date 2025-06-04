using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10f;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > Camera.main.orthographicSize * Camera.main.aspect+ 2f) {
            Destroy(gameObject);
        }
    }
}
