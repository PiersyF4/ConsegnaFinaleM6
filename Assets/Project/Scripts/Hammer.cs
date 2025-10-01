using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private float swingAngle = 45f;    // Angolo massimo di oscillazione
    [SerializeField] private float swingSpeed = 2f;     // Velocità di oscillazione
    [SerializeField] private float pushMultiplier = 2f; // Quanto forte spinge

    // Variabili per gestire l'oscillazione
    private Quaternion startRotation;
    private Vector3 lastPos;

    void Start()
    {
        startRotation = transform.localRotation;
        lastPos = transform.position;
    }

    void Update()
    {
        // Oscillazione come progetto precedente con Mathf.Sin
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.localRotation = startRotation * Quaternion.Euler(0f, 0f, angle);
    }

    void LateUpdate()
    {
        lastPos = transform.position;
    }

    // Rileva collisioni con il giocatore
    private void OnCollisionEnter(Collision collision)
    {
        // Uscita rapida se non è il player
        if (!collision.collider.CompareTag("Player"))
            return;

        // Usa il riferimento già disponibile nel Collision per evitare GetComponent
        var rb = collision.rigidbody;
        if (rb == null)
            return;

        // Calcola l'inverso del deltaTime
        float dt = Time.deltaTime;
        if (dt <= 0f)
            return;

        float invDt = 1f / dt;
        Vector3 hammerVelocity = (transform.position - lastPos) * invDt;

        rb.AddForce(hammerVelocity * pushMultiplier, ForceMode.Impulse);
    }
}
