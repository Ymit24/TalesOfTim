using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionLinkController : MonoBehaviour
{
    public RegionLocation From, To;
    public bool AutoTravel;
    public GameObject SpawnLocation;

    public bool Is(string location)
    {
        return From.Location.Equals(location) || To.Location.Equals(location);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController ctr = collision.gameObject.GetComponent<PlayerController>();
        if (ctr == null || (Input.GetKey(KeyCode.E) == false && !AutoTravel)) return;
        RegionController.ActivateLink(To);
    }
}
