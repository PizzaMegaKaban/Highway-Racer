//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// UI upgrade button.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/UI/HR UI Modification Upgrade")]
public class HR_UIModificationUpgrade : MonoBehaviour {

    public UpgradeClass upgradeClass;
    public enum UpgradeClass { Engine, Handling, Brake, NOS, Speed }

    public int upgradePrice = 2500;
    private bool fullyUpgraded = false;

    public Text priceLabel;
    public Image priceImage;

    private void OnEnable() {

        CheckPurchase();

    }

    public void Upgrade() {

        int playerCoins = HR_API.GetCurrency();

        if (!fullyUpgraded)
            Buy();

        StartCoroutine(CheckPurchase2());

    }

    private IEnumerator CheckPurchase2() {

        yield return new WaitForFixedUpdate();
        CheckPurchase();

    }

    private void CheckPurchase() {

        HR_VehicleUpgrade_UpgradeManager dm = FindObjectOfType<HR_VehicleUpgrade_UpgradeManager>();

        if (!dm)
            return;

        switch (upgradeClass) {

            case UpgradeClass.Engine:
                if (dm.engineLevel >= 5)
                    fullyUpgraded = true;
                else
                    fullyUpgraded = false;
                break;
            case UpgradeClass.Handling:
                if (dm.handlingLevel >= 5)
                    fullyUpgraded = true;
                else
                    fullyUpgraded = false;
                break;
            case UpgradeClass.Brake:
                if (dm.brakeLevel >= 5)
                    fullyUpgraded = true;
                else
                    fullyUpgraded = false;
                break;
            case UpgradeClass.NOS:
                if (dm.nosState)
                    fullyUpgraded = true;
                else
                    fullyUpgraded = false;
                break;
            case UpgradeClass.Speed:
                if (dm.speedLevel >= 5)
                    fullyUpgraded = true;
                else
                    fullyUpgraded = false;
                break;

        }

        if (!fullyUpgraded) {

            if (!priceImage.gameObject.activeSelf)
                priceImage.gameObject.SetActive(true);

            if (priceLabel.text != upgradePrice.ToString())
                priceLabel.text = upgradePrice.ToString();

        } else {

            if (priceImage.gameObject.activeSelf)
                priceImage.gameObject.SetActive(false);

            if (priceLabel.text != "UPGRADED")
                priceLabel.text = "UPGRADED";

        }

    }

    private void Buy() {

        HR_VehicleUpgrade_UpgradeManager dm = FindObjectOfType<HR_VehicleUpgrade_UpgradeManager>();

        if (!dm)
            return;

        int playerCoins = HR_API.GetCurrency();

        if (playerCoins >= upgradePrice) {

            switch (upgradeClass) {

                case UpgradeClass.Engine:
                    if (dm.engineLevel < 5) {
                        dm.UpgradeEngine();
                        HR_API.ConsumeCurrency(upgradePrice);
                    }
                    break;
                case UpgradeClass.Handling:
                    if (dm.handlingLevel < 5) {
                        dm.UpgradeHandling();
                        HR_API.ConsumeCurrency(upgradePrice);
                    }
                    break;
                case UpgradeClass.Brake:
                    if (dm.brakeLevel < 5) {
                        dm.UpgradeBrake();
                        HR_API.ConsumeCurrency(upgradePrice);
                    }
                    break;
                case UpgradeClass.NOS:
                    if (!dm.nosState) {
                        dm.UpgradeNOS();
                        HR_API.ConsumeCurrency(upgradePrice);
                    }
                    break;
                case UpgradeClass.Speed:
                    if (dm.speedLevel < 5) {
                        dm.UpgradeSpeed();
                        HR_API.ConsumeCurrency(upgradePrice);
                    }
                    break;

            }

        } else {

            HR_UIInfoDisplayer.Instance.ShowInfo("Not Enough Coins", "You have to earn " + (upgradePrice - HR_API.GetCurrency()).ToString() + " more coins to purchase this upgrade", HR_UIInfoDisplayer.InfoType.NotEnoughMoney);
            return;

        }

    }

}
