using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{

    [SerializeField] Tilemap tilemap;
    private Vector3 botomLeftEdge;
    private Vector3 topRightEdge;
    void Start()
    {
        botomLeftEdge = tilemap.localBounds.min + new Vector3(1f, 1f, 0f);
        topRightEdge = tilemap.localBounds.max + new Vector3(-1.5f, -1.5f, 0f);

        PlayerController.instance.SetLimit(botomLeftEdge, topRightEdge);
    }
}
