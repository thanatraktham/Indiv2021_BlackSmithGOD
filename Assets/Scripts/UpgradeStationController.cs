using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStationController : MonoBehaviour
{
    // private Rigidbody2D rb;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public GameObject meleeUpgradeCanvas;
    public GameObject startMeleeUpgradeStationButton;

    public Button backButton;

    private GameManager gm;

    private void Start() {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        startMeleeUpgradeStationButton.GetComponent<Button>().onClick.AddListener(delegate{
            gm.setIsTalking(true);
        });

        backButton.onClick.AddListener(delegate{
            gm.setIsTalking(false);
            meleeUpgradeCanvas.SetActive(false);
        });
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Enter Melee Station");
        startMeleeUpgradeStationButton.SetActive(true);
        // meleeUpgradeCanvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other) {
        // Debug.Log("Exit Melee Station");
        // meleeUpgradeCanvas.SetActive(false);
        startMeleeUpgradeStationButton.SetActive(false);
    }

    private void toggleUpgradeUI() {
        startMeleeUpgradeStationButton.SetActive(false);
        meleeUpgradeCanvas.SetActive(true);
    }
}
