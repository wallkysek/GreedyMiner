using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CircleCollider2D))]
public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GridLayout gridLayout;
    [SerializeField] private Tilemap gridTilemap;
    [SerializeField] private List<GeneratableTile> tileList;
    [SerializeField] private TileBase trackTile;
    [SerializeField] private int deleteOffset;
    [SerializeField] private int trackQueueLength = 100;

    private CircleCollider2D _generationCollider;

    private Vector3Int _currentCell;
    private int _generationColliderRadius;

    public static Queue<Vector3Int> TrackTiles = new Queue<Vector3Int>();
    private int _previousTrackDirection = -1;

    private void Awake()
    {
        if (!this.gameObject.TryGetComponent(out _generationCollider))
        {
            throw new Exception($"No Circle collider on {gameObject.name}");
        }
        _currentCell = gridLayout.WorldToCell(gameObject.transform.position);

        _generationColliderRadius = Mathf.CeilToInt(_generationCollider.radius);
        gridTilemap.SetTile(_currentCell, trackTile);
        TrackTiles.Enqueue(_currentCell);
        
        GenerateTrackTiles();
        StartCoroutine(GenerateGroundTiles());
        StartCoroutine(GenerateTrackTiles());
        StartCoroutine(DeleteTiles());
    }

    private IEnumerator GenerateTrackTiles()
    {
        while (true)
        {
            if (TrackTiles.Count < trackQueueLength)
            {
                for (int i = 0; i < trackQueueLength - TrackTiles.Count; i++)
                {
                    var rndDirection = RndDirection();
                    var lastTrackTile = TrackTiles.Last();

                    switch (rndDirection)
                    {
                        case 0:
                            lastTrackTile.x += 1;
                            break;
                        case 1:
                            lastTrackTile.y += 1;
                            break;
                        case 2:
                            lastTrackTile.x -= 1;
                            break;
                        case 3:
                            lastTrackTile.y -= 1;
                            break;
                    }

                    gridTilemap.SetTile(lastTrackTile, trackTile);
                    TrackTiles.Enqueue(lastTrackTile);
                    _previousTrackDirection = rndDirection;
                }
            }
            yield return null;
        }
    }

    private int RndDirection()
    {
        //0 - RIGHT
        //1 - UP
        //2 - LEFT
        //3 - DOWN
        int rndDirection = -1;
        do
        {
            rndDirection = Random.Range(2, 4);
        } while (Mathf.Abs(rndDirection - _previousTrackDirection) == 2);
        return rndDirection;
    }


    private IEnumerator GenerateGroundTiles()
    {
        while (true)
        {
            Vector3Int lastPosition = _currentCell;
            for (int x = -_generationColliderRadius; x < _generationColliderRadius; x++)
            {
                for (int y = -_generationColliderRadius; y < _generationColliderRadius; y++)
                {
                    var newTilePosition = new Vector3Int(lastPosition.x + x, lastPosition.y + y, lastPosition.z);
                    if (!gridTilemap.GetTile(newTilePosition))
                    {
                        TileBase tileToPlace = null;
                        float probability = Random.Range(0f, 1f);
                        foreach (GeneratableTile tile in tileList)
                        {
                            if (tile.Probability > probability)
                            {
                                tileToPlace = tile.Tile;
                                break;
                            }

                            //probability -= tile.Probability;
                        }
                        gridTilemap.SetTile(newTilePosition, tileToPlace);
                    }
                }

                yield return null;
            }

            yield return new WaitUntil(() => Vector3Int.Distance(lastPosition, _currentCell) > 2);
        }
    }

    private IEnumerator DeleteTiles()
    {
        while (true)
        {
            Vector3Int lastPosition = _currentCell;
            for (int x = -_generationColliderRadius - deleteOffset; x < _generationColliderRadius + deleteOffset; x++)
            {
                for (int y = -_generationColliderRadius - deleteOffset;
                    y < _generationColliderRadius + deleteOffset;
                    y++)
                {
                    if ((y < -_generationColliderRadius || y > _generationColliderRadius)
                        || (x < -_generationColliderRadius || x > _generationColliderRadius))
                    {
                        var newTilePosition =
                            new Vector3Int(lastPosition.x + x, lastPosition.y + y, lastPosition.z);
                        if (!TrackTiles.Contains(newTilePosition))
                        {
                            gridTilemap.SetTile(newTilePosition, null);
                        }
                    }
                }
            }

            yield return new WaitUntil(() => Vector3Int.Distance(lastPosition, _currentCell) > 3);
        }
    }

    private void Update()
    {
        _currentCell = gridLayout.WorldToCell(gameObject.transform.position);
    }
}