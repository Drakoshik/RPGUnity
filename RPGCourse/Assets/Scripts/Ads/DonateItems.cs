using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct DonatItems
{
    public ItemManager item;
    public int priceInGems;
}

[System.Serializable]
public struct ActiveItemButton
{
    public Image activeItemImage;

    public TextMeshProUGUI name;
    public TextMeshProUGUI decs;
    public TextMeshProUGUI textOnButton;

    public Button button;

    public GameObject activeButtonField;
}


public class DonateItems : MonoBehaviour
{
    [SerializeField] bool isRandom = true;
    [SerializeField] List<DonatItems> _itemsList;
    [SerializeField] DonatItems[] _activeItemsList = new DonatItems[3];
    [SerializeField] ActiveItemButton[] _activeItemButtons = new ActiveItemButton[3];
    [SerializeField] ItemManager[] _noRandomActiveItems = new ItemManager[3];
    

    private void Start()
    {
        AddingActiveitems();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddingActiveitems();
        }
    }

    public void AddingActiveitems()
    {
        if (isRandom)
        {
            int randomItem;

            for (int i = 0; i < _activeItemsList.Length; i++)
            {
                bool copied = false;
                do
                {
                    copied = false;
                    randomItem = Random.Range(0, _itemsList.Count);
                    foreach (DonatItems item in _activeItemsList)
                    {
                        if (_itemsList[randomItem].item == item.item)
                        {
                            copied = true;
                        }
                    }
                } while (copied);
                _activeItemsList[i] = _itemsList[randomItem];

            }
        }
        else
        {
            for(int i = 0; i < _noRandomActiveItems.Length; i++)
            {
                for(int j = 0; j < _itemsList.Count; j++)
                {
                    if(_noRandomActiveItems[i] == _itemsList[j].item)
                    {
                        _activeItemsList[i] = _itemsList[j];
                    }
                }
            }
        }
        
        for (int i = 0; i < _activeItemButtons.Length; i++)
        {
            _activeItemButtons[i].activeButtonField.SetActive(true);
            _activeItemButtons[i].name.text = _activeItemsList[i].item.itemName;
            _activeItemButtons[i].decs.text = _activeItemsList[i].item.itemDescription;
            _activeItemButtons[i].activeItemImage.sprite = _activeItemsList[i].item.itemImage;
            _activeItemButtons[i].textOnButton.text = "buy: " + _activeItemsList[i].priceInGems;
        }

    }


    public void Buyitem(int itemCount)
    {
        GameManager.instance.currentGems -= _activeItemsList[itemCount].priceInGems;
        Inventory.instance.AddItems(_activeItemsList[itemCount].item);
        _activeItemButtons[itemCount].activeButtonField.SetActive(false);
        ShopManager.instance.UpdateCurrentGems(GameManager.instance.currentGems);
    }

}
