using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    MinigameManager minigameManager;
    private Rigidbody2D rb;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!FindFirstObjectByType<MinigameManager>().isMinigameActive)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = transform.position.z;
                isMoving = true;
            }
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }
}
