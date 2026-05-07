using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public SnakeController snake;

    [Header("3 loại food")]
    public GameObject normalFood;   // Food thường (+10 điểm)
    public GameObject speedFood;    // Food tăng tốc
    public GameObject slowFood;     // Food giảm tốc

    [Header("Map")]
    public int mapSize = 9;

    private GameObject currentFood;

    void Start()
    {
        // Spawn food đầu tiên khi bắt đầu game
        SpawnFood();
    }

    public void SpawnFood()
    {
        // ===== Xóa food cũ =====
        if (currentFood != null)
        {
            Destroy(currentFood);
        }

        // ===== FIX: random vị trí spawn =====
        Vector2 pos;

        // random cho đến khi không đè lên rắn
        do
        {
            int x = Random.Range(-mapSize, mapSize + 1);
            int y = Random.Range(-mapSize, mapSize + 1);

            pos = new Vector2(x, y);

        } while (IsOnSnake(pos));

        // ===== Random loại food =====
        int rand = Random.Range(0, 100);

        GameObject selectedFood = null;

        // 70% food thường
        if (rand < 70)
        {
            selectedFood = normalFood;
            Debug.Log("Spawn Food thường");
        }
        // 15% tăng tốc
        else if (rand < 85)
        {
            selectedFood = speedFood;
            Debug.Log("Spawn Food tăng tốc");
        }
        // 15% giảm tốc
        else
        {
            selectedFood = slowFood;
            Debug.Log("Spawn Food giảm tốc");
        }

        // ===== Check prefab =====
        if (selectedFood == null)
        {
            Debug.LogError("Chưa kéo prefab food vào Inspector!");
            return;
        }

        // ===== Spawn =====
        currentFood = Instantiate(selectedFood, pos, Quaternion.identity);

        Debug.Log("Spawn tại vị trí: " + pos);
    }

    bool IsOnSnake(Vector2 pos)
    {
        // check đầu rắn
        if ((Vector2)snake.transform.position == pos)
            return true;

        // check thân rắn
        foreach (Transform part in snake.GetBody())
        {
            if ((Vector2)part.position == pos)
                return true;
        }

        return false;
    }
}