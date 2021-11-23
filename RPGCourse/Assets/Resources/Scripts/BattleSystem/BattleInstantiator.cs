using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInstantiator : MonoBehaviour
{
    [SerializeField] BattleTypeManager[] avaliableBattles;

    [SerializeField] bool activateOnEnter;
    private bool inArea = false;

    [SerializeField] float timeBetweenBattles;
    private float battleCounter;

    [SerializeField] bool deactivateAfterStart;

    [SerializeField] bool canRunAway;

    [SerializeField] bool shouldCompleteThequest;
    public string questToComplete;

    private void Start()
    {
        battleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
    }

    private void Update()
    {
        
        if (inArea && !PlayerController.instance.stopMove)
        {
            
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                battleCounter -= Time.deltaTime;
            }
        }
        if(battleCounter <= 0)
        {
            battleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
            StartCoroutine(StartBattleCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(battleCounter);
        if (collision.CompareTag("Player"))
        {
            if (activateOnEnter)
            {
                StartCoroutine(StartBattleCoroutine());
            }
            else
            {
                inArea = true;
            }
        }
        
    }

    public IEnumerator StartBattleCoroutine()
    {
        MenuManager.instance.FadeImage();
        GameManager.instance.isBattleStart = true;
        int selectBattle;
        if (avaliableBattles.Length == 1)
        {
            selectBattle = 0;
        }
        else
        {
            selectBattle = Random.Range(0, avaliableBattles.Length);
        }

        BattleManager.instance.itemsReward = avaliableBattles[selectBattle].rewardItems;
        BattleManager.instance.xpRewardAmount = avaliableBattles[selectBattle].rewardXP;

        BattleRewardHandler.instance.markQuestComplete = shouldCompleteThequest;
        BattleRewardHandler.instance.questToComplete = questToComplete;

        yield return new WaitForSeconds(1f);
        MenuManager.instance.Fadeout();

        BattleManager.instance.StartBattle(avaliableBattles[selectBattle].enemies, canRunAway);

        if (deactivateAfterStart)
        {
            Destroy(gameObject);
        }
        inArea = false;
    }

}
