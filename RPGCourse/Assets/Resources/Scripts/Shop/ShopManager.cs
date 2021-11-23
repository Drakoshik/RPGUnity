using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject shopMenu, buyPanel, sellPanel;

    [SerializeField] TextMeshProUGUI currentCurrencyText;

    public List<ItemManager> itemForSale;


    [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotSellContainerParent, itemSlotBuyContainerParent;

    [SerializeField] ItemManager selectedItem;
    [SerializeField] TextMeshProUGUI buyItemName, buyItemDesc, buyItemValue;
    [SerializeField] TextMeshProUGUI sellItemName, sellItemDesc, sellItemValue;



    private void Start()
    {
        instance = this;
    }


    public void OpenShopMenu()
    {
        shopMenu.SetActive(true);
        GameManager.instance.shopOpened = true;

        OpenBuyPannel();

        currentCurrencyText.text = "Curr:" + GameManager.instance.currentCurrency;
    }

    public void CloseShopMenu()
    {
        shopMenu.SetActive(false);
        GameManager.instance.shopOpened = false;
    }

    public void OpenBuyPannel()
    {
        buyPanel.SetActive(true);
        sellPanel.SetActive(false);

        UpdateItemsInShop(itemSlotBuyContainerParent, itemForSale);
    }

    public void OpenSellPannel()
    {
        buyPanel.SetActive(false);
        sellPanel.SetActive(true);

        UpdateItemsInShop(itemSlotSellContainerParent, Inventory.instance.GetItemsList());
    }



    public void UpdateItemsInShop(Transform itemSlotContainerParent, List<ItemManager> itemsTolookThrough)
    {
        foreach (Transform itemSlot in itemSlotContainerParent)
        {
            Destroy(itemSlot.gameObject);
        }
        foreach (ItemManager item in itemsTolookThrough)
        {
            RectTransform itemSlot = Instantiate(itemSlotContainer, itemSlotContainerParent).GetComponent<RectTransform>();

            Image itemImage = itemSlot.Find("Image").GetComponent<Image>();
            itemImage.sprite = item.itemImage;

            RectTransform rt = itemImage.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(95, 95);

            TextMeshProUGUI itemsAmountText = itemSlot.Find("AmountText").GetComponent<TextMeshProUGUI>();

            if (item.amount > 1)
                itemsAmountText.text = item.amount.ToString();
            else
                itemsAmountText.text = "";


            itemSlot.GetComponent<ItemButton>().itemOnButton = item;
            itemSlot.GetComponent<Button>().interactable = true;
        }
    }

    public void SelectedBuyItem(ItemManager itemToBuy)
    {
        selectedItem = itemToBuy;
        buyItemDesc.text = selectedItem.itemDescription;
        buyItemName.text = selectedItem.itemName;
        buyItemValue.text = "Value: " + selectedItem.valueInCoins;
    }

    public void SelectedSellItem(ItemManager itemToSell)
    {
        selectedItem = itemToSell;
        sellItemDesc.text = selectedItem.itemDescription;
        sellItemName.text = selectedItem.itemName;
        sellItemValue.text = "Value: " + (int)(selectedItem.valueInCoins * 0.75f);
    }

    public void BuyItem()
    {
        if(GameManager.instance.currentCurrency >= selectedItem.valueInCoins)
        {
            GameManager.instance.currentCurrency -= selectedItem.valueInCoins;
            Inventory.instance.AddItems(selectedItem);


            currentCurrencyText.text = "Curr: " + GameManager.instance.currentCurrency;
        }
    }

    public void SellItem()
    {
        if (selectedItem)
        {
            GameManager.instance.currentCurrency += (int)(selectedItem.valueInCoins * 0.75);
            Inventory.instance.RemoveItem(selectedItem);
            selectedItem = null;
            currentCurrencyText.text = "Curr: " + GameManager.instance.currentCurrency;
            UpdateItemsInShop(itemSlotSellContainerParent, Inventory.instance.GetItemsList());
        }
    }
}
