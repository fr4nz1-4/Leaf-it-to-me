using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

// controls the Player movement

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 mousePos;
    public Vector3 mousePosWorld;
    private Vector2 mousePosWorld2d;
    public Vector2 targetPos;
    public float moveSpeed = 5f;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private RaycastHit2D hit;
    RockPaperScissorsScript rockPaperScissorsScript;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Mouse button clicked");
            mousePos = Input.mousePosition;
            print("ScreenSpace: " + Input.mousePosition);
            mousePosWorld = mainCamera.ScreenToWorldPoint(mousePos);
            print("WorldSpace: " + mousePosWorld);
            mousePosWorld2d = new Vector2(mousePosWorld.x, mousePosWorld.y);
            
                // targetPosition.z = transform.position.z;
                hit = Physics2D.Raycast(mousePosWorld2d, Vector2.zero);

                if (hit.collider != null)
                {
                    print("Collider getroffen: " + hit.collider.name);
                    if (hit.collider.gameObject.CompareTag("Ground"))
                    {
                        targetPos = hit.point;
                        isMoving = true;
                    }
                }

            if (gameObject.transform.position.x > targetPos.x)
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
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            
            // Skalierung anhand der y-Position
            float minY = -1.5f; // höchste position / am weitesten hinten
            float maxY = -5f; // niedrigste position / am weitesten vorne

            float scale = Mathf.InverseLerp(minY, maxY, transform.position.y); // Wert zwischen 0 und 1
            float finalScale = Mathf.Lerp(0.4f, 0.5f, scale); 

            transform.localScale = new Vector3(finalScale, finalScale, 0.5f); // gleichmäßig skalieren
            
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                isMoving = false;
            }
        }
    }
}
