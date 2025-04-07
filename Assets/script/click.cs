using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click : MonoBehaviour
{
    void OnMouseDown()
    {
        FindObjectOfType<OXMiniGame>().RotateCylinder(gameObject);
    }
}