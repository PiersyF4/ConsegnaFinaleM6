using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestisce la riproduzione di musica di sottofondo e effetti sonori (SFX) nell'intero gioco.
/// Implementa un Singleton persistente tra scene e cambia musica in base alla scena caricata.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Sorgente audio dedicata alla musica di sottofondo (tipicamente in loop)
    public AudioSource musicSource;  

    // Sorgente audio dedicata agli effetti sonori (riprodotti via PlayOneShot)
    public AudioSource sfxSource;     

    // Clip per la musica del menu (scena indice 0)
    [SerializeField] private AudioClip menuMusic;

    // Clip per la musica di gioco (scena indice 1)
    [SerializeField] public AudioClip backgroundMusic;

    // Clip per i vari effetti sonori
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip bombSound;
    [SerializeField] private AudioClip knifeSound;
    [SerializeField] private AudioClip laserSound;

    // Istanza Singleton globale dell'AudioManager
    public static AudioManager Instance;

    private void Awake()
    {
        // Implementazione del pattern Singleton:
        // - se non esiste un'istanza, questa diventa quella globale
        // - l'oggetto non viene distrutto al cambio scena
        // - ci si sottoscrive all'evento di scena caricata per cambiare musica
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // Se esiste già un'istanza, si distrugge questo duplicato
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Callback invocata quando una nuova scena è stata caricata.
    /// Cambia la musica in base all'indice di build della scena.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Nota: al caricamento della scena la musica viene riavviata.
        // Se si desidera evitare il riavvio quando è già la stessa traccia,
        // si può verificare se musicSource.clip == clip prima di avviare.
        if (scene.buildIndex == 0) 
            PlayMusic(menuMusic);

        else if (scene.buildIndex == 1) 
            PlayMusic(backgroundMusic);
    }

    /// <summary>
    /// Riproduce una clip musicale in loop sulla sorgente della musica.
    /// </summary>
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Ferma la musica di sottofondo se attualmente in riproduzione.
    /// </summary>
    public void StopBackgroundMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }

    /// <summary>
    /// Riproduce il suono della moneta (SFX).
    /// </summary>
    public void PlayCoinSound()
    {
        if (coinSound != null)
            sfxSource.PlayOneShot(coinSound);
    }

    /// <summary>
    /// Ferma la musica e riproduce il suono di vittoria (SFX).
    /// </summary>
    public void PlayWinSound()
    {
        StopBackgroundMusic();
        if (winSound != null)
            sfxSource.PlayOneShot(winSound);
    }

    /// <summary>
    /// Ferma la musica e riproduce il suono di sconfitta (SFX).
    /// </summary>
    public void PlayLoseSound()
    {
        StopBackgroundMusic();
        if (loseSound != null)
            sfxSource.PlayOneShot(loseSound);
    }

    /// <summary>
    /// Riproduce il suono del pulsante (SFX).
    /// </summary>
    public void PlayButtonSound()
    {
        if (buttonSound != null)
            sfxSource.PlayOneShot(buttonSound);
    }

    /// <summary>
    /// Riproduce il suono della bomba (SFX).
    /// </summary>
    public void PlayBombSound()
    {
        // Probabile refuso: qui si controlla loseSound invece di bombSound.
        // Funziona comunque se bombSound è assegnato, ma il null-check non è corretto per questo metodo.
        if (loseSound != null)
            sfxSource.PlayOneShot(bombSound);
    }

    /// <summary>
    /// Riproduce il suono del coltello (SFX).
    /// </summary>
    public void PlayKnifeSound()
    {
        if (knifeSound != null)
            sfxSource.PlayOneShot(knifeSound);
    }

    /// <summary>
    /// Riproduce il suono del laser (SFX).
    /// </summary>
    public void PlayLaserSound()
    {
        if (laserSound != null)
            sfxSource.PlayOneShot(laserSound);
    }
}

