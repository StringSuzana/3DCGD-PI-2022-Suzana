using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public interface IFpsPlayer: IPlayer
    {
        IEnumerator TakeDamage(float damageAmount);
        void Heal(int healAmount);
    }

    public interface ITpsPlayer: IPlayer
    {
        public void GrabVaccineBag(BagObject bag);
    }

    public interface IPlayer
    {

    }
}