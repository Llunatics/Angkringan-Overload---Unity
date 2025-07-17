using UnityEngine;
using System.Collections;

public class CustomerFollow : MonoBehaviour
{
    public float speed = 2f;
    public bool IsSeated = false;

    private Transform target;
    private Seat assignedSeat;
    private bool exiting = false;
    private Vector3 doorOutPos;

    public void FollowPlayer(Transform player)
    {
        target = player;
        IsSeated = false;
    }

    public void GoToSeat(Vector3 seatPosition, Seat seat)
    {
        assignedSeat = seat;
        StartCoroutine(MoveToPosition(seatPosition, () =>
        {
            IsSeated = true;
            GetComponent<CustomerOrder>()?.GenerateOrder();
        }));
    }

    public void ExitRestaurant(Vector3 exitPoint)
    {
        if (exiting) return;

        exiting = true;
        doorOutPos = exitPoint;

        StartCoroutine(MoveToPosition(doorOutPos, () =>
        {
            if (assignedSeat != null)
            {
                assignedSeat.SetDirty(); // Kursi menjadi kotor saat customer pergi
            }
            Destroy(gameObject);
        }));
    }

    IEnumerator MoveToPosition(Vector3 destination, System.Action onArrive)
    {
        while (Vector3.Distance(transform.position, destination) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }

        onArrive?.Invoke();
    }

    public Seat GetAssignedSeat()
    {
        return assignedSeat;
    }
}
