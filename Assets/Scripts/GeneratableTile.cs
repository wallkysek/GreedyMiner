using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "StartTile", menuName = "MapTiles/GeneratableTile", order = 0)]
public class GeneratableTile : ScriptableObject
{
    [SerializeField] private TileBase tile;
    [SerializeField] private float probability;

    public TileBase Tile => tile;

    public float Probability => probability;
}
