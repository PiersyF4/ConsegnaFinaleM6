using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Torretta che spara periodicamente quando il bersaglio è all'interno del cono visivo.
/// Usa il <see cref="PoolManager"/> per generare i proiettili.
/// </summary>
public class FastTurret : MonoBehaviour
{
    /// <summary>
    /// Tag dell'oggetto in pool da spawnare come proiettile.
    /// </summary>
    [SerializeField] protected string bulletTag = "Bullet";

    /// <summary>
    /// Punto di spawn del proiettile; se non assegnato, usa il transform della torretta.
    /// </summary>
    [SerializeField] protected Transform spawnPoint;

    /// <summary>
    /// Intervallo tra uno sparo e l'altro (in secondi).
    /// </summary>
    [SerializeField] protected float fireInterval = 2f;

    /// <summary>
    /// Riferimento al componente che determina se il bersaglio è visibile.
    /// </summary>
    [SerializeField] protected VisionCone visionCone;

    /// <summary>
    /// Timer interno per scandire l'intervallo di fuoco.
    /// </summary>
    protected float timer;

    void Awake()
    {
        // Applica un VisionCone per attivare la torretta quando il bersaglio è visibile.
        // Se non già assegnato via Inspector, prova a recuperarlo nei figli.
        visionCone = GetComponentInChildren<VisionCone>(); 
    }

    protected void Start()
    {
        // Se non specificato, lo spawn avviene dalla posizione/rotazione della torretta.
        if (spawnPoint == null) spawnPoint = transform;

        // Inizializza il timer all'intervallo configurato.
        timer = fireInterval;
    }

    protected virtual void Update()
    {
        // Decrementa il timer a ogni frame.
        timer -= Time.deltaTime;

        // Spara solo se il bersaglio è attualmente visibile.
        if (visionCone.isVisible == true)
        {
            if (timer <= 0f)
            {
                // Riproduce SFX e spara.
                AudioManager.Instance.PlayLaserSound();
                Shoot();

                // Reset del timer.
                timer = fireInterval;
            }
        }      
    }

    /// <summary>
    /// Esegue lo spawn del proiettile dal pool.
    /// </summary>
    protected virtual void Shoot()
    {
        // Usa il PoolManager per spawnare il proiettile.
        PoolManager.Instance.SpawnFromPool(bulletTag, spawnPoint.position, spawnPoint.rotation);
    }
}

