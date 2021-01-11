using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRigidBody : MonoBehaviour
{
    Vector2 pos;                                // For movement
    public float speed = 5.0f;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        GameObject[] rocks;
        rocks = GameObject.FindGameObjectsWithTag("ROCK");
        GameObject[] walls;
        walls = GameObject.FindGameObjectsWithTag("WALL");
        pos = (Vector2)transform.position;

    }

    void FixedUpdate()
    {

        if (Input.GetKey("left") && (Vector2)transform.position == pos)
        {
            pos += Vector2.left;
        }

        if (Input.GetKey("right") && (Vector2)transform.position == pos)
        {
            pos += Vector2.right;
        }
        if (Input.GetKey("up") && (Vector2)transform.position == pos)
        {
            pos += Vector2.up;
        }
        if (Input.GetKey("down") && (Vector2)transform.position == pos)
        {
            pos += Vector2.down;
        }

        rb.MovePosition(Vector2.MoveTowards((Vector2)transform.position, pos, Time.deltaTime * speed));
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Debug.Log("called");
        if (collisionInfo.collider.tag == "WALL")
        {
            Debug.Log("HIT");
        }
    }

}
