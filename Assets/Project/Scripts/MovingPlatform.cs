using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float speed = 2f;

    private Vector3 startPos;
    private Vector3 lastPosition;
    private Rigidbody rb;

    // Oggetti che "cavalcano" la piattaforma
    private readonly HashSet<Transform> riders = new HashSet<Transform>();

    void Start()
    {
        startPos = transform.position;
        lastPosition = startPos;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // Muovere in FixedUpdate con MovePosition per una fisica stabile
    void FixedUpdate()
    {
        float offset = Mathf.PingPong(Time.time * speed, distance);
        Vector3 targetPos = startPos + direction.normalized * offset;

        Vector3 delta = targetPos - lastPosition;

        if (delta.sqrMagnitude > 0f)
        {
            // Sposta la piattaforma
            rb.MovePosition(targetPos);

            // Applica lo stesso delta a chi è sopra
            foreach (var t in riders)
            {
                MoveRiderWithDelta(t, delta);
            }

            lastPosition = targetPos;
        }
    }

    // Aggiunge come "rider" solo se il contatto è dall'alto
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Verifica che il player sia sopra la piattaforma (normale verso l'alto)
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                riders.Add(collision.transform);
                break;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Se il contatto non è più dall'alto, rimuovi
        bool onTop = false;
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                onTop = true;
                break;
            }
        }

        if (onTop)
            riders.Add(collision.transform);
        else
            riders.Remove(collision.transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            riders.Remove(collision.transform);
        }
    }

    private static void MoveRiderWithDelta(Transform rider, Vector3 delta)
    {
        if (delta.sqrMagnitude == 0f) return;

        // Se usa CharacterController
        var cc = rider.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.Move(delta);
            return;
        }

        // Se usa Rigidbody dinamico
        var rr = rider.GetComponent<Rigidbody>();
        if (rr != null && !rr.isKinematic)
        {
            rr.MovePosition(rr.position + delta);
            return;
        }

        // Fallback: sposta il Transform
        rider.position += delta;
    }
}

