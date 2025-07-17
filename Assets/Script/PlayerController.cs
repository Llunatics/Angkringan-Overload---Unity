using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movement;
    private bool isMovementEnabled = true;

    private GameObject currentCustomer = null;
    private CustomerFollow currentWaitingCustomer = null;
    private CustomerFollow selectedCustomerToSeat = null;

    private Seat currentSeat = null;
    private bool isNearSeat = false;
    private bool isNearCustomer = false;
    private bool isNearCustomerEntrance = false;
    private bool isNearKitchen = false;
    private bool isNearFoodPickup = false;

    private string carriedFood = "";
    private string pendingOrder = "";
    private bool hasPendingOrder = false;

    void Update()
    {
        // Pergerakan
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // ----------------------------------
        // E - Interaksi Kontekstual
        // ----------------------------------

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Memilih customer antrean
            if (isNearCustomerEntrance && currentWaitingCustomer != null && !currentWaitingCustomer.IsSeated)
            {
                selectedCustomerToSeat = currentWaitingCustomer;
                selectedCustomerToSeat.FollowPlayer(transform);
                Debug.Log("Customer dipilih, tuntun ke kursi lalu tekan [E] untuk duduk.");
                return;
            }

            // Mendudukkan customer
            if (isNearSeat && selectedCustomerToSeat != null && currentSeat != null && currentSeat.IsAvailableForCustomer())
            {
                currentSeat.AssignCustomer(selectedCustomerToSeat);
                selectedCustomerToSeat = null;
                Debug.Log("Customer duduk di kursi.");
                return;
            }

            // Ambil pesanan dari customer
            if (isNearCustomer && !hasPendingOrder && string.IsNullOrEmpty(carriedFood))
            {
                CustomerOrder order = currentCustomer.GetComponent<CustomerOrder>();
                if (order != null && order.HasOrdered)
                {
                    pendingOrder = order.GetOrder();
                    hasPendingOrder = true;
                    Debug.Log("Pesanan diambil: " + pendingOrder);
                }
                return;
            }

            // Antar pesanan ke dapur
            if (isNearKitchen && hasPendingOrder)
            {
                KitchenStation.Instance.ReceiveOrder(pendingOrder);
                hasPendingOrder = false;
                Debug.Log("Pesanan dikirim ke dapur: " + pendingOrder);
                return;
            }

            // Antar makanan ke customer
            if (isNearCustomer && !string.IsNullOrEmpty(carriedFood))
            {
                CustomerOrder order = currentCustomer.GetComponent<CustomerOrder>();
                if (order != null)
                {
                    bool correct = carriedFood == order.GetOrder();
                    order.ReceiveFood(correct);
                    carriedFood = "";
                }
                return;
            }
        }

        // ----------------------------------
        // Q - Ambil makanan dari pickup
        // ----------------------------------
        if (Input.GetKeyDown(KeyCode.Q) && isNearFoodPickup)
        {
            string food = FoodReadyHandler.Instance.PickupFood();
            if (!string.IsNullOrEmpty(food))
            {
                carriedFood = food;
                Debug.Log("Mengambil makanan: " + food);
            }
        }

        // ----------------------------------
        // R - Bersihkan kursi
        // ----------------------------------
        if (Input.GetKeyDown(KeyCode.R) && isNearSeat && currentSeat != null && currentSeat.isDirty)
        {
            currentSeat.CleanSeat();
        }
    }

    void FixedUpdate()
    {
        if (isMovementEnabled)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Customer"))
        {
            var cust = collision.GetComponent<CustomerFollow>();
            if (cust != null && !cust.IsSeated)
            {
                currentWaitingCustomer = cust;
                isNearCustomerEntrance = true;
            }
            else
            {
                isNearCustomer = true;
                currentCustomer = collision.gameObject;
            }
        }

        if (collision.CompareTag("Seat"))
        {
            currentSeat = collision.GetComponent<Seat>();
            isNearSeat = true;
        }

        if (collision.CompareTag("Kitchen")) isNearKitchen = true;
        if (collision.CompareTag("FoodPickup")) isNearFoodPickup = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Customer"))
        {
            isNearCustomerEntrance = false;
            isNearCustomer = false;
            currentWaitingCustomer = null;
            currentCustomer = null;
        }

        if (collision.CompareTag("Seat"))
        {
            isNearSeat = false;
            currentSeat = null;
        }

        if (collision.CompareTag("Kitchen")) isNearKitchen = false;
        if (collision.CompareTag("FoodPickup")) isNearFoodPickup = false;
    }
}
