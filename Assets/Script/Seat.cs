using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool isOccupied = false;
    public bool isDirty = false;

    private CustomerFollow currentCustomer;

    public void AssignCustomer(CustomerFollow customer)
    {
        if (isDirty)
        {
            Debug.Log("Kursi kotor, customer tidak mau duduk.");
            return;
        }

        isOccupied = true;
        currentCustomer = customer;
        customer.GoToSeat(transform.position, this);
    }

    public void SetDirty()
    {
        isDirty = true;
        isOccupied = false;
        currentCustomer = null;
        Debug.Log("Kursi jadi kotor setelah customer pergi.");
    }

    public void CleanSeat()
    {
        if (!isDirty)
        {
            Debug.Log("Kursi ini sudah bersih.");
            return;
        }

        isDirty = false;
        Debug.Log("Kursi sudah dibersihkan!");
    }

    public void FreeSeat()
    {
        isOccupied = false;
        currentCustomer = null;
    }

    public bool IsAvailableForCustomer()
    {
        return !isOccupied && !isDirty;
    }
}
