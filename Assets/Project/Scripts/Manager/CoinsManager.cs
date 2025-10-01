using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// Gestisce il conteggio delle monete del gioco e l'aggiornamento della UI.
/// Implementa un pattern Singleton "soft" (non persistente tra scene) per un accesso globale rapido.
/// </summary>
public class CoinsManager : MonoBehaviour
{
    /// <summary>
    /// Riferimento al testo TMP in cui mostrare il conteggio delle monete.
    /// Assegnare questo campo dall'Inspector.
    /// </summary>
    [SerializeField] private TextMeshProUGUI coinsText;

    /// <summary>
    /// Numero massimo di monete visualizzato/atteso in UI.
    /// Nota: non viene applicato un clamp automatico a <see cref="totalCoins"/>.
    /// </summary>
    [SerializeField] private int maxCoins = 10;

    /// <summary>
    /// Conteggio corrente totale delle monete.
    /// </summary>
    public int totalCoins = 0;

    /// <summary>
    /// Istanza Singleton accessibile globalmente.
    /// </summary>
    public static CoinsManager instance;

    private void Awake()
    {
        // Inizializza il Singleton: mantiene la prima istanza e distrugge eventuali duplicati nella stessa scena.
        // Nota: non viene usato DontDestroyOnLoad, quindi non persiste tra scene.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // All'avvio aggiorna la UI per riflettere il valore iniziale di totalCoins.
        UpdateUI();
    }

    /// <summary>
    /// Aggiunge un valore (positivo o negativo) al totale delle monete e aggiorna la UI.
    /// </summary>
    /// <param name="value">Numero di monete da aggiungere (può essere negativo per sottrarre).</param>
    public void AddCoins(int value)
    {
        totalCoins += value;
        // Nota: non viene applicato un clamp a 0 o a maxCoins; gestirlo a monte o aggiungere clamp se necessario.
        UpdateUI();
    }

    /// <summary>
    /// Aggiorna il testo della UI con il formato "Coins X/ Y".
    /// </summary>
    public void UpdateUI()
    {
        // Aggiorna la stringa mostrata a video. Assicurarsi che coinsText sia assegnato in Inspector.
        coinsText.text = "Coins " + totalCoins + "/ " + maxCoins;
    }
   
    /// <summary>
    /// Imposta direttamente il totale delle monete e aggiorna la UI.
    /// </summary>
    /// <param name="value">Nuovo valore assoluto del totale monete.</param>
    public void SetCoins(int value)
    {
        totalCoins = value;
        UpdateUI();
    }
}
