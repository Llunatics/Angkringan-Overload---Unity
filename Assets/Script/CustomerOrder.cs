using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    public string[] possibleOrders = { "Nasi Goreng", "Mie Ayam", "Sate", "Bakso" };
    private string currentOrder;
    private bool hasOrdered = false;
    private bool hasReceivedFood = false;

    private int satisfaction = 10;
    private float waitingTimer = 0f;
    private float decayInterval = 3f;

    public float eatingDuration = 5f; // Bisa diubah di Inspector

    public bool HasOrdered => hasOrdered;

    void Update()
    {
        if (hasOrdered && !hasReceivedFood)
        {
            waitingTimer += Time.deltaTime;
            if (waitingTimer >= decayInterval)
            {
                DecreaseSatisfaction(1);
                waitingTimer = 0f;
            }
        }

        if (satisfaction <= 0 && !hasReceivedFood)
        {
            ExitCustomer();
        }
    }

    public void GenerateOrder()
    {
        if (!hasOrdered)
        {
            currentOrder = possibleOrders[Random.Range(0, possibleOrders.Length)];
            hasOrdered = true;
            Debug.Log(gameObject.name + " memesan: " + currentOrder);
        }
    }

    public string GetOrder()
    {
        return hasOrdered ? currentOrder : "";
    }

    public void ReceiveFood(bool correct)
    {
        if (!hasOrdered) return;

        if (correct)
        {
            Debug.Log($"{gameObject.name} menerima makanan yang benar: {currentOrder}");
            hasReceivedFood = true;
            hasOrdered = false;
            waitingTimer = 0f;
            Invoke(nameof(FinishEating), eatingDuration);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} menerima makanan yang SALAH!");
            DecreaseSatisfaction(3);
            PrintSatisfaction();
        }
    }

    void FinishEating()
    {
        Debug.Log($"{gameObject.name} selesai makan.");
        ExitCustomer();
    }

    void ExitCustomer()
    {
        hasOrdered = false;
        hasReceivedFood = true;
        waitingTimer = 0f;

        CustomerFollow follower = GetComponent<CustomerFollow>();
        if (follower != null)
        {
            GameObject doorOut = GameObject.FindWithTag("DoorOut");
            if (doorOut != null)
            {
                follower.ExitRestaurant(doorOut.transform.position);
            }
            else
            {
                Debug.LogWarning("DoorOut tidak ditemukan!");
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void DecreaseSatisfaction(int amount)
    {
        satisfaction -= amount;
        if (satisfaction < 0) satisfaction = 0;
        PrintSatisfaction();
    }

    public void PrintSatisfaction()
    {
        int maxBar = 10;
        string barUnit = "┃";
        string output = "";

        for (int i = 0; i < satisfaction; i++) output += barUnit;
        for (int i = satisfaction; i < maxBar; i++) output += " ";

        Debug.Log($"Kepuasan {gameObject.name}: {output} ({satisfaction}/10)");
    }
}
