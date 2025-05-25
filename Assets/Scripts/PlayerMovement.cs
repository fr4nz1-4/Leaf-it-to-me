using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

// controls the Player movement

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 mousePosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    RockPaperScissorsScript _rockPaperScissorsScript;
    private Rigidbody2D rb;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!FindFirstObjectByType<RockPaperScissorsScript>().isMinigameActive)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = transform.position.z;
                isMoving = true;
            }

            if (gameObject.transform.position.x > targetPosition.x)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            // Skalierung anhand der y-Position
            float minY = 0.5f; 
            float maxY = -2f;

            float scale = Mathf.InverseLerp(minY, maxY, transform.position.y); // Wert zwischen 0 und 1
            float finalScale = Mathf.Lerp(0.35f, 0.5f, scale); // Skaliere zwischen 60% und 100%

            transform.localScale = new Vector3(finalScale, finalScale, 0.5f); // gleichmäßig skalieren
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
            
        }
    }

    void checkSpriteFlip()
    {
        
    }
}
