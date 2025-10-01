using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Sistema di salvataggio/caricamento in JSON (singleton persistente).
/// - Salva posizione/rotazione del player, monete e vite.
/// - Scrive/legge un file in Application.persistentDataPath.
/// - All'avvio di una scena specifica (buildIndex == 1) tenta di caricare e applicare i dati.
/// </summary>
public class SaveSystem : MonoBehaviour
{
    [SerializeField] private Transform _player;   // Riferimento al Transform del player (assegnato o trovato a runtime)
    public SaveData SaveData { get; private set; } // Ultimi dati di salvataggio in memoria
    public bool _isLoad { get; private set; }      // Flag interno: indica che un salvataggio è stato caricato e va applicato

    private string _path; // Percorso completo del file di salvataggio
    private string _data; // Cache stringa JSON

    public static SaveSystem Instance; // Singleton semplice (non thread-safe)

    private void Awake()
    {
        // Inizializza struttura dati e percorso (nota: il primo _path viene sovrascritto se questa è l'istanza valida)
        SaveData = new SaveData();
        _path = Application.dataPath + "/savefile.txt"; // Percorso non persistente; verrà rimpiazzato dalla versione persistente sotto

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Ascolta il cambio scena per applicare eventuali dati caricati quando la scena è pronta
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Percorso persistente consigliato per salvataggi utente
            _path = Path.Combine(Application.persistentDataPath, "save.json");
        }
        else
        {
            // Evita duplicati del singleton
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Quando una scena viene caricata, se è quella target (buildIndex == 1),
    /// attende un frame e poi applica i dati al player (se presenti).
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            StartCoroutine(LoadPlayerDelayed());
        }
    }

    /// <summary>
    /// Attende un frame per garantire che gli oggetti di scena (es. Player) siano inizializzati,
    /// poi trova il player e applica i dati se è stato effettuato un LoadGame.
    /// </summary>
    private IEnumerator LoadPlayerDelayed()
    {
        yield return null; // attende un frame
        FoundPlayer();
        if (_isLoad) LoadPlayerInfo();
    }

    private void Update()
    {
        // Shortcut da tastiera per test: salva quando si preme Y
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SaveGame();
        }
    }

    /// <summary>
    /// Serializza lo stato corrente del player (posizione/rotazione), monete e vite su disco.
    /// </summary>
    public void SaveGame()
    {
        // Estrae posizione (x,y,z)
        float[] position = new float[3];
        position[0] = _player.position.x;
        position[1] = _player.position.y;
        position[2] = _player.position.z;

        // Estrae rotazione come quaternion (x,y,z,w)
        float[] rotation = new float[4];
        rotation[0] = _player.rotation.x;
        rotation[1] = _player.rotation.y;
        rotation[2] = _player.rotation.z;
        rotation[3] = _player.rotation.w;

        // Altri dati di gioco
        int coins = CoinsManager.instance.totalCoins;
        int lifes = LifeController.instance.currentLives;

        // Costruisce il payload e serializza in JSON
        SaveData = new SaveData(position, rotation, coins, lifes);
        _data = JsonConvert.SerializeObject(SaveData, Formatting.Indented);

        // Scrive su file (in persistentDataPath)
        try
        {
            File.WriteAllText(_path, _data);
        }
        catch
        {
            Debug.Log("Error saving file");
        }
    }

    /// <summary>
    /// Carica il file di salvataggio se esiste e imposta il flag per applicarlo al player.
    /// </summary>
    public void LoadGame()
    {
        if (File.Exists(_path))
        {
            _data = File.ReadAllText(_path);
            SaveData = JsonConvert.DeserializeObject<SaveData>(_data);
            _isLoad = true; // segnala che i dati vanno applicati alla prossima scena/aggiornamento
        }
        else
        {
            Debug.Log("No save file found");
        }
    }

    /// <summary>
    /// Applica i dati caricati al player (posizione, rotazione, vite, monete).
    /// Richiama FoundPlayer se il riferimento al player non è ancora disponibile.
    /// </summary>
    public void LoadPlayerInfo()
    {
        if (_player == null)
        {
            FoundPlayer();
        }

        if (_isLoad && _player != null)
        {
            // Ripristina trasform del player
            _player.position = new Vector3(SaveData.position[0], SaveData.position[1], SaveData.position[2]);
            _player.rotation = new Quaternion(SaveData.rotation[0], SaveData.rotation[1], SaveData.rotation[2], SaveData.rotation[3]);

            // Ripristina stato di gioco
            LifeController.instance.SetLives(SaveData.lifes);
            CoinsManager.instance.SetCoins(SaveData.coins);

            // Consuma il flag: i dati sono stati applicati
            _isLoad = false;
        }
    }

    /// <summary>
    /// Trova e assegna il Transform del player tramite il singleton PlayerController.
    /// </summary>
    public void FoundPlayer()
    {
        _player = PlayerController.instance.transform;
    }
}


