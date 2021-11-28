using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] PlayerStats[] playerStats;

    public bool gameMenuOpened, DialogBoxOpened, shopOpened, isBattleStart;


    public int currentCurrency;

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


    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveData();
            QuestManager.instance.SaveQuestData();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (PlayerPrefs.HasKey("Player_Pos_x"))
            {
                LoadData();
            }
        }


        if (gameMenuOpened || DialogBoxOpened || shopOpened || isBattleStart)
        {
            PlayerController.instance.stopMove = true;
        }
        else
        {
            PlayerController.instance.stopMove = false;
        }
    }


    public PlayerStats[] GetPlayerStats()
    {
        playerStats = FindObjectsOfType<PlayerStats>();
        return playerStats;
    }

    public void SaveData()
    {
        SavingPlayerPosition();
        SavingPlayerStats();

        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);

        PlayerPrefs.SetInt("Number_Of_Items", Inventory.instance.GetItemsList().Count);
        for(int i = 0; i < Inventory.instance.GetItemsList().Count; i++)
        {
            ItemManager itemInInventory = Inventory.instance.GetItemsList()[i];
            PlayerPrefs.SetString("Item_" + i + "_Name", itemInInventory.itemName);
            print(itemInInventory);

            if (itemInInventory.isStakable)
            {
                PlayerPrefs.SetInt("Items_" + i + "_Amount", itemInInventory.amount);
            }
        }
    }

    private static void SavingPlayerPosition()
    {
        PlayerPrefs.SetFloat("Player_Pos_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Pos_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Pos_z", PlayerController.instance.transform.position.z);
    }

    private void SavingPlayerStats()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_active", 1);
            }
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_active", 0);
            }

            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_CurrentXP", playerStats[i].currentXp);

            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_MaxHp", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_CurrentHP", playerStats[i].currentHp);

            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_MaxMana", playerStats[i].maxMana);
            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_CurrentMana", playerStats[i].currentMana);

            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_Dexterity", playerStats[i].dexterity);
            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_Defence", playerStats[i].defence);

            PlayerPrefs.SetString("Player_" + playerStats[i].playerName + "_EquipedWeaponName", playerStats[i].equipedWeaponName);
            PlayerPrefs.SetString("Player_" + playerStats[i].playerName + "_equipedArmorName", playerStats[i].equipedArmorName);

            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_WeaponPowerP", playerStats[i].weaponPower);
            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_ArmorDefence", playerStats[i].armorDefence);

        }
    }

    IEnumerator LoadDataCoroutine()
    {

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));
        LoadingPlayerPosition();
        LoadingPlayerStats();
        LoadingInventoryData();
        QuestManager.instance.LoadQuestData();
    }

    public void LoadData()
    {
        MenuManager.instance.FadeImageIn();
        StartCoroutine(LoadDataCoroutine());
    }

    private static void LoadingInventoryData()
    {
        int itemListCount = Inventory.instance.GetItemsList().Count;

        for (int i = 0; i < itemListCount; i++)
        {
            ItemManager itemInInventory = Inventory.instance.GetItemsList()[0];
            if (itemInInventory.isStakable)
            {
                itemInInventory.amount = 1;
            }
            Inventory.instance.RemoveItem(itemInInventory);
        }

        for (int i = 0; i < PlayerPrefs.GetInt("Number_Of_Items"); i++)
        {
            string itemName = PlayerPrefs.GetString("Item_" + i + "_Name");
            ItemManager itemToAdd = ItemsAssets.instance.GetItemAsset(itemName);
            print(itemToAdd);
            int itemAmount = 0;
            if (PlayerPrefs.HasKey("Items_" + i + "_Amount"))
            {
                itemAmount = PlayerPrefs.GetInt("Items_" + i + "_Amount");
            }
            Inventory.instance.AddItems(itemToAdd);
            if (itemToAdd.isStakable && itemAmount > 1)
            {
                itemToAdd.amount = itemAmount;
            }

        }
    }

    private void LoadingPlayerStats()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(true);
            }
            else
            {
                playerStats[i].gameObject.SetActive(false);
            }


            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_Level");
            playerStats[i].currentXp = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_CurrentXP");

            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_MaxHp");
            playerStats[i].currentHp = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_CurrentHP");

            playerStats[i].maxMana = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_MaxMana");
            playerStats[i].currentMana = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_CurrentMana");

            playerStats[i].dexterity = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_Dexterity");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_Defence");

            playerStats[i].equipedWeaponName = PlayerPrefs.GetString("Player_" + playerStats[i].playerName + "_EquipedWeaponName");
            playerStats[i].equipedArmorName = PlayerPrefs.GetString("Player_" + playerStats[i].playerName + "_equipedArmorName");

            playerStats[i].weaponPower = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_WeaponPowerP");
            playerStats[i].armorDefence = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_ArmorDefence");

        }
    }

    private static void LoadingPlayerPosition()
    {
        PlayerController.instance.transform.position = new Vector3(
                    PlayerPrefs.GetFloat("Player_Pos_x"),
                    PlayerPrefs.GetFloat("Player_Pos_y"),
                    PlayerPrefs.GetFloat("Player_Pos_z")
                    );
    }
}
