using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject[] customerPrefabs; // Array dari beberapa prefab customer
    public Transform spawnPoint;
    public float minInterval = 3f;
    public float maxInterval = 8f;

    private float spawnTimer;

    void Start()
    {
        if (customerPrefabs == null || customerPrefabs.Length == 0)
        {
            Debug.LogWarning("Customer Prefabs belum di-assign! Spawner tidak akan bekerja.");
        }

        ResetSpawnTimer();
    }

    void Update()
    {
        if (customerPrefabs == null || customerPrefabs.Length == 0) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            TrySpawnCustomer();
            ResetSpawnTimer();
        }
    }

    void TrySpawnCustomer()
    {
        GameObject[] allSeats = GameObject.FindGameObjectsWithTag("Seat");

        foreach (GameObject seatObj in allSeats)
        {
            Seat seat = seatObj.GetComponent<Seat>();
            if (seat != null && !seat.isOccupied && !seat.isDirty)
            {
                // Pilih prefab customer secara acak
                GameObject chosenPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];

                GameObject customer = Instantiate(chosenPrefab, spawnPoint.position, Quaternion.identity);
                Debug.Log("Customer baru spawned.");
                return;
            }
        }

        Debug.Log("Semua kursi sedang terisi atau kotor, tunda spawn.");
    }

    void ResetSpawnTimer()
    {
        spawnTimer = Random.Range(minInterval, maxInterval);
    }
}
