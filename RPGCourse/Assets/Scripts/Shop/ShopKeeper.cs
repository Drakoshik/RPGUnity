using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour, IOpenButton
{
    

    [SerializeField] List<ItemManager> itemsForSale;
    [SerializeField] string _name;
    public string Name { get; set; }

    private void Start()
    {
        _name = "Shop";
        Name = _name;
    }
    public void OpenWindow()
    {
        
        if (!ShopManager.instance.shopMenu.activeInHierarchy)
        {
            ShopManager.instance.itemForSale = itemsForSale;
            ShopManager.instance.OpenShopMenu();
            Name = "shop";
        }
    }


}
