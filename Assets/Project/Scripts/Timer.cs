using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Timer countdown: aggiorna il testo ogni frame e,
/// allo scadere, mostra un canvas (es. Game Over).
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Riferimento al testo UI
    [SerializeField] private float remainingTime; // Tempo rimanente in secondi
    [SerializeField] private GameObject _canvas;  // Canvas da mostrare quando il tempo finisce
    
    // Update is called once per frame
    void Update()
    {
        // Decrementa il timer se non è già a zero
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        // Quando arriva a zero, mostra il canvas una sola volta
        else if (remainingTime <= 0)
        {
            remainingTime = 0;
            _canvas.SetActive(true);
        }

        // Calcola minuti e secondi per la formattazione "M:SS"
        int minutes = Mathf.FloorToInt(remainingTime / 60F);
        int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);

        // Aggiorna il testo del timer
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
