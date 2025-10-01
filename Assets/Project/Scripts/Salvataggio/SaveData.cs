using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dati serializzabili del gioco da salvare su disco.
/// Contiene posizione e rotazione del player, insieme a monete e vite.
/// </summary>
[System.Serializable]
public class SaveData
{
    // Componenti della posizione del player (x, y, z)
    public float[] position = new float[3];

    // Componenti della rotazione del player come quaternion (x, y, z, w)
    public float[] rotation = new float[4];

    // Monete raccolte
    public int coins;

    // Numero di vite
    public int lifes;

    /// <summary>
    /// Costruttore vuoto richiesto da alcuni serializer (es. Newtonsoft/Unity).
    /// </summary>
    public SaveData()
    {
    }

    /// <summary>
    /// Costruttore per inizializzare tutti i campi della struttura di salvataggio.
    /// </summary>
    /// <param name="position">Array [x,y,z] della posizione.</param>
    /// <param name="rotation">Array [x,y,z,w] della rotazione (quaternion).</param>
    /// <param name="coins">Totale monete.</param>
    /// <param name="lifes">Numero vite.</param>
    public SaveData(float[] position, float[] rotation, int coins, int lifes)
    {
        this.position = position;
        this.rotation = rotation;
        this.coins = coins;
        this.lifes = lifes;
    }
}
