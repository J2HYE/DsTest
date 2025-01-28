using System;
using Unity.VisualScripting;
using UnityEngine;

public class Trigger_H1 : MonoBehaviour
{
    public GameObject nextPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            nextPoint.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
