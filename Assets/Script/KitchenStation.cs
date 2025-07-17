using UnityEngine;
using System.Collections;

public class KitchenStation : MonoBehaviour
{
    public static KitchenStation Instance;

    [SerializeField] private float minCookTime = 2f;
    [SerializeField] private float maxCookTime = 5f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ReceiveOrder(string foodName)
    {
        Debug.Log("Menerima pesanan baru: " + foodName);
        StartCoroutine(CookOrder(foodName));
    }

    private IEnumerator CookOrder(string foodName)
    {
        float cookTime = Random.Range(minCookTime, maxCookTime);
        Debug.Log("Memasak " + foodName + " selama " + cookTime + " detik...");
        yield return new WaitForSeconds(cookTime);

        FoodReadyHandler.Instance.AddReadyFood(foodName);
        Debug.Log("Makanan siap: " + foodName);
    }
}
