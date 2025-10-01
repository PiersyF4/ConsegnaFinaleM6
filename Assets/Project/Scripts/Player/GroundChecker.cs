using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Semplice checker per capire se il player è a contatto con il terreno,
/// mediante un Raycast verso il basso. Espone un evento quando lo stato cambia.
/// </summary>
public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float _groundCheckDistance = 0.1f; // Distanza del ray verso il basso
    [SerializeField] private LayerMask _groundLayer;            // Layer del terreno (vedi TODO sotto)
    [SerializeField] private UnityEvent<bool> onIsGroundedChanged; // Invocato quando cambia IsGrounded

    /// <summary>
    /// Stato corrente: true se considerato "a terra".
    /// </summary>
    public bool IsGrounded { get; private set; }

    private void OnDrawGizmos()
    {
        // Visualizza la linea del raycast in Scene View per aiutare il debug.
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * _groundCheckDistance);
    }

    void Update()
    {
        bool wasGrounded = IsGrounded;

        // Esegue il raycast verso il basso per la distanza indicata.
        // TODO: filtrare con _groundLayer per evitare falsi positivi (es. altri colliders):
        // Esempio: Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer)
        IsGrounded = Physics.Raycast(transform.position, -Vector3.up, _groundCheckDistance);

        // Se lo stato cambia, notifica gli ascoltatori.
        if (wasGrounded != IsGrounded)
        {
            onIsGroundedChanged.Invoke(IsGrounded);
        }
    }
}
