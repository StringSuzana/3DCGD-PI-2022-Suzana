using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    IEnumerator TakeDamage(float damageAmount);
    void Heal(int healAmount);
    public void GrabVaccineBag();
}