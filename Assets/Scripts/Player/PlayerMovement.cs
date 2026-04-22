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

        // Bloqueia movimento e animação se qualquer painel de UI estiver aberto
        if (IsAnyPanelOpen())
        {
            inputMovement = Vector2.zero;
        }

        UpdateDirectionState();
        UpdateAnimatorParameters();
        UpdateSpriteFlip();
    }

    // Verifica se algum painel gerenciado pelo CanvasManager está ativo na cena
    private bool IsAnyPanelOpen()
    {
        if (CanvasManager.Instance == null) return false;

        foreach (GameObject panel in CanvasManager.Instance.allPanels)
        {
            if (panel != null && panel.activeSelf) return true;
        }
        return false;
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
        // Usa apenas inputMovement (já zerado quando painel está aberto).
        // GetAxisRaw("Horizontal/Vertical") captura WASD e setas nativamente,
        // então as verificações extras de Input.GetKey eram redundantes.
        bool isMoving = Mathf.Abs(inputMovement.x) > deadzone
                     || Mathf.Abs(inputMovement.y) > deadzone;

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
