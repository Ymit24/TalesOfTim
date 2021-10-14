using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    private static ShopController Instance;

    public GameObject ShopRootPanel;
    public Transform RootItemList;
    public GameObject ShopItemEntryPrefab;

    public TMPro.TMP_Text WeaponTitle, WeaponInfo, ArmorTitle, ArmorInfo;
    public Image WeaponIcon, ArmorIcon;
    
    private void Awake()
    {
        Instance = this;
    }

    public static void OpenShop(ShopLocationArchetype shopData)
    {
        PlayerController.IsPaused = true;
        Instance.ShopRootPanel.SetActive(true);

        foreach (Transform itemEntry in Instance.RootItemList)
        {
            Destroy(itemEntry.gameObject);
        }

        foreach (ItemBase itemData in shopData.Items)
        {
            GameObject entry = Instantiate(Instance.ShopItemEntryPrefab);
            entry.transform.SetParent(Instance.RootItemList);
            ShopItemSetupController ctr = entry.GetComponent<ShopItemSetupController>();
            ctr.Setup(itemData);
            entry.GetComponentInChildren<Button>().onClick.AddListener(() => {
                OnClickBuy(itemData);
            });
        }

        RefreshGear();
    }

    private static void OnClickBuy(ItemBase item)
    {
        PlayerController.StateInfo info = PlayerController.Instance.Info;
        if (info.Gold >= item.Price)
        {
            info.Gold -= item.Price;

            if (item.isWeapon)
            {
                PlayerController.Instance.Weapon = item as WeaponArchetype;
                RefreshGear();
            }
            else if (!item.isWeapon)
            {
                PlayerController.Instance.EquipArmor(item as ArmorArchetype);
                RefreshGear();
            }
            PlayerController.Instance.RefreshStatUI();
        }
    }

    private static void RefreshGear()
    {
        Instance.WeaponTitle.text = PlayerController.Instance.Weapon.Title;
        Instance.WeaponInfo.text = PlayerController.Instance.Weapon.Info;
        Instance.WeaponIcon.sprite = PlayerController.Instance.Weapon.Icon;

        Instance.ArmorTitle.text = PlayerController.Instance.Armor.Title;
        Instance.ArmorInfo.text = PlayerController.Instance.Armor.Info;
        Instance.ArmorIcon.sprite = PlayerController.Instance.Armor.Icon;
    }

    public static void CloseShop()
    {
        PlayerController.IsPaused = false;
        Instance.ShopRootPanel.SetActive(false);
    }
}
