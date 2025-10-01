using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestisce il movimento del proiettile e il suo ciclo di vita all'interno del pool.
/// Il proiettile avanza nella direzione del proprio forward e si disattiva dopo
/// un tempo prefissato o al contatto con un collider (es. Player).
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;    // Velocità di avanzamento (unità/secondo)
    [SerializeField] private float lifeTime = 5f;  // Durata massima prima di auto-disattivarsi

    private float timer; // Timer interno per tracciare la durata residua

    private void OnEnable()
    {
        // Ogni volta che l'oggetto viene attivato dal pool, resetta il timer di vita
        timer = lifeTime;
    }

    private void Update()
    {
        // Movimento lineare lungo il forward del proiettile
        transform.position += transform.forward * (speed * Time.deltaTime);

        // Conta alla rovescia la durata residua e disattiva l'oggetto quando scade
        timer -= Time.deltaTime;
        if (timer <= 0f)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Se colpisce il Player, invoca la perdita di vita
        if (other.CompareTag("Player"))
        {
            LifeController.instance.LoseLife();
        }

        // In ogni caso, il proiettile si disattiva per tornare nel pool
        gameObject.SetActive(false); 
    }
}


