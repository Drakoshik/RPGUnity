using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    [SerializeField] float effectTime;
    [SerializeField] int SFXNumberToPlay;

    private void Start()
    {
        AudioManager.instance.PlaySFX(SFXNumberToPlay);
    }

    private void Update()
    {
        Destroy(gameObject, effectTime);
    }
}
