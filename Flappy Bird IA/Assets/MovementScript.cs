using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public int speed = 5;
    public float range = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = -Vector2.right * speed;
        transform.position = new Vector2(transform.position.x, transform.position.y - range * Random.value);
    }

}
