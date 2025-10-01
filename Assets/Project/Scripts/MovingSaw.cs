using UnityEngine;

/// <summary>
/// Sega mobile: si muove avanti/indietro lungo una direzione usando PingPong
/// e ruota costantemente. Alla collisione con il Player infligge danno.
/// </summary>
public class MovingSaw : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.right; // direzione del binario (x=destra/sinistra, y=su/giù, z=avanti/indietro)
    [SerializeField] private  float distance = 5f;               // distanza massima di movimento
    [SerializeField] private float speed = 2f;                  // velocità avanti/indietro
    [SerializeField] private float rotationSpeed = 360f;        // velocità di rotazione in gradi/secondo
    private Vector3 startPos;                                   // posizione iniziale

    void Start()
    {
        // Salva la posizione di partenza per calcolare lo spostamento
        startPos = transform.position;
    }

    void Update()
    {
        // Calcola uno spostamento oscillante tra 0 e 'distance'
        float offset = Mathf.PingPong(Time.time * speed, distance);

        // Aggiorna la posizione lungo la direzione normalizzata
        transform.position = startPos + direction.normalized * offset;

        // Rotazione continua attorno all'asse X (aspetto estetico)
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Danneggia solo il Player
        if (other.CompareTag("Player"))
        {
            LifeController.instance ?.LoseLife();
        }
    }
}
