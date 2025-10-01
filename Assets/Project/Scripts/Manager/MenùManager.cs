using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenùManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;  // AudioSource per riprodurre i suoni
    [SerializeField] private AudioClip _buttonSound;    // Suono del pulsante

    private bool _isTransitioning = false; // Bool per prevenire input multipli durante la transizione


    // Pulsante per iniziare una nuova partita
    public void NewGame()
    {
        if (_isTransitioning) return;
        // Suona e poi aspetta che finisca prima di caricare la scena
        StartCoroutine(PlayButtonSoundThen(() => SceneManager.LoadScene("Game")));
    }

    // Pulsante per uscire dal gioco
    public void ExitGame()
    {
        if (_isTransitioning) return;
        // Suona e poi aspetta che finisca prima di caricare la scena
        StartCoroutine(PlayButtonSoundThen(() => Application.Quit()));
    }

    // Pulsante per caricare il gioco salvato
    public void LoadGame()
    {
        if (_isTransitioning) return;
        // Suona e poi aspetta che finisca prima di caricare la scena
        StartCoroutine(PlayButtonSoundThen(() => {
            SceneManager.LoadScene("Game");
            // Prova a caricare i dati salvati dopo che la scena è stata richiesta.
            SaveSystem.Instance.LoadGame();
        }));
    }

    // Pulsante per andare alle impostazioni
    public void Settings()
    {
        if (_isTransitioning) return;
        // Suona e poi aspetta che finisca prima di caricare la scena
        StartCoroutine(PlayButtonSoundThen(() => SceneManager.LoadScene("Settings")));
    }

    // Pulsante per tornare al menù principale
    public void BackToMenu()
    {
        if (_isTransitioning) return;
        // Suona e poi aspetta che finisca prima di caricare la scena
        StartCoroutine(PlayButtonSoundThen(() => SceneManager.LoadScene("MainMenu")));
    }
    
    // Suono del pulsante
    public void PlayButtonSound()
    {
        _audioSource.PlayOneShot(_buttonSound);
    }

    // Coroutine che suona il suono del pulsante e poi esegue l'azione passata come parametro
    private IEnumerator PlayButtonSoundThen(Action onComplete)
    {
        _isTransitioning = true;

        if (_audioSource != null && _buttonSound != null)
        {
            _audioSource.PlayOneShot(_buttonSound);
            // Usa WaitForSecondsRealtime così aspetta anche se timeScale == 0
            yield return new WaitForSecondsRealtime(_buttonSound.length);
        }
        else
        {
            // Se non c'è audio, aspetta un frame per evitare attivazioni multiple istantanee
            yield return null;
        }

        // Usa try/finally per assicurarsi che il flag si resetti anche se la callback lancia un'eccezione
        try
        {
            // Chiama la funzione di callback
            onComplete?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        finally
        {
            // Se il GameObject sopravvive al cambio scena il flag si resetta; altrimenti è innocuo.
            _isTransitioning = false;
        }
    }
}
