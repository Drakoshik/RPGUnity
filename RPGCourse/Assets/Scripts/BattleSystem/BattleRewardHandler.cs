using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleRewardHandler : MonoBehaviour
{
    public static BattleRewardHandler instance;

    [SerializeField] TextMeshProUGUI xpText, itemsText;
    [SerializeField] GameObject rewardScreen;

    [SerializeField] ItemManager[] rewardItems;
    [SerializeField] int xpReward;

    [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotContainerParent;

    public bool markQuestComplete;
    public string questToComplete;


    private void Start()
    {
        instance = this;
    }



    public void OpenRewardScreen(int xpErned, ItemManager[] itemsErned)
    {

        GameManager.instance.isBattleStart = true;

        rewardScreen.SetActive(true);
        xpReward = xpErned;
        rewardItems = itemsErned;
        xpText.text = xpErned + "XP";
        itemsText.text = "";

        foreach (Transform itemSlot in itemSlotContainerParent)
        {
            Destroy(itemSlot.gameObject);
        }
        foreach (ItemManager item in rewardItems)
        {
            RectTransform itemSlot = Instantiate(itemSlotContainer, itemSlotContainerParent).GetComponent<RectTransform>();

            Image itemImage = itemSlot.Find("Image").GetComponent<Image>();
            itemImage.sprite = item.itemImage;

            RectTransform rt = itemImage.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(95, 95);

            TextMeshProUGUI itemsAmountText = itemSlot.Find("AmountText").GetComponent<TextMeshProUGUI>();

            itemsAmountText.text = "";


            itemSlot.GetComponent<ItemButton>().itemOnButton = item;
            itemSlot.GetComponent<Button>().interactable = false;
        }
    }

    public void CloseButton()
    {

        foreach (PlayerStats activePlayer in GameManager.instance.GetPlayerStats())
        {
            if (activePlayer.gameObject.activeInHierarchy)
            {
                activePlayer.AddXP(xpReward);
            }
        }

        foreach(ItemManager ItemRewarded in rewardItems)
        {
            Inventory.instance.AddItems(ItemRewarded);
        }

        rewardScreen.SetActive(false);

        GameManager.instance.isBattleStart = false;

        if (markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToComplete);
        }
    }

}
