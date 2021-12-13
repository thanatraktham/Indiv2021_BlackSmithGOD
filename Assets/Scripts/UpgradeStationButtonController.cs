using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStationButtonController : MonoBehaviour
{

    public GameObject upgradeButton1;
    public GameObject upgradeButton2;
    public GameObject upgradeButton3;

    public GameObject equipButton1;
    public GameObject equipButton2;
    public GameObject equipButton3;

    public GameObject unequipButton1;
    public GameObject unequipButton2;
    public GameObject unequipButton3;

    public Text purchaseStatus1;
    public Text purchaseStatus2;
    public Text purchaseStatus3;

    private GameManager gm;

    public void resetEquipButton() {

        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (!upgradeButton1.gameObject.activeSelf) {
            equipButton1.SetActive(true);
            purchaseStatus1.text = "Purchased";
            purchaseStatus1.color = Color.yellow;
        }
        if (!upgradeButton2.gameObject.activeSelf) {
            equipButton2.SetActive(true);
            purchaseStatus2.text = "Purchased";
            purchaseStatus2.color = Color.yellow;
        }
        if (!upgradeButton3.gameObject.activeSelf) {
            equipButton3.SetActive(true);
            purchaseStatus3.text = "Purchased";
            purchaseStatus3.color = Color.yellow;
        }

        unequipButton1.SetActive(false);
        unequipButton2.SetActive(false);
        unequipButton3.SetActive(false);
    }

    
    public void clearTextUI() {
        if (upgradeButton1.gameObject.activeSelf) {
            purchaseStatus1.text = "";
            purchaseStatus1.color = Color.white;
        }
        if (upgradeButton2.gameObject.activeSelf) {
            purchaseStatus2.text = "";
            purchaseStatus2.color = Color.white;
        }
        if (upgradeButton3.gameObject.activeSelf) {
            purchaseStatus3.text = "";
            purchaseStatus3.color = Color.white;
        }

        // resetEquipButton();

        if (unequipButton1.gameObject.activeSelf) {
            purchaseStatus1.text = "Equiped";
            purchaseStatus1.color = Color.green;
        }
        if (unequipButton2.gameObject.activeSelf) {
            purchaseStatus2.text = "Equiped";
            purchaseStatus2.color = Color.green;
        }
        if (unequipButton3.gameObject.activeSelf) {
            purchaseStatus3.text = "Equiped";
            purchaseStatus3.color = Color.green;
        }

        if (equipButton1.gameObject.activeSelf) {
            purchaseStatus1.text = "Purchased";
            purchaseStatus1.color = Color.yellow;
        }
        if (equipButton2.gameObject.activeSelf) {
            purchaseStatus2.text = "Purchased";
            purchaseStatus2.color = Color.yellow;
        }
        if (equipButton3.gameObject.activeSelf) {
            purchaseStatus3.text = "Purchased";
            purchaseStatus3.color = Color.yellow;
        }
    }


}
