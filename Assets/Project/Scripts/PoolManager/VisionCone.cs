using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esegue un semplice controllo di visione a cono tramite una griglia di raycast
/// su pitch/yaw a partire dal forward dell'oggetto. Imposta <see cref="isVisible"/>
/// a true se almeno un raggio colpisce un collider sul <see cref="targetLayer"/>
/// entro <see cref="viewDistance"/>.
/// </summary>
public class VisionCone : MonoBehaviour
{
    public float viewDistance = 10f;          // Distanza massima di vista (raggio del raycast)
    public float viewAngle = 45f;             // Ampiezza del cono (in gradi) centrata sul forward
    public int horizontalRayCount = 20;       // Numero di campioni orizzontali (yaw)
    public int verticalRayCount = 10;         // Numero di campioni verticali (pitch)
    public LayerMask targetLayer;             // Layer dei bersagli considerati visibili
    public bool isVisible;                    // Stato di visibilità aggiornato ad ogni frame

    private void Update()
    {
        // Calcola la visibilità ad ogni frame
        CastCone();
    }

    /// <summary>
    /// Spara una serie di raycast entro un cono definito da <see cref="viewAngle"/> e
    /// imposta <see cref="isVisible"/> a true al primo impatto con <see cref="targetLayer"/>.
    /// </summary>
    /// <returns>True se il bersaglio è rilevato, altrimenti false.</returns>
    public bool CastCone()
    {
        float halfAngle = viewAngle / 2f;
        isVisible = false;

        // Itera i campioni verticali (pitch). L'uso di <= include entrambe le estremità del cono.
        for (int v = 0; v <= verticalRayCount; v++)
        {
            float pitch = -halfAngle + (viewAngle / verticalRayCount) * v;

            // Itera i campioni orizzontali (yaw)
            for (int h = 0; h <= horizontalRayCount; h++)
            {
                float yaw = -halfAngle + (viewAngle / horizontalRayCount) * h;

                // Direzione del raggio ottenuta ruotando il forward della torretta
                Vector3 dir = Quaternion.Euler(pitch, yaw, 0f) * transform.forward;

                // Raycast limitato a viewDistance e filtrato per targetLayer
                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, viewDistance, targetLayer))
                {
                    // Primo impatto trovato: il bersaglio è visibile
                    return isVisible = true;
                }
            }
        }

        // Nessun impatto rilevato
        return isVisible;
    }
}


