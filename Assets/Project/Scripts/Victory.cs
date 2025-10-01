using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestisce la vittoria: alla collisione del Player mostra il pannello,
/// riproduce il suono di vittoria e mette il gioco in pausa.
/// </summary>
public class Victory : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel; // Pannello UI da mostrare alla vittoria
    
    private void Awake()
    {
        // Assicura che il pannello sia nascosto all'avvio
        victoryPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Reagisce solo al Player
        if (other.CompareTag("Player"))
        {
            // Mostra UI di vittoria
            victoryPanel.SetActive(true);

            // Suono di vittoria
            AudioManager.Instance.PlayWinSound();

            // Mette in pausa il gioco (ricordarsi di ripristinare a 1f al restart)
            Time.timeScale = 0f;
        }
    }
}
