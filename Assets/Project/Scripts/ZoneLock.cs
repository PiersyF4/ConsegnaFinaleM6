using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ZoneLock : MonoBehaviour
{
    public int totalCoinsRequired = 10;      
    public CoinsManager coinManager;         
    public GameObject warningCanvas;       
    public float canvasDuration = 2f;

    private void Start()
    {
        if (warningCanvas != null)
            warningCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (coinManager.totalCoins < totalCoinsRequired)
            {
                StartCoroutine(ShowCanvas());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ShowCanvas()
    {
        warningCanvas.SetActive(true);
        yield return new WaitForSeconds(canvasDuration);
        warningCanvas.SetActive(false);
    }
}

