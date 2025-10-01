using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab dell'effetto di esplosione
    public AudioClip explosionSound; // Suono dell'esplosione
    public bool destroyAfterExplode = true; // Se true, la bomba viene distrutta dopo l'esplosione
    bool exploded = false; // Per evitare esplosioni multiple


    // Rileva la collisione con il giocatore
    private void OnTriggerEnter(Collider other)
    {
        // Evita esplosioni multiple
        if (exploded) return;

        // 1) Controlla se l'oggetto che ha attivato la collisione è il giocatore
        if (other.CompareTag("Player"))
        {

            // 2) Effetto audio
            exploded = true;
            AudioManager.Instance.PlayBombSound();

            // Il giocatore perde una vita
            LifeController.instance.LoseLife();

            // 2a) Effetto visivo
            if (explosionPrefab != null)
            {
                GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                ParticleSystem ps = fx.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                    Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
                }
            }

            // 3) Suono
            if (explosionSound != null)
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);

            // 4) Distruzione bomba
            if (destroyAfterExplode)
                Destroy(gameObject);
            else
            {
                Collider c = GetComponent<Collider>();
                if (c != null) c.enabled = false;
                MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
                if (mr != null) mr.enabled = false;
            }
        }
    }
}

