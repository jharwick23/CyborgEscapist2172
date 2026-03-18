using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private Tilemap doorTilemap;

    private bool isActive = true;

    public void DoorInteraction()
    {
        if(!isActive) return;

        StartCoroutine(DoorCoroutine());
    }

    IEnumerator DoorCoroutine()
    {
        isActive = false;
        doorTilemap.GetComponent<TilemapRenderer>().enabled = false;
        doorTilemap.GetComponent<TilemapCollider2D>().enabled = false;

        yield return new WaitForSeconds(5f);

        doorTilemap.GetComponent<TilemapRenderer>().enabled = true;
        doorTilemap.GetComponent<TilemapCollider2D>().enabled = true;
        isActive = true;
    }
}
