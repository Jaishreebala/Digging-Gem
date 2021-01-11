using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class colliderScriptMovement : MonoBehaviour
{
    Vector2 oldPos;
    public Vector2 pos;                                // For movement
    public float speed = 5.0f;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    public bool canMoveLeft;
    public bool canMoveRight;
    public bool canMoveUp;
    public bool canMoveDown;
    public int score = 0;
    public int lives = 3;
    public Text txt;

    public string scoreString;
    // public GameObject rock;

    public GameObject[] rocks;
    public GameObject[] diamonds;
    public GameObject losePanel;
    public GameObject winPanel;
    public int diamondsInGame;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rocks = GameObject.FindGameObjectsWithTag("ROCK");
        diamonds = GameObject.FindGameObjectsWithTag("DIAMOND");
        diamondsInGame = diamonds.Length;
        GameObject[] walls;
        walls = GameObject.FindGameObjectsWithTag("WALL");
        GameObject[] sands;
        sands = GameObject.FindGameObjectsWithTag("SAND");
        Debug.Log(diamondsInGame);
        pos = (Vector2)transform.position;
        oldPos = pos;
        canMoveDown = true;
        canMoveLeft = true;
        canMoveRight = true;
        canMoveUp = true;
    }

    void FixedUpdate()
    {
        // Debug.Log(rocks[0].transform.position);
        if (Input.GetKey("left") && (Vector2)transform.position == pos && canMoveLeft)
        {
            oldPos = pos;
            pos += Vector2.left;
            checkObjectAbove(pos.x, pos.y + 1);
            canMoveDown = true;
            canMoveRight = true;
            canMoveUp = true;
        }

        if (Input.GetKey("right") && (Vector2)transform.position == pos && canMoveRight)
        {
            oldPos = pos;
            pos += Vector2.right;
            checkObjectAbove(pos.x, pos.y + 1);
            canMoveDown = true;
            canMoveLeft = true;
            canMoveUp = true;
        }
        if (Input.GetKey("up") && (Vector2)transform.position == pos && canMoveUp)
        {
            oldPos = pos;
            pos += Vector2.up;
            checkObjectAbove(pos.x, pos.y + 1);
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
            if (score == diamondsInGame)
            {
                winPanel.gameObject.SetActive(true);
                Invoke("reload", 3);
            }
            //increase score
        }
        else if (col.collider.tag == "SAND")
        {
            // StartCoroutine(rockStop(col.transform.position.x, 0.7f));
            // StartCoroutine(rockFall(col.transform.position.x, 0.7f));
            Destroy(col.gameObject);
        }
        else if (col.collider.tag == "ROCK" && (col.collider.transform.position.y - pos.y > 0.8f))
        {
            Debug.Log("DEAD");
            --lives;

            losePanel.gameObject.SetActive(true);
            Invoke("reload", 3);
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


            Debug.Log(col.collider.tag);
            Debug.Log(col.collider.tag == "ROCK" && (col.collider.transform.position.y - pos.y > 0.8f));
            Debug.Log((col.collider.transform.position.y - pos.y));
        }
    }
    void checkObjectAbove(double x, double y)
    {
        foreach (GameObject rock in rocks)
        {
            if (((rock.transform.position.x - x <= 0.2f && rock.transform.position.x - x >= 0f) ||
            (x - rock.transform.position.x <= 0.2f && x - rock.transform.position.x >= 0.0f)) &&
            ((rock.transform.position.y - y <= 0.2f && rock.transform.position.y - y >= 0f) ||
            (y - rock.transform.position.y <= 0.2f && y - rock.transform.position.y >= 0.0f))
            )
            {
                rock.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                StartCoroutine(waitBeforeMovingObject(rock, 0.5f));
                break;
            }
        }

        foreach (GameObject rock in diamonds)
        {
            if (((rock.transform.position.x - x <= 0.2f && rock.transform.position.x - x >= 0f) ||
            (x - rock.transform.position.x <= 0.2f && x - rock.transform.position.x >= 0.0f)) &&
            ((rock.transform.position.y - y <= 0.2f && rock.transform.position.y - y >= 0f) ||
            (y - rock.transform.position.y <= 0.2f && y - rock.transform.position.y >= 0.0f)))
            {
                rock.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                StartCoroutine(waitBeforeMovingObject(rock, 0.5f));
                break;
            }
        }
    }

    IEnumerator waitBeforeMovingObject(GameObject obj, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (obj)
        {
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

    }
    void reload()
    {
        losePanel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
