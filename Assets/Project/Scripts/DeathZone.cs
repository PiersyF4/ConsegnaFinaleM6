using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zona di morte: quando il Player entra nel trigger, perde una vita,
/// viene riprodotto un suono, viene mostrata una UI (canvas) e il Player viene distrutto.
/// </summary>
public class DeathZone : MonoBehaviour
{
    [SerializeField] private GameObject _canvas; // Canvas da mostrare al contatto (es. messaggio di morte/respawn)

    void Start()
    {
        // Assicura che il canvas sia nascosto all'avvio
        _canvas.SetActive(false);
    }

    /// <summary>
    /// Se il Player entra nel trigger, applica le conseguenze della morte.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Perdita di una vita (uso del null-conditional in caso LifeController non sia ancora inizializzato)
            LifeController.instance?.LoseLife();

            // Suono di sconfitta
            AudioManager.Instance.PlayLoseSound();

            // Mostra UI (es. pannello di game over/respawn)
            _canvas.SetActive(true);

            // Distrugge l'oggetto Player (verificare che il flusso di respawn lo gestisca correttamente)
            Destroy(other.gameObject);
        }
    }
}
