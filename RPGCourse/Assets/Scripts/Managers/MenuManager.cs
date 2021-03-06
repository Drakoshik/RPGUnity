using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Image imageToFade;
    public GameObject menu, statPanel, inventoryPanel, characterInfoPanel;


    [SerializeField] GameObject[] statsButtons;

    public static MenuManager instance;

    private PlayerStats[] playerStats;

    [SerializeField] TextMeshProUGUI[] nameText, hpText, manaText, currentXpText, xpText;
    [SerializeField] Slider[] xpSlider;
    [SerializeField] Image[] characterImage;
    [SerializeField] GameObject[] characterPanel;
    [SerializeField] GameObject menuButton;


    [SerializeField] TextMeshProUGUI statName, statHP, statMana, statDex, statDef, statEquipedWeapon, statEquipedArmor;
    [SerializeField] TextMeshProUGUI statEquipedWeaponPower, statEquipedArmorDefence;
    [SerializeField] Image characterStatImage;


    [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotContainerParent, itemSlotEquipedArmor, itemSlotEquipedWeapon;

    public TextMeshProUGUI itemName, itemDescription;


    public ItemManager activeItem;

    [SerializeField] GameObject characterChoisePanel;
    [SerializeField] TextMeshProUGUI[] itemCharacterChoiceNames;
    [SerializeField] int playerSelectedNumberForUnequpied;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateItemsInventory();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.instance.shopOpened == false && GameManager.instance.isBattleStart == false)
        {
            if (menu.activeInHierarchy)
            {
                CloseMenu();
                CloseCharacterChoisePanel();
                statPanel.SetActive(false);
                inventoryPanel.SetActive(false);
                
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void OpenMenu()
    {
        UpdateStats();
        menu.SetActive(true);
        characterInfoPanel.SetActive(true);
        GameManager.instance.gameMenuOpened = true;
        menuButton.SetActive(false);
    }
    public void CloseMenu()
    {
        menu.SetActive(false);
        GameManager.instance.gameMenuOpened = false;
        menuButton.SetActive(true);
    }
    public void SavePanel()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void OpenItemsPanel()
    {
        if (inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(false);
            statPanel.SetActive(false);
            characterInfoPanel.SetActive(true);
        }
        else
        {
            inventoryPanel.SetActive(true);
            statPanel.SetActive(false);
            characterInfoPanel.SetActive(false);
        }
    }

    public void OpenStatsPanel()
    {
        if (statPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(false);
            statPanel.SetActive(false);
            characterInfoPanel.SetActive(true);
        }
        else
        {
            inventoryPanel.SetActive(false);
            statPanel.SetActive(true);
            characterInfoPanel.SetActive(false);
        }
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
        playerSelectedNumberForUnequpied = playerSelectedNumber;
        statName.text = playerSelected.playerName;
        statHP.text = playerSelected.currentHp.ToString() + "/" + playerSelected.maxHP;
        statMana.text = playerSelected.currentMana.ToString() + "/" + playerSelected.maxMana;

        statDex.text = playerSelected.dexterity.ToString();
        statDef.text = playerSelected.defence.ToString();


        var equiped = playerSelected.equipedArmor;
        UpadteEquiped(equiped, itemSlotEquipedArmor);
        equiped = playerSelected.equipedWeapon;
        UpadteEquiped(equiped, itemSlotEquipedWeapon);

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

    private void UpadteEquiped(ItemManager equiped, Transform ItemSlotBox)
    {
        foreach (Transform itemSlot in ItemSlotBox)
        {
            Destroy(itemSlot.gameObject);
        }

        if (equiped != null)
        {
            RectTransform itemSlot = Instantiate(itemSlotContainer, ItemSlotBox).GetComponent<RectTransform>();

            Image itemImage = itemSlot.Find("Image").GetComponent<Image>();
            itemImage.sprite = equiped.itemImage;

            RectTransform rt = itemImage.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(95, 95);
            TextMeshProUGUI itemsAmountText = itemSlot.Find("AmountText").GetComponent<TextMeshProUGUI>();

            itemsAmountText.text = "";

            itemSlot.GetComponent<ItemButton>().itemOnButton = equiped;
        }
    }

    public void UnEquipButton()
    {
        Inventory.instance.AddItems(activeItem);
        PlayerStats playerSelected = playerStats[playerSelectedNumberForUnequpied];
        if (activeItem)
        {
            if (activeItem.GetComponentInChildren<Armor>())
            {
                playerSelected.equipedArmor = null;
                playerSelected.equipedArmorName = "";
                playerSelected.armorDefence = 0;
                foreach (Transform itemSlot in itemSlotEquipedArmor)
                {
                    Destroy(itemSlot.gameObject);
                }
            }
            else if (activeItem.GetComponentInChildren<Weapon>())
            {
                playerSelected.equipedWeapon = null;
                playerSelected.equipedWeaponName = "";
                playerSelected.weaponPower = 0;
                foreach (Transform itemSlot in itemSlotEquipedWeapon)
                {
                    Destroy(itemSlot.gameObject);
                }
            }
        }
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

            bool isStacks = item.GetComponentInChildren<StackableItems>();

            if (isStacks)
            {
                int itemStacksCount = item.GetComponentInChildren<StackableItems>().itemAmount;

                if (itemStacksCount > 1)
                    itemsAmountText.text = itemStacksCount.ToString();
                else
                    itemsAmountText.text = "";
            }
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
            //Instantiate(activeItem);
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
        activeItem = null;
    }


    public void QuitGame()
    {
        Application.Quit(); 
    }

    public void FadeImageIn()
    {
        imageToFade.GetComponent<Animator>().SetTrigger("StartFading");
    }

    public void FadeImageOut()
    {
        imageToFade.GetComponent<Animator>().SetTrigger("EndFading");
    }

}
