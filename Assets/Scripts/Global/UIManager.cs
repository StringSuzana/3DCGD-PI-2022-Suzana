using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Singleton

    public static UIManager shared { get; private set; }

    [SerializeField]
    private TMP_Text weaponName;

    void Awake()
    {
        shared = this;
    }

    public void SetWeaponName(string name)
    {
        weaponName.text = name;
    }
}