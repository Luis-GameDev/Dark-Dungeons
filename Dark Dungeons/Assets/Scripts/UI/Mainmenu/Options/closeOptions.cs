using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeOptions : MonoBehaviour
{
    [SerializeField] private GameObject options;
    public void close() {
        options.SetActive(false);
    }
}
