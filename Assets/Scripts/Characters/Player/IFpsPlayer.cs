using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public interface IFpsPlayer : IPlayer
    {
        bool TakeDamage(float damageAmount);
        void Heal(int healAmount);
    }

    public interface ITpsPlayer : IPlayer
    {
        public void GrabVaccineBag(GameObject itemGameObject, ItemObject bag);
    }

    public interface IPlayer
    {
    }
}