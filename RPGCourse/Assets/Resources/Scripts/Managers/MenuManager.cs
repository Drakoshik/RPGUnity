using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Image imageToFade;
    public GameObject menu, statPanel, inventoryPanel;


    [SerializeField] GameObject[] statsButtons;

    public static MenuManager instance;

    private PlayerStats[] playerStats;

    [SerializeField] TextMeshProUGUI[] nameText, hpText, manaText, currentXpText, xpText;
    [SerializeField] Slider[] xpSlider;
    [SerializeField] Image[] characterImage;
    [SerializeField] GameObject[] characterPanel;


    [SerializeField] TextMeshProUGUI statName, statHP, statMana, statDex, statDef, statEquipedWeapon, statEquipedArmor;
    [SerializeField] TextMeshProUGUI statEquipedWeaponPower, statEquipedArmorDefence;
    [SerializeField] Image characterStatImage;


    [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotContainerParent;

    public TextMeshProUGUI itemName, itemDescription;


    public ItemManager activeItem;

    [SerializeField] GameObject characterChoisePanel;
    [SerializeField] TextMeshProUGUI[] itemCharacterChoiceNames;


    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.shopOpened == false)
        {
            if (menu.activeInHierarchy)
            {
                menu.SetActive(false);
                GameManager.instance.gameMenuOpened = false;
                CloseCharacterChoisePanel();
                statPanel.SetActive(false);
                inventoryPanel.SetActive(false);
                
            }
            else
            {
                UpdateStats();
                menu.SetActive(true);
                GameManager.instance.gameMenuOpened = true;
            }
        }
        
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        GameManager.instance.gameMenuOpened = false;
    }



    public void UpdateStats()
    {
        playerStats = GameManager.instance.GetPlayerStats();
        for (int i = 0; i < playerStats.Length; i++)
        {
            characterPanel[i].SetActive(true);


            nameText[i].text = playerStats[i].playerName;
            hpText[i].text = "HP: " + playerStats[i].currentHp + "/" + playerStats[i].maxHP;
            manaText[i].text = "Mana: " + playerStats[i].currentMana + "/" + playerStats[i].maxMana;
            currentXpText[i].text = "XP: " + playerStats[i].currentXp;

            characterImage[i].sprite = playerStats[i].CharacterImage;
            characterImage[i].SetNativeSize();
            characterImage[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            xpText[i].text = playerStats[i].currentXp.ToString() + "/" + playerStats[i].xpForNextLevel[playerStats[i].playerLevel];
            xpSlider[i].maxValue = playerStats[i].xpForNextLevel[playerStats[i].playerLevel];
            xpSlider[i].value = playerStats[i].currentXp;
        }
    }


    public void StatsMenu()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            statsButtons[i].SetActive(true);

            statsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerStats[i].playerName;
        }

        StatsMenuUpdate(0);
    }


    public void StatsMenuUpdate(int playerSelectedNumber)
    {
        PlayerStats playerSelected = playerStats[playerSelectedNumber];

        statName.text = playerSelected.playerName;
        statHP.text = playerSelected.currentHp.ToString() + "/" + playerSelected.maxHP;
        statMana.text = playerSelected.currentMana.ToString() + "/" + playerSelected.maxMana;

        statDex.text = playerSelected.dexterity.ToString();
        statDef.text = playerSelected.defence.ToString();

        characterStatImage.sprite = playerSelected.CharacterImage;
        characterStatImage.SetNativeSize();
        if (!playerSelected.CompareTag("Player"))
            characterStatImage.transform.localScale = new Vector3(2f, 2f, 2f);
        else characterStatImage.transform.localScale = new Vector3(1f, 1f, 1f);


        statEquipedArmor.text = playerSelected.equipedArmorName;
        statEquipedWeapon.text = playerSelected.equipedWeaponName;
        statEquipedWeaponPower.text = playerSelected.weaponPower.ToString();
        statEquipedArmorDefence.text = playerSelected.armorDefence.ToString();
    }

    public void UpdateItemsInventory()
    {
        foreach (Transform itemSlot in itemSlotContainerParent)
        {
            Destroy(itemSlot.gameObject);
        }
        foreach (ItemManager item in Inventory.instance.GetItemsList())
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


    public void Discard()
    {
        if (activeItem)
        {
            Inventory.instance.RemoveItem(activeItem);
            UpdateItemsInventory();
            AudioManager.instance.PlaySFX(3);
        }
    }


    public void UseItem(int selectedCharacter)
    {
        activeItem.UseItem(selectedCharacter);
        OpenCharacterChoisePanel();
        Inventory.instance.RemoveItem(activeItem);
        UpdateItemsInventory();
        AudioManager.instance.PlaySFX(8);
    }


    public void OpenCharacterChoisePanel()
    {
        if(activeItem != null)
        {
            characterChoisePanel.SetActive(true);

            if (activeItem)
            {
                for (int i = 0; i < playerStats.Length; i++)
                {
                    PlayerStats activePlayer = GameManager.instance.GetPlayerStats()[i];
                    itemCharacterChoiceNames[i].text = activePlayer.playerName;

                    bool ActivePlayerAvailable = activePlayer.gameObject.activeInHierarchy;
                    itemCharacterChoiceNames[i].transform.parent.gameObject.SetActive(ActivePlayerAvailable);
                }
            }        

        }
    }

    public void CloseCharacterChoisePanel()
    {
        characterChoisePanel.SetActive(false);
    }


    public void QuitGame()
    {
        Application.Quit(); 
    }

    public void FadeImage()
    {
        imageToFade.GetComponent<Animator>().SetTrigger("StartFading");
    }

    public void Fadeout()
    {
        imageToFade.GetComponent<Animator>().SetTrigger("EndFading");
    }

}
