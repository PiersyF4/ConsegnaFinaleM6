using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Variante di <see cref="FastTurret"/> che spara due proiettili per colpo:
/// uno dal punto base della torretta (via base.Shoot) e uno da un secondo spawnpoint.
/// </summary>
public class Secondturret : FastTurret
{
    [SerializeField] private Transform spawnpoint2; // Secondo punto di spawn; assegnare da Inspector

    /// <summary>
    /// Esegue lo sparo di due proiettili: quello standard della base + uno dal secondo punto.
    /// </summary>
    protected override void Shoot()
    {
        // Spara dal punto di spawn principale definito in FastTurret
        base.Shoot();

        // Spara un secondo proiettile dal punto aggiuntivo
        PoolManager.Instance.SpawnFromPool(bulletTag, spawnpoint2.position, spawnpoint2.rotation);
    }
}
