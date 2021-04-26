using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CargoSpawner : MonoBehaviour
{
    [SerializeField] private bool isLast;
    private CargoSpawner _previousSpawner;
    private AudioSource _explodeAudio;

    public delegate void CargoUpdate(GameObject newCargoTrain);

    public void Awake()
    {
        this.TryGetComponent(out _explodeAudio);
    }

    private void Start()
    {
        ResourceManager.Singleton.goldThresholdReached += GoldUpdated;
    }

    private void OnDestroy()
    {
        ResourceManager.Singleton.goldThresholdReached -= GoldUpdated;
    }

    private void GoldUpdated(ResourceManager.ThresholdTrend trend)
    {
        if (isLast)
        {
            if (trend == ResourceManager.ThresholdTrend.UP)
            {
                var nextCargo = Instantiate(this.gameObject);
                var cargoSpawner = nextCargo.GetComponent<CargoSpawner>();
                cargoSpawner.isLast = true;
                this.isLast = false;
                cargoSpawner._previousSpawner = this;
                this.gameObject.GetComponent<TrainMovement>().nextTrain = nextCargo.GetComponent<TrainMovement>();
                    
            }
            else
            {
                if (_previousSpawner)
                {
                    _previousSpawner.gameObject.GetComponent<TrainMovement>().nextTrain = null;    
                    _previousSpawner.isLast = true;
                    if (_explodeAudio)
                    {
                        _explodeAudio.Play();
                        var explodeAudioClip = _explodeAudio.clip;

                        ParticleSystem explosionParticles;
                        if (TryGetComponent(out explosionParticles))
                        {
                            explosionParticles.Play();
                        }

                        SpriteRenderer cargoSprite;
                        if (TryGetComponent(out cargoSprite))
                        {
                            cargoSprite.enabled = false;
                        }
                        Destroy(this.gameObject, (float)explodeAudioClip.samples / explodeAudioClip.frequency);
                    }
                    else
                    {
                        Destroy(this.gameObject);   
                    }
                }
            }
        }
    }
    
}
