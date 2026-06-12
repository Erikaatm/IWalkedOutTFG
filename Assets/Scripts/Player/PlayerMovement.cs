using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private AudioClip sonidoPasos; 
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1f;

        PlayerInput input = GetComponent<PlayerInput>();
        if (input != null)
        {
            input.ActivateInput();
            input.SwitchCurrentActionMap("Player");
        }

    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        
        if (PauseController.IsGamePaused)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.Sleep(); // Congela completamente el rigidbody
            animator.SetBool("isWalking", false);
            audioSource.Stop();
            return;
        }

        rb.WakeUp(); // Asegúrate de que esté activo al reanudar
        rb.linearVelocity = moveInput * moveSpeed;
        animator.SetBool("isWalking", rb.linearVelocity.magnitude > 0);

    }

    public void Move(InputAction.CallbackContext context)
    {

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
            audioSource.Stop(); // para el sonido al soltar
        }
        else
        {
            // empieza a reproducir si no est� ya sonando
            if (!audioSource.isPlaying && sonidoPasos != null)
            {
                audioSource.clip = sonidoPasos;
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }
}