using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public interface IFpsPlayer
    {
        IEnumerator TakeDamage(float damageAmount);
        void Heal(int healAmount);
    }

    public interface ITpsPlayer
    {
        public void GrabVaccineBag(BagObject bag);
    }
}