using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Semplice gestore di object pooling.
/// Pre-instanzia un numero definito di oggetti per ciascun tag e li ricicla tramite attivazione/disattivazione.
/// Nota: il design attuale rimette in coda immediatamente l'oggetto appena spawnato; assicurarsi
/// che gli oggetti si auto-disattivino (es. a fine vita o su impatto) per evitare riutilizzi prematuri.
/// </summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance; // Singleton ultra-semplice (non persistente, non thread-safe)

    [System.Serializable]
    public class Pool
    {
        public string tag;        // Identificatore del pool (usato per lo spawn)
        public GameObject prefab; // Prefab da instanziare
        public int size;          // Numero di istanze pre-create
    }

    public List<Pool> pools; // Configurazione dei pool da Inspector
    private Dictionary<string, Queue<GameObject>> poolDictionary; // Mappa tag -> coda di oggetti

    private void Awake()
    {
        // Inizializza il singleton. Si assume un solo PoolManager in scena.
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Pre-creazione delle istanze per ciascun pool configurato
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); // Gli oggetti partono disattivati nel pool
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// Estrae un oggetto dal pool indicato, lo posiziona/ruota e lo riattiva.
    /// L'oggetto viene anche reinserito in coda subito dopo l'estrazione.
    /// </summary>
    /// <param name="tag">Tag del pool</param>
    /// <param name="position">Posizione di spawn</param>
    /// <param name="rotation">Rotazione di spawn</param>
    /// <returns>L'istanza spawnata oppure null se il tag non esiste</returns>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool con tag {tag} non esiste!");
            return null;
        }

        // Estrae il primo oggetto disponibile dalla coda
        GameObject obj = poolDictionary[tag].Dequeue();

        // Configura e attiva l'oggetto
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        // Reinserisce subito l'oggetto in coda (pattern round-robin)
        // Attenzione: se il pool è troppo piccolo rispetto alla frequenza di spawn,
        // si rischia di riutilizzare oggetti ancora attivi.
        poolDictionary[tag].Enqueue(obj);

        return obj;
    }
}

