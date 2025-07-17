using System.Collections.Generic;
using UnityEngine;

public class FoodReadyHandler : MonoBehaviour
{
    public static FoodReadyHandler Instance;

    private Queue<string> readyFoods = new Queue<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddReadyFood(string foodName)
    {
        readyFoods.Enqueue(foodName);
        Debug.Log("Ditambahkan ke antrean makanan siap: " + foodName);
    }

    public string PickupFood()
    {
        if (readyFoods.Count > 0)
        {
            string food = readyFoods.Dequeue();
            Debug.Log("Player mengambil makanan: " + food);
            return food;
        }
        else
        {
            Debug.Log("Belum ada makanan yang siap diambil.");
            return "";
        }
    }

    public int GetReadyFoodCount()
    {
        return readyFoods.Count;
    }
}
