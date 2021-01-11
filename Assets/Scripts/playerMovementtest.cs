// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// public class playerMovementtest : MonoBehaviour
// {
//     // public Transform transform;
//     // public float movement = 10f;
//     // private Rigidbody2D rb2d;
//     // Start is called before the first frame update
//     // * Time.deltaTime

//     Vector3 pos;                                // For movement
//     float speed = 5.0f;                         // Speed of movement
//     void Start()
//     {
//         pos = transform.position;          // Take the initial position
//     }

//     // Update is called once per frame
//     void FixedUpdate()
//     {

//         if (Input.GetKey("left") && transform.position == pos)
//         {
//             pos += Vector3.left;
//         }
//         if (Input.GetKey("right") && transform.position == pos)
//         {
//             pos += Vector3.right;
//         }
//         if (Input.GetKey("up") && transform.position == pos)
//         {
//             pos += Vector3.up;
//         }
//         if (Input.GetKey("down") && transform.position == pos)
//         {
//             pos += Vector3.down;
//         }
//         transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
//     }
// }
