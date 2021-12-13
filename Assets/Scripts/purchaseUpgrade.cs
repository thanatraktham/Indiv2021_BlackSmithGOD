using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class purchaseUpgrade : MonoBehaviour
{
    // public GameObject material_1;
    // public GameObject material_2;
    // public GameObject material_3;

    public int material_1_cost;
    public int material_2_cost;
    public int material_3_cost;

    public Button upgrade_button;
    public Button equip_button;
    public Button unequip_button;

    public Text purchaseStatus;

    private GameManager gm;

    private void Start() {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        purchaseStatus.text = "";
        purchaseStatus.color = Color.green;

        upgrade_button.onClick.AddListener(delegate{
            if (    (gm.getMaterial("material_1") > material_1_cost) &&
                    (gm.getMaterial("material_2") > material_2_cost) &&
                    (gm.getMaterial("material_3") > material_3_cost)) {
                purchase(material_1_cost, material_2_cost, material_3_cost);
            } else {
                Debug.Log("Insufficient Fund");
                purchaseStatus.text = "Insufficient Fund";
                purchaseStatus.color = Color.red;
            }
        });
        equip_button.onClick.AddListener(delegate{
            purchaseStatus.text = "Equiped";
            purchaseStatus.color = Color.green;
        });
        unequip_button.onClick.AddListener(delegate{
            purchaseStatus.text = "Purchased";
            purchaseStatus.color = Color.yellow;
        });

    }

    public void purchase(int material_1_cost, int material_2_cost, int material_3_cost) {
        
        gm.removeMaterial("material_1", material_1_cost);
        gm.removeMaterial("material_2", material_2_cost);
        gm.removeMaterial("material_3", material_3_cost);

        gm.SaveMatarial();
        gm.LoadMaterial();

        purchaseStatus.text = "Purchased";
        purchaseStatus.color = Color.yellow;

        upgrade_button.gameObject.SetActive(false);
        equip_button.gameObject.SetActive(true);
        unequip_button.gameObject.SetActive(false);
    }

}
