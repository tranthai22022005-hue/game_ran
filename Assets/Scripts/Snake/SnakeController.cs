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
        // ===== Tạo sẵn 3 đốt =====
        CreateStartingBody();

        // ===== Bắt đầu di chuyển =====
        InvokeRepeating(nameof(Move), moveDelay, moveDelay);
    }

    void CreateStartingBody()
    {
        for (int i = 1; i <= 3; i++)
        {
            Vector3 spawnPos = transform.position - new Vector3(i, 0, 0);

            GameObject newPart = Instantiate(bodyPrefab, spawnPos, Quaternion.identity);

            body.Add(newPart.transform);
        }
    }

    void Move()
    {
        Vector3 prevPos = transform.position;

        // ===== Di chuyển đầu =====
        transform.position += (Vector3)direction;

        // ===== Thân đi theo =====
        for (int i = 0; i < body.Count; i++)
        {
            Vector3 temp = body[i].position;
            body[i].position = prevPos;
            prevPos = temp;
        }

        // ===== Check va chạm =====
        CheckSelfCollision();
        CheckEatFood();
    }

    void CheckSelfCollision()
    {
        foreach (Transform part in body)
        {
            if (part.position == transform.position)
            {
                Debug.Log("Va chạm thân!");

                if (GameManager.Instance != null)
                    GameManager.Instance.GameOver();

                break;
            }
        }
    }

    void CheckEatFood()
    {
        // ===== Tìm food hiện tại =====
        GameObject food = GameObject.FindGameObjectWithTag("Food");

        if (food == null) return;

        // ===== Chỉ ăn khi đứng đúng ô =====
        if (transform.position == food.transform.position)
        {
            Debug.Log("Ăn: " + food.name);

            // ===== Xử lý theo loại food =====
            if (food.name.Contains("Normal"))
            {
                // food thường
                Grow();

                if (GameManager.Instance != null)
                    GameManager.Instance.AddScore(10);
            }
            else if (food.name.Contains("Speed"))
            {
                // food tăng tốc
                Grow();

                if (GameManager.Instance != null)
                    GameManager.Instance.AddScore(10);

                StartCoroutine(SpeedBoost());
            }
            else if (food.name.Contains("Slow"))
            {
                // food giảm tốc
                Grow();

                if (GameManager.Instance != null)
                    GameManager.Instance.AddScore(10);

                StartCoroutine(SlowBoost());
            }

            // ===== Xóa food cũ =====
            Destroy(food);

            // ===== Spawn food mới =====
            foodManager.SpawnFood();
        }
    }

    // ===== Tăng tốc 5 giây =====
    IEnumerator SpeedBoost()
    {
        CancelInvoke(nameof(Move));

        float oldDelay = moveDelay;
        moveDelay = 0.1f;

        InvokeRepeating(nameof(Move), moveDelay, moveDelay);

        Debug.Log("Tăng tốc!");

        yield return new WaitForSeconds(5f);

        CancelInvoke(nameof(Move));
        moveDelay = oldDelay;
        InvokeRepeating(nameof(Move), moveDelay, moveDelay);

        Debug.Log("Hết tăng tốc");
    }

    // ===== Giảm tốc 5 giây =====
    IEnumerator SlowBoost()
    {
        CancelInvoke(nameof(Move));

        float oldDelay = moveDelay;
        moveDelay = 0.35f;

        InvokeRepeating(nameof(Move), moveDelay, moveDelay);

        Debug.Log("Giảm tốc!");

        yield return new WaitForSeconds(5f);

        CancelInvoke(nameof(Move));
        moveDelay = oldDelay;
        InvokeRepeating(nameof(Move), moveDelay, moveDelay);

        Debug.Log("Hết giảm tốc");
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
        Vector3 spawnPos;

        // ===== Spawn nối đuôi =====
        if (body.Count == 0)
            spawnPos = transform.position - (Vector3)direction;
        else
            spawnPos = body[body.Count - 1].position;

        GameObject newPart = Instantiate(bodyPrefab, spawnPos, Quaternion.identity);

        body.Add(newPart.transform);

        // ===== FIX: bỏ cộng điểm ở đây =====
        // Điểm xử lý trong CheckEatFood()
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Chạm: " + other.tag);

        // ===== Va tường = thua =====
        if (other.CompareTag("Wall"))
        {
            Debug.Log("Va chạm tường!");

            if (GameManager.Instance != null)
                GameManager.Instance.GameOver();
        }
    }
}