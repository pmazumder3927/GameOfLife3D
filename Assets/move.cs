using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward* Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.position += transform.right*Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.position += transform.up* (Input.GetKey(KeyCode.Space) ?  moveSpeed*Time.deltaTime : 0);
    }
}
