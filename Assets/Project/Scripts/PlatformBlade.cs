using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lama su piattaforma: alla collisione (trigger) con il Player fa perdere una vita.
/// </summary>
public class PlatformBlade : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            LifeController.instance.LoseLife();
        }
    }
}
