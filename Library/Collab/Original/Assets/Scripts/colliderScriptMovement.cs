using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class colliderScriptMovement : MonoBehaviour
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
    // public GameObject rock;

    public GameObject[] rocks;
    public GameObject[] diamonds;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rocks = GameObject.FindGameObjectsWithTag("ROCK");
        diamonds = GameObject.FindGameObjectsWithTag("DIAMOND");
        GameObject[] walls;
        walls = GameObject.FindGameObjectsWithTag("WALL");
        GameObject[] sands;
        sands = GameObject.FindGameObjectsWithTag("SAND");
        pos = (Vector2)transform.position;
        oldPos = pos;
        canMoveDown = true;
        canMoveLeft = true;
        canMoveRight = true;
        canMoveUp = true;
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        }
        foreach (GameObject sand in sands)
        {
            sand.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        }
    }

    void FixedUpdate()
    {
        // Debug.Log(rocks[0].transform.position);
        if (Input.GetKey("left") && (Vector2)transform.position == pos && canMoveLeft)
        {
            oldPos = pos;
            pos += Vector2.left;
            StartCoroutine(test(pos.x, pos.y + 1, 0.5f));
            canMoveDown = true;
            canMoveRight = true;
            canMoveUp = true;
        }

        if (Input.GetKey("right") && (Vector2)transform.position == pos && canMoveRight)
        {
            oldPos = pos;
            pos += Vector2.right;
            StartCoroutine(test(pos.x, pos.y + 1, 0.5f));
            canMoveDown = true;
            canMoveLeft = true;
            canMoveUp = true;
        }
        if (Input.GetKey("up") && (Vector2)transform.position == pos && canMoveUp)
        {
            oldPos = pos;
            pos += Vector2.up;
            StartCoroutine(test(pos.x, pos.y + 1, 0.5f));
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


    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.collider.tag == "DIAMOND")
        {
            score++;
            scoreString = "Score: " + score.ToString();
            GameObject[] newDiamonds = new GameObject[diamonds.Length - 1];
            int j = 0;
            for (int i = 0; i < diamonds.Length; ++i)
            {
                if (diamonds[i] != col.gameObject)
                {
                    newDiamonds[j] = diamonds[i];
                    j++;
                }
            }
            diamonds = newDiamonds;

            Destroy(col.gameObject);
            txt.GetComponent<UnityEngine.UI.Text>().text = scoreString;

            Debug.Log(score);
            //increase score
        }
        else if (col.collider.tag == "SAND")
        {
            // StartCoroutine(rockStop(col.transform.position.x, 0.7f));
            // StartCoroutine(rockFall(col.transform.position.x, 0.7f));
            Destroy(col.gameObject);
        }
        else if (col.collider.tag == "WALL" || col.collider.tag == "ROCK")
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

    IEnumerator rockFall(double index, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        foreach (GameObject rock in rocks)
        {
            if ((rock.transform.position.x - index <= 0.2f && rock.transform.position.x - index >= 0f) ||
            (index - rock.transform.position.x <= 0.2f && index - rock.transform.position.x >= 0.0f))
            {
                rock.GetComponent<Rigidbody2D>().gravityScale = 1;
                rock.GetComponent<Rigidbody2D>().drag = 2;
            }
        }
    }
    IEnumerator rockStop(double index, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        foreach (GameObject rock in rocks)
        {
            if (rock.transform.position.x != index)
            {
                rock.GetComponent<Rigidbody2D>().gravityScale = 0;
                rock.GetComponent<Rigidbody2D>().drag = 0;
            }
        }
    }

    IEnumerator test(double x, double y, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        foreach (GameObject rock in rocks)
        {
            if (((rock.transform.position.x - x <= 0.2f && rock.transform.position.x - x >= 0f) ||
            (x - rock.transform.position.x <= 0.2f && x - rock.transform.position.x >= 0.0f)) &&
            ((rock.transform.position.y - y <= 0.2f && rock.transform.position.y - y >= 0f) ||
            (y - rock.transform.position.y <= 0.2f && y - rock.transform.position.y >= 0.0f)))
            {
                rock.GetComponent<Rigidbody2D>().gravityScale = 1;
                rock.GetComponent<Rigidbody2D>().drag = 2;
                checkAbove(x, y + 1);
            }
            else if (!((rock.transform.position.x - x <= 0.2f && rock.transform.position.x - x >= 0f) ||
            (x - rock.transform.position.x <= 0.2f && x - rock.transform.position.x >= 0.0f)))

            {
                rock.GetComponent<Rigidbody2D>().gravityScale = 0;
                rock.GetComponent<Rigidbody2D>().drag = 0;
            }
        }

        foreach (GameObject rock in diamonds)
        {
            if (((rock.transform.position.x - x <= 0.2f && rock.transform.position.x - x >= 0f) ||
            (x - rock.transform.position.x <= 0.2f && x - rock.transform.position.x >= 0.0f)) &&
            ((rock.transform.position.y - y <= 0.2f && rock.transform.position.y - y >= 0f) ||
            (y - rock.transform.position.y <= 0.2f && y - rock.transform.position.y >= 0.0f)))
            {
                rock.GetComponent<Rigidbody2D>().gravityScale = 1;
                rock.GetComponent<Rigidbody2D>().drag = 2;
                checkAbove(x, y + 1);
            }
            else if (!((rock.transform.position.x - x <= 0.2f && rock.transform.position.x - x >= 0f) ||
            (x - rock.transform.position.x <= 0.2f && x - rock.transform.position.x >= 0.0f)))

            {
                rock.GetComponent<Rigidbody2D>().gravityScale = 0;
                rock.GetComponent<Rigidbody2D>().drag = 0;
            }
        }
    }

    void checkAbove(double x, double y)
    {
        foreach (GameObject rock in rocks)
        {
            if (((rock.transform.position.x - x <= 0.2f && rock.transform.position.x - x >= 0f) ||
            (x - rock.transform.position.x <= 0.2f && x - rock.transform.position.x >= 0.0f)) &&
            ((rock.transform.position.y - y <= 0.2f && rock.transform.position.y - y >= 0f) ||
            (y - rock.transform.position.y <= 0.2f && y - rock.transform.position.y >= 0.0f)))
            {
                rock.GetComponent<Rigidbody2D>().gravityScale = 1;
                rock.GetComponent<Rigidbody2D>().drag = 2;
                checkAbove(x, y + 1);
            }
        }
        foreach (GameObject rock in diamonds)
        {
            if (((rock.transform.position.x - x <= 0.2f && rock.transform.position.x - x >= 0f) ||
            (x - rock.transform.position.x <= 0.2f && x - rock.transform.position.x >= 0.0f)) &&
            ((rock.transform.position.y - y <= 0.2f && rock.transform.position.y - y >= 0f) ||
            (y - rock.transform.position.y <= 0.2f && y - rock.transform.position.y >= 0.0f)))
            {
                rock.GetComponent<Rigidbody2D>().gravityScale = 1;
                rock.GetComponent<Rigidbody2D>().drag = 2;
                checkAbove(x, y + 1);
            }
        }
    }
}
