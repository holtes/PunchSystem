using UnityEngine;

namespace _Project.Develop.Runtime.Domain.Models
{
    public class PlayerModel
    {
        private GameObject _heldWeaponLeft;
        private GameObject _heldWeaponRight;

        public bool TryGrabWeapon(GameObject weapon, bool isRight = true)
        {
            if (!weapon.CompareTag("Weapon")) return false;

            if (isRight) _heldWeaponRight = weapon;
            else _heldWeaponLeft = weapon;
            return true;
        }

        public void DropWeapon(bool isRight = true)
        {
            if (isRight) _heldWeaponRight = null;
            else _heldWeaponLeft = null;
        }

        public void DropAllWeapons()
        {
            _heldWeaponLeft = null;
            _heldWeaponRight = null;
        }

        public GameObject GetHeldWeapon(bool isRight = true)
        {
            if (isRight) return _heldWeaponRight;
            else return _heldWeaponLeft;
        }
    }
}