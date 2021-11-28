using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    private bool isBattleStart;

    [SerializeField] GameObject battleScene;
    [SerializeField] List<BattleCharacters> activeCharacters = new List<BattleCharacters>();

    [SerializeField] Transform[] playersPositions, enemiesPositions;


    [SerializeField] BattleCharacters[] playerPrefabs, enemiesPrefabs;

    [SerializeField] int currentTurn;
    [SerializeField] bool waitingForTurn;
    [SerializeField] Button[] battleButtons;

    [SerializeField] BattleMoves[] battleMovesList;

    [SerializeField] ParticleSystem characterAttackeffect;

    [SerializeField] CharacterDamageGUI damageText;

    [SerializeField] GameObject[] playersBattleStats;

    [SerializeField] TextMeshProUGUI[] playersNameText;
    [SerializeField] Slider[] playerHealthSlider, playerManaSlider;

    [SerializeField] GameObject enemyTargetPanel;
    [SerializeField] BattleTargetButtons[] targetButtons;

    public GameObject spellChoisePanel;
    [SerializeField] SpellButton[] spellButtons;

    public BattleNotifications battleNotice;

    [SerializeField] float chanceToRunAway = 0.5f;


    public GameObject itemToUseMenu;
    [SerializeField] ItemManager selectedItem;
    [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotContainerParent;
    [SerializeField] TextMeshProUGUI itemName, itemDesc;

    [SerializeField] GameObject characterChoisePanel;
    [SerializeField] TextMeshProUGUI[] playerNames;

    [SerializeField] string gameOverScene;
    private bool runningAway;
    public int xpRewardAmount;
    public ItemManager[] itemsReward;

    private bool canRun;

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
        CheckPlayersButtons();

        
    }

    private void CheckPlayersButtons()
    {
        if (isBattleStart)
        {
            if (waitingForTurn)
            {
                if (activeCharacters[currentTurn].IsPlayer())
                {
                    foreach (Button buttons in battleButtons)
                    {
                        buttons.interactable = true;
                    }
                }
                else
                {
                    foreach (Button buttons in battleButtons)
                    {
                        buttons.interactable = false;
                    }
                    
                    StartCoroutine(EnemyAttackCoroutine());
                    
                }
            }
        }
    }

    

    private void UpdateBattle()
    {
        bool allEnemiesAreDead = true;
        bool allPlayersAreDead = true;

        for(int i = 0; i< activeCharacters.Count; i++)
        {
            if(activeCharacters[i].currentHp < 0)
                activeCharacters[i].currentHp = 0;

            if (activeCharacters[i].currentHp == 0)
            {
                if(activeCharacters[i].IsPlayer() && !activeCharacters[i].isDead)
                {
                    activeCharacters[i].KillPlayer();
                }

                if (!activeCharacters[i].IsPlayer() && !activeCharacters[i].isDead)
                {
                    activeCharacters[i].KillEnemy();
                }
            }
            else
            {
                if (activeCharacters[i].IsPlayer())
                    allPlayersAreDead = false;
                else
                    allEnemiesAreDead = false;
            }
        }

        if(allEnemiesAreDead || allPlayersAreDead)
        {
            if (allEnemiesAreDead)
                StartCoroutine(EndBattleCoroutine());
            else if (allPlayersAreDead)
                StartCoroutine(GameOverCoroutine());

        }
        else
        {
            while(activeCharacters[currentTurn].currentHp == 0)
            {
                currentTurn++;
                if(currentTurn >= activeCharacters.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    

    public void StartBattle(string[] enemiesToSpawn, bool canRunAway)
    {
        if (!isBattleStart)
        {
            canRun = canRunAway;
            SettingupBattle();
            AddingPLayers();
            AddingEnemies(enemiesToSpawn);
        }

        waitingForTurn = true;
        currentTurn = Random.Range(0, activeCharacters.Count);
        if (activeCharacters[currentTurn].IsPlayer())
        {
            UpdateCharacterPosition(-1, currentTurn);
        }
        UpdatePlayerStats();
    }

    private void AddingEnemies(string[] enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {

            if (enemiesToSpawn[i] != null)
            {
                for (int j = 0; j < enemiesPrefabs.Length; j++)
                {
                    if (enemiesPrefabs[j].characterName == enemiesToSpawn[i])
                    {
                        BattleCharacters newEnemy = Instantiate(
                            enemiesPrefabs[j],
                            enemiesPositions[i].position,
                            enemiesPositions[i].rotation,
                            enemiesPositions[i]
                            );

                        activeCharacters.Add(newEnemy);
                    }
                }
            }
        }
    }

    private void AddingPLayers()
    {
        for (int i = 0; i < GameManager.instance.GetPlayerStats().Length; i++)
        {
            if (GameManager.instance.GetPlayerStats()[i].gameObject.activeInHierarchy)
            {
                for (int k = 0; k < playerPrefabs.Length; k++)
                {
                    if (playerPrefabs[k].characterName == GameManager.instance.GetPlayerStats()[i].playerName)
                    {
                        BattleCharacters newPlayer = Instantiate(
                            playerPrefabs[k],
                            playersPositions[i].position,
                            playersPositions[i].rotation,
                            playersPositions[i]
                            );

                        activeCharacters.Add(newPlayer);

                        ImportPlayerStats(i);
                    }
                }
            }
        }
    }


    private void ImportPlayerStats(int i)
    {
        PlayerStats player = GameManager.instance.GetPlayerStats()[i];
        activeCharacters[i].currentHp = player.currentHp;
        activeCharacters[i].maxHP = player.currentHp;

        activeCharacters[i].maxMana = player.maxMana;
        activeCharacters[i].currentMana = player.currentMana;

        activeCharacters[i].dexterity = player.dexterity;
        activeCharacters[i].defence = player.defence;

        activeCharacters[i].weaponPower = player.weaponPower;
        activeCharacters[i].armorDefence = player.armorDefence;
    }

    private void SettingupBattle()
    {
        isBattleStart = true;
        GameManager.instance.isBattleStart = true;

        transform.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            transform.position.z
            );

        battleScene.SetActive(true);
    }

    private void NextTurn()
    {
        if (activeCharacters[currentTurn].IsPlayer())
        {
            UpdateCharacterPosition(1, currentTurn);
        }
        currentTurn++;
        if (currentTurn >= activeCharacters.Count)
        {
            currentTurn = 0;
        }

        waitingForTurn = true;


        UpdateBattle();
        UpdatePlayerStats();
        //при смене айдишника на активного иргрока сдвинуть его, после смены на след адишник вернуть назад.
        if (activeCharacters[currentTurn].IsPlayer())
        {
            UpdateCharacterPosition(-1, currentTurn);
        }

    }

    private void UpdateCharacterPosition(int tranformPosition, int character)
    {
        activeCharacters[character].transform.position = new Vector3(
                    activeCharacters[character].transform.position.x + tranformPosition,
                    activeCharacters[character].transform.position.y,
                    activeCharacters[character].transform.position.z
                    );


    }

    public IEnumerator EnemyAttackCoroutine()
    {
        UpdateCharacterPosition(1, currentTurn);
        waitingForTurn = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        UpdateCharacterPosition(-1, currentTurn);
        NextTurn();
    }

    private void EnemyAttack()
    {


        List<int> players = new List<int>();

        for (int i = 0; i < activeCharacters.Count; i++)
        {
            if (activeCharacters[i].IsPlayer() && activeCharacters[i].currentHp > 0)
            {
                players.Add(i);
            }
        }

        int selectedPlayerToAttack = players[Random.Range(0, players.Count)];
        int movePower = 0;

        int selectedAttack = Random.Range(0, activeCharacters[currentTurn].AttackMovesAvaliable().Length);

        for (int i = 0; i < battleMovesList.Length; i++)
        {
            if (battleMovesList[i].moveName == activeCharacters[currentTurn].AttackMovesAvaliable()[selectedAttack])
            {
                movePower = GettingMovePowerAndEffectInst(selectedPlayerToAttack,i);
            }
        }


        //Instantiate(
        //    characterAttackeffect,
        //    activeCharacters[currentTurn].transform.position,
        //    activeCharacters[currentTurn].transform.rotation
        //    );

        DealdamageToCharacters(selectedPlayerToAttack, movePower);


        UpdatePlayerStats();
    }

    private IEnumerator ChangePositionOnAttack(int tranformPosition)
    {
        activeCharacters[currentTurn].transform.position = new Vector3(
                    activeCharacters[currentTurn].transform.position.x  + tranformPosition,
                    activeCharacters[currentTurn].transform.position.y,
                    activeCharacters[currentTurn].transform.position.z
                    );
        yield return new WaitForSeconds(0.1f);
    }

    private void DealdamageToCharacters(int selectedCharacterToAttack, int movePower)
    {
        float attackPower = activeCharacters[currentTurn].dexterity + activeCharacters[currentTurn].weaponPower;
        float defenceAmount = activeCharacters[selectedCharacterToAttack].defence + activeCharacters[selectedCharacterToAttack].armorDefence;

        float damageAmount = (attackPower / defenceAmount) * movePower * Random.Range(0.7f, 1.3f);
        int damageToGive = (int)damageAmount;

        damageToGive = CalculateCritical(damageToGive);

        Debug.Log(activeCharacters[currentTurn].name + "_" + damageAmount + "_" + damageToGive
            + "_" + activeCharacters[selectedCharacterToAttack].characterName);

        activeCharacters[selectedCharacterToAttack].TakeHpDamage(damageToGive);

        CharacterDamageGUI characterDamageText = Instantiate(
            damageText,
            activeCharacters[selectedCharacterToAttack].transform.position,
            activeCharacters[selectedCharacterToAttack].transform.rotation
            );

        characterDamageText.SetDamage(damageToGive);

    }

    private int CalculateCritical(int damageToGive)
    {
        if (Random.value <= 0.3f)
        {
            return (damageToGive * 2);
        }
        else
            return damageToGive;
    }


    public void UpdatePlayerStats()
    {
        
        for(int i = 0; i < playersNameText.Length; i++)
        {
            if(activeCharacters.Count > i)
            {
                if (activeCharacters[i].IsPlayer())
                {
                    BattleCharacters playerData = activeCharacters[i];

                    playersNameText[i].text = playerData.characterName;

                    playerHealthSlider[i].maxValue = playerData.maxHP;
                    playerHealthSlider[i].value = playerData.currentHp;

                    playerManaSlider[i].maxValue = playerData.maxMana;
                    playerManaSlider[i].value = playerData.currentMana;

                    if(playerManaSlider[i].value == 0)
                        playerManaSlider[i].fillRect.gameObject.SetActive(false);
                    if (playerHealthSlider[i].value == 0)
                        playerHealthSlider[i].fillRect.gameObject.SetActive(false);
                }
                else
                {
                    playersBattleStats[i].SetActive(false);
                }
            }
            else
            {
                playersBattleStats[i].SetActive(false);
            }
        }
    }

    //Player Attacking Methods

    private IEnumerator DoPlayerAttack()
    {


        yield return new WaitForSeconds(1f);
    }

    public void PlayerAttack(string moveName, int selectEnemytarget)
    {
        //вызвать корутину здвинуть игрока -1
        int movePower = 0;

        for(int i = 0; i < battleMovesList.Length; i++)
        {
            if(battleMovesList[i].moveName == moveName)
            {
                movePower = GettingMovePowerAndEffectInst(selectEnemytarget, i);
            }
        }


        DealdamageToCharacters(selectEnemytarget, movePower);

        waitingForTurn = false;

        //Instantiate(
        //    characterAttackeffect,
        //    activeCharacters[currentTurn].transform.position,
        //    activeCharacters[currentTurn].transform.rotation
        //    );
        NextTurn();
        enemyTargetPanel.SetActive(false);
    }


    public void OpenTargetMenu(string MoveName)
    {
        enemyTargetPanel.SetActive(true);

        List<int> Enemies = new List<int>();
        for(int i = 0; i < activeCharacters.Count; i++)
        {
            if (!activeCharacters[i].IsPlayer())
            {
                Enemies.Add(i);
            }
        }

        for(int i = 0; i < targetButtons.Length; i++)
        {
            if(Enemies.Count > i && activeCharacters[Enemies[i]].currentHp > 0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = MoveName;
                targetButtons[i].activeBattletarget = Enemies[i];
                targetButtons[i].targetName.text = activeCharacters[Enemies[i]].characterName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }


    public void OpenSpellPanel()
    {
        spellChoisePanel.SetActive(true);

        for(int i = 0; i < spellButtons.Length; i++)
        {
            if(activeCharacters[currentTurn].AttackMovesAvaliable().Length > i)
            {
                spellButtons[i].gameObject.SetActive(true);
                spellButtons[i].spellName = GetCurrantActiveCharacter().AttackMovesAvaliable()[i];
                spellButtons[i].spellNameText.text = spellButtons[i].spellName;

                for(int j = 0; j < battleMovesList.Length; j++)
                {
                    if(battleMovesList[j].moveName == spellButtons[i].spellName)
                    {
                        spellButtons[i].spellCost = battleMovesList[i].manaCost;
                        spellButtons[i].spellCostText.text = spellButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                spellButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public BattleCharacters GetCurrantActiveCharacter()
    {
        return activeCharacters[currentTurn];
    }

    private int GettingMovePowerAndEffectInst(int characterTarget, int i)
    {
        int movePower;
        Instantiate(
               battleMovesList[i].theEffecttoUse,
               activeCharacters[characterTarget].transform.position,
               activeCharacters[characterTarget].transform.rotation
               );

        movePower = battleMovesList[i].movePower;
        return movePower;
    }
    public void Runaway()
    {
        if (canRun)
        {
            if (Random.value > chanceToRunAway)
            {
                runningAway = true;
                StartCoroutine(EndBattleCoroutine());
            }
            else
            {
                NextTurn();
                battleNotice.SetText("NOPE");
                battleNotice.Activate();
            }
        }
        else
        {
            NextTurn();
            battleNotice.SetText("NOPE");
            battleNotice.Activate();
        }
    }


    public void UpdateItemsOnInventory()
    {
        itemToUseMenu.SetActive(true);

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

    public void SelectedItemToUSe(ItemManager itemToUse)
    {
        selectedItem = itemToUse;
        itemName.text = itemToUse.itemName;
        itemDesc.text = itemToUse.itemDescription;
    }

    public void OpencharacterMenu()
    {
        if (selectedItem)
        {
            characterChoisePanel.SetActive(true);
            for(int i = 0; i < activeCharacters.Count; i++)
            {
                if (activeCharacters[i].IsPlayer())
                {
                    PlayerStats activePlayer = GameManager.instance.GetPlayerStats()[i];
                    playerNames[i].text = activePlayer.playerName;

                    bool activePlayerInHierarchy = activePlayer.gameObject.activeInHierarchy;
                    playerNames[i].transform.parent.gameObject.SetActive(activePlayerInHierarchy);
                }
            }
        }
        else
        {
            print("dfhbaghERgd");
        }
    }


    public void UseItemButton(int selectedPlayer)
    {
        activeCharacters[selectedPlayer].UseItemInBattle(selectedItem);
        Inventory.instance.RemoveItem(selectedItem);

        UpdatePlayerStats();
        CloseCharacterChoiseMenu();
        UpdateItemsOnInventory();

    }

    public void CloseCharacterChoiseMenu()
    {
        characterChoisePanel.SetActive(false);
        itemToUseMenu.SetActive(false);
    }


    public IEnumerator EndBattleCoroutine()
    {
        isBattleStart = false;
        foreach (Button buttons in battleButtons)
        {
            buttons.interactable = false;
        }
        enemyTargetPanel.SetActive(false);
        spellChoisePanel.SetActive(false);

        if (!runningAway)
        {
            battleNotice.SetText("WE WON!");
            battleNotice.Activate();
        }
        
        yield return new WaitForSeconds(2f);
        foreach(BattleCharacters playersInBattle in activeCharacters)
        {
            if (playersInBattle.IsPlayer())
            {
                foreach(PlayerStats playerStats in GameManager.instance.GetPlayerStats())
                {
                    if(playersInBattle.characterName == playerStats.playerName)
                    {
                        playerStats.currentHp = playersInBattle.currentHp;
                        playerStats.currentMana = playersInBattle.currentMana;
                    }
                }
            }

            Destroy(playersInBattle.gameObject);
        }


        battleScene.SetActive(false);
        activeCharacters.Clear();

        if (runningAway)
        {
            currentTurn = 0;
            GameManager.instance.isBattleStart = false;
        }
        else
        {

            Debug.Log(BattleRewardHandler.instance);
            BattleRewardHandler.instance.OpenRewardScreen(xpRewardAmount, itemsReward);
        }

        

    }

    public IEnumerator GameOverCoroutine()
    {
        battleNotice.SetText("YOU LOOSE");
        battleNotice.Activate();

        yield return new WaitForSeconds(2f);


        StartCoroutine(EndBattleCoroutine());
        SceneManager.LoadScene(gameOverScene);
    }
}
