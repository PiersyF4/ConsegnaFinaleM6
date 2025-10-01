using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Gestisce il conteggio delle vite, l'UI dei cuori, il respawn del player e lo stato di Game Over.
/// </summary>
public class LifeController : MonoBehaviour
{
    [Header("Impostazioni Vite")]
    public int maxLives = 3;     // Numero massimo di vite
    public int currentLives;     // Vite correnti

    [Header("UI Cuori")]
    public Image[] hearts;       // Array di immagini cuore da aggiornare
    public Sprite fullHeart;     // Sprite cuore pieno
    public Sprite emptyHeart;    // Sprite cuore vuoto

    [Header("Player e Respawn")]
    public Transform player;         // Riferimento al player da disattivare/riattivare
    public Transform respawnPoint;   // Punto di respawn

    [Header("Game Over")]
    public GameObject gameOverCanvas; // Canvas da mostrare a Game Over

    private bool isRespawning = false; // Flag per evitare respawn multipli
    public static LifeController instance; // Singleton semplice per accesso globale

    void Start()
    {
        instance = this;
        currentLives = maxLives;
        UpdateHeartsUI(); // Sincronizza UI con vite iniziali
    }

    /// <summary>
    /// Decrementa una vita e gestisce respawn o Game Over.
    /// </summary>
    public void LoseLife()
    {
        currentLives--;
        UpdateHeartsUI();

        if (currentLives > 0)
        {
            // Innesca il respawn dopo un breve delay.
            StartCoroutine(RespawnCoroutine());
        }
        else
        {
            GameOver();
        }
    }

    /// <summary>
    /// Disattiva temporaneamente il player, attende e lo riposiziona al punto di respawn.
    /// </summary>
    IEnumerator RespawnCoroutine()
    {
        isRespawning = true;

        if (player != null)
            player.gameObject.SetActive(false); // Disattiva il player durante il respawn

        yield return new WaitForSeconds(3f); // Delay respawn

        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
            player.gameObject.SetActive(true);
        }

        isRespawning = false;
    }

    /// <summary>
    /// Aggiorna la UI dei cuori in base a currentLives.
    /// </summary>
    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Mostra cuore pieno se l'indice è minore del numero di vite correnti.
            hearts[i].sprite = i < currentLives ? fullHeart : emptyHeart;
        }
    }

    /// <summary>
    /// Imposta le vite (clamp tra 0 e maxLives) e aggiorna la UI.
    /// </summary>
    public void SetLives(int value)
    {
        currentLives = Mathf.Clamp(value, 0, maxLives);
        UpdateHeartsUI();
    }

    /// <summary>
    /// Mostra il canvas di Game Over, riproduce il suono di sconfitta e ferma il tempo di gioco.
    /// </summary>
    void GameOver()
    {
        // ATTENZIONE: senza parentesi graffe solo la prima riga è condizionata dall'if.
        // Probabilmente si intendeva:
        // if (gameOverCanvas != null) { gameOverCanvas.SetActive(true); }
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        // Riproduce il suono di sconfitta (assume che AudioManager.Instance non sia null).
        AudioManager.Instance.PlayLoseSound();

        // Ferma il tempo di gioco.
        Time.timeScale = 0f;
    }
}
