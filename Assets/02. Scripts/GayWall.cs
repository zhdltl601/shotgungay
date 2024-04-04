using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayWall : MonoBehaviour
{
    [HideInInspector] public bool fuck = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && fuck)
        {

        }
    }
}
