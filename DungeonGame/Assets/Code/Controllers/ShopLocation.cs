using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLocation : MonoBehaviour
{
    public ShopLocationArchetype Data;
    public float Range = 3.0f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(PlayerController.Instance.transform.position, transform.position) <= Range)
        {
            ShopController.OpenShop(Data);
        }
    }
}
