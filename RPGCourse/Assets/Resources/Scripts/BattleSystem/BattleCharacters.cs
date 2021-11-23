using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacters : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] string[] attacksAvailable;

    public string characterName;
    public int currentHp, maxHP, currentMana, maxMana, dexterity, defence, weaponPower, armorDefence;
    public bool isDead;

    public SpriteRenderer deadSprite;
    public ParticleSystem deadParticle;


    private void Update()
    {
        if(!IsPlayer() && isDead)
        {
            FadeOutEnemy();
        }
    }

    private void FadeOutEnemy()
    {
        GetComponent<SpriteRenderer>().color = new Color(
            Mathf.MoveTowards(GetComponent<SpriteRenderer>().color.r, 1f, 0.3f*Time.deltaTime),
            Mathf.MoveTowards(GetComponent<SpriteRenderer>().color.g, 0f, 0.3f*Time.deltaTime),
            Mathf.MoveTowards(GetComponent<SpriteRenderer>().color.b, 0f, 0.3f*Time.deltaTime),
            Mathf.MoveTowards(GetComponent<SpriteRenderer>().color.a, 0f, 0.3f*Time.deltaTime)
            );

        if(GetComponent<SpriteRenderer>().color.a == 0)
        {
            gameObject.SetActive(false);
        }
        
    }

    public void KillEnemy()
    {
        isDead = true;
    }


    public bool IsPlayer()
    {
        return isPlayer;
    }


    public string[] AttackMovesAvaliable()
    {
        return attacksAvailable;
    }


    public void TakeHpDamage(int damageTorecieve)
    {
        currentHp -= damageTorecieve;
        if (currentHp < 0)
            currentHp = 0;
    }


    public void UseItemInBattle(ItemManager itemToUse)
    {
        if(itemToUse.itemType == ItemManager.ItemType.Item)
        {
            if(itemToUse.affectType == ItemManager.AffectType.HP)
            {
                AddHP(itemToUse.AmountOfAffect);
            }
            else if (itemToUse.affectType == ItemManager.AffectType.Mana)
            {
                AddMana(itemToUse.AmountOfAffect);
            }
            else if(itemToUse.affectType == ItemManager.AffectType.MultiPotion)
            {
                AddHPAndMana(itemToUse.AmountOfAffect);
            }
        }
    }

    private void AddHPAndMana(int amountOfAffect)
    {
        currentHp += amountOfAffect;
        currentMana += amountOfAffect;
    }

    private void AddMana(int amountOfAffect)
    {
        currentMana += amountOfAffect;
    }

    private void AddHP(int amountOfAffect)
    {
        currentHp += amountOfAffect;
    }

    public void KillPlayer()
    {
        if(deadSprite){
            GetComponent<SpriteRenderer>().sprite = deadSprite.sprite;
            Instantiate(deadParticle, transform.position, transform.rotation);
            isDead = true;
        }
    }
}
