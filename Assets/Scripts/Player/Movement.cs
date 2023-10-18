using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Photon.Pun.MonoBehaviourPun
{
    public float speed = 15;

    Vector2 velocity;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            velocity.x = Input.GetAxis("Horizontal");
            velocity.y = Input.GetAxis("Vertical");
        }
        
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            rb.MovePosition(rb.position + velocity * speed * Time.fixedDeltaTime);
        }
    }
}
