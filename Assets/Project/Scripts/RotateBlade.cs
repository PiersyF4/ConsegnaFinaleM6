using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lama rotante: ruota costantemente intorno ad un asse e,
/// al contatto con il Player, riproduce un suono e infligge danno (perdita vita).
/// </summary>
public class RotateBlade : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Asse di rotazione
    [SerializeField] private float rotationSpeed = 200f;        // Velocità in gradi/secondo

    void Update()
    {
        // Rotazione continua in base ad asse e velocità
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Danneggia solo il Player
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayKnifeSound();
            LifeController.instance?.LoseLife();
        }
    }
}
