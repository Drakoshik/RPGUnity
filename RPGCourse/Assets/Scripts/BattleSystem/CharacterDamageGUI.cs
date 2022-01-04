using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterDamageGUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] float lifeTime = 1f, moveSpeed = 3f, textVibration = 0.5f;

    void Update()
    {
        Destroy(gameObject, lifeTime);
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }

    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
        float jitterAmount = Random.Range(-textVibration, +textVibration);
        transform.position += new Vector3(jitterAmount, jitterAmount, 0f);
    }
}
