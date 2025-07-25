using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.AI;

// controls the Player movement

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 mousePos;
    public Vector3 mousePosWorld;
    private Vector2 _mousePosWorld2d;
    public Vector2 targetPos;
    public float moveSpeed = 5f;
    // private bool isMoving = false;
    public Rigidbody2D rb;
    public Animator animator;
    private RaycastHit2D _hit;
    // RockPaperScissorsScript rockPaperScissorsScript;
    private AudioSource _audioSource;
    // public InputActionReference move;
    // private Vector2 _movementDirection;
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    // old update method
    /*void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Mouse button clicked");
            audioSource.Play();
            mousePos = Input.mousePosition;
            print("ScreenSpace: " + Input.mousePosition);
            mousePosWorld = mainCamera.ScreenToWorldPoint(mousePos);
            print("WorldSpace: " + mousePosWorld);
            _mousePosWorld2d = new Vector2(mousePosWorld.x, mousePosWorld.y);
            
            // targetPosition.z = transform.position.z;
            hit = Physics2D.Raycast(_mousePosWorld2d, Vector2.zero);
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
        }*/

    // simple WASD movement
    /*void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w"))
        {
            pos.y += moveSpeed * Time.deltaTime;
        } else if (Input.GetKey("s"))
        {
            pos.y -= moveSpeed * Time.deltaTime;
        } else if (Input.GetKey("d"))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            pos.x += moveSpeed * Time.deltaTime;
        } else if (Input.GetKey("a"))
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            pos.x -= moveSpeed * Time.deltaTime;
        }

        transform.position = pos;
    }*/
    
    
    // movement with rigidbody --> looks better
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Mouse button clicked");
            _audioSource.Play();
            mousePos = Input.mousePosition;
            print("ScreenSpace: " + Input.mousePosition);
            mousePosWorld = mainCamera.ScreenToWorldPoint(mousePos);
            print("WorldSpace: " + mousePosWorld);
            _mousePosWorld2d = new Vector2(mousePosWorld.x, mousePosWorld.y);
        }
        targetPos.x = Input.GetAxisRaw("Horizontal");
        targetPos.y = Input.GetAxisRaw("Vertical");
        
        animator.SetFloat("horizontal", targetPos.x);
        animator.SetFloat("vertical", targetPos.y);
        animator.SetFloat("speed", targetPos.sqrMagnitude);
        
        // FlipX für Richtung
        if (targetPos.x > 0) _spriteRenderer.flipX = false;
        if (targetPos.x < 0) _spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        // Physik-konforme Bewegung
        rb.MovePosition(rb.position + targetPos.normalized * moveSpeed * Time.fixedDeltaTime);
        
        // Skalierung anhand der y-Position
        float minY = 0.1f; // höchste position / am weitesten hinten
        float maxY = -2.0f; // niedrigste position / am weitesten vorne

        float scale = Mathf.InverseLerp(minY, maxY, transform.position.y); // Wert zwischen 0 und 1
        float finalScale = Mathf.Lerp(0.5f, 0.6f, scale); 

        transform.localScale = new Vector3(finalScale, finalScale, 0.5f); // gleichmäßig skalieren

    }

    void OnDisable()
    {
        Debug.Log("PlayerMovement wurde deaktiviert!");
    }
    
    void OnEnable()
    {
        Debug.Log("PlayerMovement wurde aktiviert!");
    }
}