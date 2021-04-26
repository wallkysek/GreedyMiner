using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private GameObject _gun;
    [SerializeField] public GameObject _ore;
    public void DisableMineAnimation()
    {
        GetComponent<Animator>().SetBool("isMining",false);
        _ore.GetComponent<OreNodeController>().HitOre(gameObject.transform.GetComponentInParent<PlayerController>().gameObject);
        _gun.SetActive(true);
        _gun.GetComponent<GunController>()._canShoot = true;
    }
}
