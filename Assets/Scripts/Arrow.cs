using UnityEngine;

public class Arrow : MonoBehaviour
{

    Rigidbody2D myRigidBody;
    PlayerMovement player;
    float xSpeed;
    float arrowDirection;


    [SerializeField] float arrowSpeed = 10f;
    
    
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        xSpeed = arrowSpeed * player.transform.localScale.x;
        arrowDirection = player.transform.localScale.x;
    }

    void Update()
    {
        myRigidBody.linearVelocityX = xSpeed;
        transform.localScale = new Vector2(arrowDirection, 1f);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy") {
            Destroy(other.gameObject);
        }
            Destroy(gameObject);
    }

}
