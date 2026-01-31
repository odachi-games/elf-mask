using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : Singleton<WeaponManager>
{


    public void EquipWeapon(Weapon weapon)
    {
        GameManager.Instance.Player.PlayerAttack.EquipWeapon(weapon);
    }
}