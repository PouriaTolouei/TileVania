using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    Rigidbody2D myRigidbody;
    BoxCollider2D myFrontCollider;

    [SerializeField] float moveSpeed = 1f;
  
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myFrontCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        myRigidbody.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other) {
        moveSpeed *= -1;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
    }
}
