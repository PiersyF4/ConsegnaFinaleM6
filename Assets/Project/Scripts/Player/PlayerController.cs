using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestisce movimento, rotazione in base alla camera e salto del player.
/// Usa Rigidbody per lo spostamento fisico in FixedUpdate.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget; // (Non utilizzato) Target per la camera, eventualmente per Cinemachine/offset
    [SerializeField] private float walkSpeed = 1f;    // Velocità camminata
    [SerializeField] private float runSpeed = 2f;     // Velocità corsa (Shift)
    [SerializeField] private float jumpForce = 5f;    // Forza del salto
    private float rotationSpeed = 0.1f;               // Tempo di smorzamento rotazione (per SmoothDampAngle)

    // Ground check
    [SerializeField] private Transform groundCheck;          // Punto sotto ai piedi
    [SerializeField] private float groundCheckRadius = 0.2f; // Raggio sfera
    [SerializeField] private LayerMask groundLayer = ~0;     // Layer del terreno (default: tutti)
    private bool isGrounded;
    private readonly Collider[] _groundHits = new Collider[8];

    private Animator _anim;
    private Rigidbody _rb;
    private Collider _col;
    private float rotationVelocity;
    private Vector3 moveDir;
    private float h;
    private float v;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    void Update()
    {
        // Input movimento
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // Aggiorna stato a terra
        UpdateGrounded();

        // Salto consentito solo se a terra
        Jump();

        // Direzione normalizzata nello spazio del mondo (prima della proiezione rispetto alla camera).
        Vector3 direction = new Vector3(h, 0f, v).normalized;

        // Calcola vettori orizzontali della camera per muovere "relative to camera".
        Vector3 _camForward = Camera.main.transform.forward;
        Vector3 _camRight = Camera.main.transform.right;

        // Annulla componente verticale per evitare drift su Y.
        _camForward.y = 0;
        _camRight.y = 0;
        _camForward.Normalize();
        _camRight.Normalize();

        // Converte input locale in direzione mondo basata sull'orientamento della camera.
        moveDir = (_camForward * direction.z + _camRight * direction.x).normalized;

        // Rotazione del personaggio verso la direzione di movimento quando c'è input significativo.
        if (moveDir.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref rotationVelocity,
                rotationSpeed
            );
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // Animator
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float _animSpeed = direction.magnitude * (isRunning ? 1f : 0.5f);
        _anim.SetFloat("Speed", _animSpeed);
    }

    private void FixedUpdate()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float _speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = moveDir * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + move);
    }

    /// <summary>
    /// Aggiorna isGrounded escludendo il proprio collider.
    /// Usa OverlapSphere su groundCheck; se assente, Raycast dal centro del collider verso il basso.
    /// </summary>
    private void UpdateGrounded()
    {
        if (groundCheck != null)
        {
            int hits = Physics.OverlapSphereNonAlloc(
                groundCheck.position,
                groundCheckRadius,
                _groundHits,
                groundLayer,
                QueryTriggerInteraction.Ignore
            );

            isGrounded = false;
            for (int i = 0; i < hits; i++)
            {
                Collider c = _groundHits[i];
                if (c == null) continue;
                if (c == _col) continue;
                if (c.attachedRigidbody != null && c.attachedRigidbody == _rb) continue;

                isGrounded = true;
                break;
            }
        }
        else
        {
            // Fallback: raycast dal centro del collider verso il basso
            Vector3 origin;
            float distance;

            if (_col != null)
            {
                origin = _col.bounds.center;
                distance = _col.bounds.extents.y + 0.05f;
            }
            else
            {
                origin = transform.position + Vector3.up * 0.1f;
                distance = 0.6f;
            }

            isGrounded = Physics.Raycast(origin, Vector3.down, distance, groundLayer, QueryTriggerInteraction.Ignore);
        }
    }

    /// <summary>
    /// Esegue un salto solo se a terra.
    /// </summary>
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Opzionale: azzera la velocità verticale per coerenza altezza salto
            Vector3 vel = _rb.velocity;
            vel.y = 0f;
            _rb.velocity = vel;

            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
#endif
}
