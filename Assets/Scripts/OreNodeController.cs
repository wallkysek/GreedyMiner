using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreNodeController : MonoBehaviour
{
    [SerializeField] public float _hitPoints = 3;
    [SerializeField] private SpriteRenderer _ore;
    [SerializeField] private Sprite[] _oreSprites;
    [SerializeField] private GameObject _button;
    [SerializeField] public float _goldAmount;
    [SerializeField] private GameObject _explosionParticle;

    public void Start()
    {
        int random = Random.Range(0, _oreSprites.Length);
        _ore.sprite = _oreSprites[random];
        _hitPoints += DepthManager.Singleton.GetModifier()/1.85f;
    }
    public void HitOre(GameObject player)
    {
        if (player.GetComponent<PlayerController>()._playerAnimator.GetBool("isMining"))
        {
            return;
        }
        
        _hitPoints -= ResourceManager.Singleton.PlayerOreDamage;
        if (_hitPoints < 1f)
        {
            GameObject particle = Instantiate(_explosionParticle, transform.position, Quaternion.identity);
            particle.SetActive(true);
            Destroy(particle, 0.5f);
            gameObject.SetActive(false);
            player.gameObject.GetComponent<PlayerController>()._ore = null;
            ResourceManager.Singleton.AddGold(_goldAmount);
        }
    }
    
    public void ShowMineable(bool isActive)
    {
        _button.SetActive(isActive);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowMineable(true);
            other.gameObject.GetComponent<PlayerController>()._canMine = true;
            other.gameObject.GetComponent<PlayerController>()._ore = gameObject;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowMineable(false);
            other.gameObject.GetComponent<PlayerController>()._canMine = false;
            other.gameObject.GetComponent<PlayerController>()._ore = null;
        }
    }

}
