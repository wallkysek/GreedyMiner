using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public TrainMovement nextTrain;
    [SerializeField] private TrainMode mode;
    
    private Vector3Int _currentGoal;
    
    private enum TrainMode
    {
        Master,
        Slave
    }
    
    // Update is called once per frame
    void Update()
    {
        if (mode == TrainMode.Master)
        {
            if (Vector3.Distance(_currentGoal, this.gameObject.transform.position) <= 0.1f)
            {
                PassNextGoal(TileGenerator.TrackTiles.Dequeue());
            }
        }
        this.gameObject.transform.position += Time.deltaTime * speed *
                                              (_currentGoal - this.gameObject.transform.position).normalized;
    }

    private void PassNextGoal(Vector3Int nextGoal)
    {
        if (nextTrain)
        {
            nextTrain.PassNextGoal(_currentGoal);   
        }
        _currentGoal = nextGoal;
    }
}
