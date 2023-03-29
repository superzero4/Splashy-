using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invertActiveStatus : MonoBehaviour
{
    public void invert()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
