using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Raccoglibile: quando il Player entra nel trigger, riproduce un suono,
/// incrementa le monete e distrugge la moneta.
/// Ruota costantemente per visibilità/feedback.
/// </summary>
public class Coin : MonoBehaviour
{
    // Valore della moneta da aggiungere al totale del giocatore
    public int coinValue = 1;

    // Velocità di rotazione (usata di seguito nell'Update)
    public float speed = 3f;

    /// <summary>
    /// Alla collisione con il Player, accredita la moneta e distrugge l'oggetto.
    /// Richiede che il Collider della moneta sia Trigger.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Effetto audio di raccolta
            AudioManager.Instance.PlayCoinSound();

            // Aggiornamento conteggio monete del giocatore
            CoinsManager.instance.AddCoins(coinValue);

            // Rimozione della moneta dalla scena
            Destroy(gameObject); 
        }
    }

    /// <summary>
    /// Rotazione continua della moneta per un feedback visivo.
    /// Nota: l'uso corrente non scala per Time.deltaTime sull'asse Y (ruota 'speed' gradi a frame).
    /// Inoltre, il termine sull'asse Z è sempre 0 (0f * Time.deltaTime / 0.01f).
    /// </summary>
    private void Update()
    {
        transform.Rotate(0f, speed, 0f * Time.deltaTime / 0.01f, Space.Self);
    }
}
