using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public FoodManager foodManager;
    public float moveDelay = 0.2f;
    public GameObject bodyPrefab;

    private Vector2 direction = Vector2.right;
    private List<Transform> body = new List<Transform>();

    void Start()
    {
        InvokeRepeating(nameof(Move), moveDelay, moveDelay);
    }

    void Move()
    {
        Vector3 prevPos = transform.position;

        // di chuyển đầu
        transform.position += (Vector3)direction;

        // thân đi theo
        for (int i = 0; i < body.Count; i++)
        {
            Vector3 temp = body[i].position;
            body[i].position = prevPos;
            prevPos = temp;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2.down)
            direction = Vector2.up;

        if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2.up)
            direction = Vector2.down;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right)
            direction = Vector2.left;

        if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2.left)
            direction = Vector2.right;
    }

    public List<Transform> GetBody()
    {
        return body;
    }

    public void Grow()
    {
        GameObject newPart = Instantiate(bodyPrefab);
        body.Add(newPart.transform);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            Grow();

            foodManager.SpawnFood();
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {

        Debug.Log("Va chạm: " + col.gameObject.name);
        if (col.gameObject.CompareTag("wall") ||
            col.gameObject.CompareTag("Body"))
        {
            GameManager.Instance.GameOver();
        }
    }
}