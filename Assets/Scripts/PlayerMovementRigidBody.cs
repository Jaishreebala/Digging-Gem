using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovementRigidBody : MonoBehaviour
{
    Vector2 oldPos;
    Vector2 pos;                                // For movement
    public float speed = 5.0f;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    public bool canMoveLeft;
    public bool canMoveRight;
    public bool canMoveUp;
    public bool canMoveDown;
    public int score = 0;
    public Text txt;
    public string scoreString;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        GameObject[] rocks;
        rocks = GameObject.FindGameObjectsWithTag("ROCK");
        GameObject[] walls;
        walls = GameObject.FindGameObjectsWithTag("WALL");
        GameObject test = walls[0];

        pos = (Vector2)transform.position;
        oldPos = pos;
        canMoveDown = true;
        canMoveLeft = true;
        canMoveRight = true;
        canMoveUp = true;

    }

    void FixedUpdate()
    {
        if (Input.GetKey("left") && (Vector2)transform.position == pos && canMoveLeft)
        {
            oldPos = pos;
            pos += Vector2.left;
            canMoveDown = true;
            canMoveRight = true;
            canMoveUp = true;
        }

        if (Input.GetKey("right") && (Vector2)transform.position == pos && canMoveRight)
        {
            oldPos = pos;
            pos += Vector2.right;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveUp = true;
        }
        if (Input.GetKey("up") && (Vector2)transform.position == pos && canMoveUp)
        {
            oldPos = pos;
            pos += Vector2.up;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = true;
        }
        if (Input.GetKey("down") && (Vector2)transform.position == pos && canMoveDown)
        {
            oldPos = pos;
            pos += Vector2.down;
            canMoveLeft = true;
            canMoveRight = true;
            canMoveUp = true;
        }

        rb.MovePosition(Vector2.MoveTowards((Vector2)transform.position, pos, Time.deltaTime * speed));
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "DIAMOND")
        {
            score++;
            scoreString = "Score: " + score.ToString();
            Destroy(col.gameObject);
            txt.GetComponent<UnityEngine.UI.Text>().text = scoreString;

            Debug.Log(score);
            //increase score
        }
        else if (col.tag == "SAND")
        {
            Debug.Log(col);
            Destroy(col.gameObject);
        }
        else if (col.tag == "WALL" || col.tag == "ROCK")
        {
            if (transform.position.x > pos.x)
            {
                canMoveLeft = false;
                pos = oldPos;
            }
            else if (transform.position.x < pos.x)
            {
                canMoveRight = false;
                pos = oldPos;
            }
            else if (transform.position.y > pos.y)
            {
                canMoveDown = false;
                pos = oldPos;
            }
            else if (transform.position.y < pos.y)
            {
                canMoveUp = false;
                pos = oldPos;
            }

        }
    }

    //   Loop through all rocks and check if any of them are above
    //  Collision enter script on all rocks - check if sand is destroyed below it and make it fall

}
