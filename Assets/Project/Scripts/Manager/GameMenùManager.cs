using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestisce le azioni del menù di gioco (ritenta e torna al menù principale).
/// </summary>
public class GameMenùManager : MonoBehaviour
{
    /// <summary>
    /// Ricarica la scena di gioco (assunta all'indice 1), ripristina la musica e il time scale.
    /// </summary>
    public void Retry()
    {
        // Carica la scena di gioco (build index 1).
        SceneManager.LoadScene(1);

        // Avvia la musica di gioco tramite l'AudioManager.
        AudioManager.Instance.PlayMusic(AudioManager.Instance.backgroundMusic);

        // Assicura che il tempo scorra normalmente (utile dopo una pausa o un game over).
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Torna al menù principale (assunto all'indice 0).
    /// </summary>
    public void Menu()
    {
        // Carica la scena del menù principale (build index 0).
        SceneManager.LoadScene(0);
    }
}
