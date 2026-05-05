using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gerencia a movimentacao da jogadora, logica de animacao e inversao de sprite (flip).
/// Bloqueia o movimento automaticamente se houver paineis de interface abertos.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private int speed;

    private Rigidbody2D characterBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 velocity;
    private Vector2 inputMovement;

    private int lastHorizontal = 1;
    private const float deadzone = 0.1f;

    //Hashes de animacao para melhor performance
    private int movementHash = Animator.StringToHash("movement");
    private int idleHash = Animator.StringToHash("idle");

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

        //Bloqueia movimento se a UI estiver aberta
        if(IsAnyPanelOpen())
        {
            inputMovement = Vector2.zero;
        }

        UpdateDirectionState();
        UpdateAnimatorParameters();
        UpdateSpriteFlip();
    }

    //Movimentacao baseada em fisica usando Rigidbody2D
    private void FixedUpdate()
    {
        Vector2 delta = inputMovement * velocity * Time.deltaTime;
        Vector2 newPosition = characterBody.position + delta;
        characterBody.MovePosition(newPosition);
    }

    /// <summary>
    /// Le as entradas da jogadora (WASD / Setas).
    /// </summary>
    private void ReadInput()
    {
        inputMovement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }

    /// <summary>
    /// Verifica no CanvasManager se existe algum painel ativo.
    /// </summary>
    /// <returns></returns>
    private bool IsAnyPanelOpen()
    {
        if(CanvasManager.Instance == null) return false;

        foreach(GameObject panel in CanvasManager.Instance.allPanels)
        {
            if(panel != null && panel.activeSelf) return true;
        }
        return false;
    }

    /// <summary>
    /// Atualiza a ultima direcao horizontal para manter o flip correto quando parado.
    /// </summary>
    private void UpdateDirectionState()
    {
        float dx = inputMovement.x;

        if(dx > deadzone) lastHorizontal = 1;
        else if(dx < -deadzone) lastHorizontal = -1;
    }

    /// <summary>
    /// Atualiza as variaveis do Animator
    /// </summary>
    private void UpdateAnimatorParameters()
    {
        bool isMoving = Mathf.Abs(inputMovement.x) > deadzone
                     || Mathf.Abs(inputMovement.y) > deadzone;

        animator.SetBool(movementHash, isMoving);
        animator.SetBool(idleHash, !isMoving);
    }

    /// <summary>
    /// Controla a inversao horizontal do sprite.
    /// </summary>
    private void UpdateSpriteFlip()
    {
        float dx = inputMovement.x;

        if(dx > deadzone)
        {
            spriteRenderer.flipX = false;
        }
        else if(dx < -deadzone)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            //Mantem a direcao do ultimo movimento
            spriteRenderer.flipX = (lastHorizontal == -1);
        }
    }
}