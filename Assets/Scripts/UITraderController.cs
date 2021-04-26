using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class UITraderController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] Transform[] _cardPosition;
    [SerializeField] private List<GameObject> _cardsList;
    [SerializeField] private GameObject _goldDisable;

    private int _damageBought = 1;
    private int _cartDamageBought = 1;
    private int _movementSpeedBought = 1;
    private int _shootingRateBought = 1;
    private int _cartShootingRateBought = 1;
    private int _repairBought = 1;
    private int _oreDamageBought = 1;

    void Start()
    {
        DepthManager.Singleton.DepthIncrementedByThreshold += ShowTrader;
    }


    void ShowTrader()
    {
        _goldText.transform.parent.gameObject.SetActive(true);
        _goldText.gameObject.SetActive(true);
        _goldDisable.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        RefreshCard();
        Rerender();
        PauseManager.Singleton.PauseGame();
    }


    void RefreshCard()
    {
        Cursor.visible = true;
        List<GameObject> list = new List<GameObject>(_cardsList);
        for (int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, list.Count);
            GameObject go = Instantiate(list[random], _cardPosition[i].position, Quaternion.identity, _cardPosition[i]);
            SetValue(go);
            go.transform.GetChild(2).GetComponent<Button>().onClick
                .AddListener(delegate() { BuyCard(GetValue(go.name), go); });
            list.Remove(list[random]);
        }
    }

    void BuyCard(float value, GameObject card)
    {
        if (card.transform.GetChild(3).gameObject.activeSelf) return;

        if (ResourceManager.Singleton.GoldAmount >= value)
        {
            ResourceManager.Singleton.SubtractGold(value);
            GetCard(card);
            Rerender();
        }
    }

    public void Reroll()
    {
        float value = 3;
        if (ResourceManager.Singleton.GoldAmount >= value)
        {
            ResourceManager.Singleton.SubtractGold(value);
            Rerender();
            DeleteCards();
            RefreshCard();
        }
    }

    private void DeleteCards()
    {
        for (int i = 0; i < 3; i++)
        {
            Destroy(_cardPosition[i].GetChild(0).gameObject);
        }
    }
    public void ExitButton()
    {
        Cursor.visible = false;
        PauseManager.Singleton.UnPauseGame();
        _goldText.gameObject.SetActive(false);
        _goldDisable.SetActive(true);
        DeleteCards();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    void GetCard(GameObject card)
    {
        string cardName = card.name;
        if (cardName == "BulletCard(Clone)")
        {
            _damageBought++;
            ResourceManager.Singleton.AddDamage();
        }
        else if (cardName == "MovementCard(Clone)")
        {
            _movementSpeedBought++;
            ResourceManager.Singleton.AddMovementSpeed();
        }
        else if (cardName == "ReloadCard(Clone)")
        {
            _shootingRateBought++;
            ResourceManager.Singleton.AddShootingRate();
        }
        else if (cardName == "RepairCard(Clone)")
        {
            _repairBought++;
            ResourceManager.Singleton.AddHealth(100f);
        }
        else if (cardName == "CartDamageCard(Clone)")
        {
            _cartDamageBought++;
            ResourceManager.Singleton.AddCartDamage();
        }
        else if (cardName == "CartReloadCard(Clone)")
        {
            _cartShootingRateBought++;
            ResourceManager.Singleton.AddCartShootingRate();
        }
        else if (cardName == "OreDamageCard(Clone)")
        {
            _oreDamageBought++;
            ResourceManager.Singleton.AddOreDamage();
        }

        card.transform.GetChild(3).gameObject.SetActive(true);
    }

    float GetValue(string cardName)
    {
        if (cardName == "BulletCard(Clone)")
        {
            Analytics.CustomEvent("BulletCard");
            return 5f + (3f * (_damageBought - 1f));
        }
        else if (cardName == "MovementCard(Clone)")
        {
            Analytics.CustomEvent("MovementCard");
            return 4f + (4f * (_movementSpeedBought - 1f));
        }
        else if (cardName == "ReloadCard(Clone)")
        {
            Analytics.CustomEvent("ReloadCard");
            return 3f + (3f * (_shootingRateBought - 1f));
        }
        else if (cardName == "RepairCard(Clone)")
        {
            Analytics.CustomEvent("RepairCard");
            return 10f + (5f * (_repairBought - 1f));
        }
        else if (cardName == "CartDamageCard(Clone)")
        {
            Analytics.CustomEvent("CartDamageCard");
            return 6f + (4f * (_cartDamageBought - 1f));
        }
        else if (cardName == "CartReloadCard(Clone)")
        {
            Analytics.CustomEvent("CartReloadCard");
            return 4f + (3f * (_cartShootingRateBought - 1f));
        }
        else if (cardName == "OreDamageCard(Clone)")
        {
            Analytics.CustomEvent("OreDamageCard");
            return 7f + (5f * (_oreDamageBought - 1f));
        }
        else
        {
            return 10f;
        }
    }

    void SetValue(GameObject card)
    {
        card.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
            GetValue(card.name).ToString();
    }

    void Rerender()
    {
        float goldAmount = Mathf.Floor(ResourceManager.Singleton.GoldAmount);
        _goldText.text = goldAmount.ToString();
    }
}