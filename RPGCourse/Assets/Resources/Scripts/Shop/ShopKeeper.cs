using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    private bool canOpenedShop;

    [SerializeField] List<ItemManager> itemsForSale;

    private void Update()
    {
        if(canOpenedShop && Input.GetMouseButton(0) && !PlayerController.instance.stopMove 
            && !ShopManager.instance.shopMenu.activeInHierarchy)
        {
            ShopManager.instance.itemForSale = itemsForSale;
            ShopManager.instance.OpenShopMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canOpenedShop = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canOpenedShop = false;
        }
    }

}
