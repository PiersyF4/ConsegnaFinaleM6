using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Torretta rapida che spara solo quando il bersaglio è visibile dal cono visivo
/// e si trova entro una distanza massima. Ruota lo spawn point verso il Player
/// prima di sparare e riproduce un suono di laser.
/// </summary>
public class ThirdTurret : FastTurret
{
    [SerializeField] private float maxDistance = 10f; // Distanza massima a cui può sparare

    /// <summary>
    /// Aggiorna il timer di fuoco e, se il Player è nel cono visivo e abbastanza vicino,
    /// orienta lo spawn e spara rispettando l'intervallo di fuoco.
    /// </summary>
    protected override void Update()
    {
        // Countdown dell'intervallo tra colpi
        timer -= Time.deltaTime;

        // Spara solo se il bersaglio è attualmente visibile dal VisionCone
        if (visionCone.isVisible == true)
        {
            // Calcola la distanza dal Player (si assume un singleton PlayerController.instance)
            Vector3 distance = PlayerController.instance.transform.position - transform.position;

            // Verifica il raggio operativo
            if (distance.magnitude <= maxDistance)
            {
                // Rispetta il cadence rate (fireInterval)
                if (timer <= 0f)
                {
                    // Allinea lo spawn point verso il Player per direzionare il proiettile
                    spawnPoint.LookAt(PlayerController.instance.transform);

                    // Effetto sonoro e sparo
                    AudioManager.Instance.PlayLaserSound();
                    Shoot();

                    // Reset del timer
                    timer = fireInterval;
                }
            }
        }
    }
}
