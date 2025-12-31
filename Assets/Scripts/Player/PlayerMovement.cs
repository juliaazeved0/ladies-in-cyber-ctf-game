using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private int speed;
    private Rigidbody2D characterBody;
    private Vector2 velocity;
    private Vector2 inputMovement;
    private SpriteRenderer spriteRenderer;

    private Animator animator;
    private const float deadzone = 0.1f;

    private int movementHash = Animator.StringToHash("movement");
    private int idleHash = Animator.StringToHash("idle");

    private int lastHorizontal = 1;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        velocity = new Vector2(speed, speed);
    }

    private void Update()
    {
        ReadInput();
        UpdateDirectionState();
        UpdateAnimatorParameters();
        UpdateSpriteFlip();
    }

    private void ReadInput()
    {
        inputMovement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }

    private void UpdateDirectionState()
    {
        float dx = inputMovement.x;

        if (dx > deadzone) lastHorizontal = 1;
        else if (dx < -deadzone) lastHorizontal = -1;
    }

    private void UpdateAnimatorParameters()
    {
        float dx = inputMovement.x;

        bool isMoving = Mathf.Abs(dx) > deadzone
                        || Mathf.Abs(inputMovement.y) > deadzone
                        || Input.GetKey(KeyCode.A)
                        || Input.GetKey(KeyCode.D)
                        || Input.GetKey(KeyCode.LeftArrow)
                        || Input.GetKey(KeyCode.RightArrow);

        animator.SetBool(movementHash, isMoving);
        animator.SetBool(idleHash, !isMoving);
    }

    private void UpdateSpriteFlip()
    {
        float dx = inputMovement.x;

        if (dx > deadzone)
        {
            spriteRenderer.flipX = false;
        }
        else if (dx < -deadzone)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = (lastHorizontal == -1);
        }
    }

    private void FixedUpdate()
    {
        Vector2 delta = inputMovement * velocity * Time.deltaTime;
        Vector2 newPosition = characterBody.position + delta;
        characterBody.MovePosition(newPosition);
    }
}
