using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    // Inventory variables
    private float amountScrap;

    // References
    [SerializeField] private ScrapUI scrap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!scrap)
        {
            scrap = GetComponent<ScrapUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(amountScrap == 1)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void IncreaseScrap(float amt)
    {
        amountScrap += amt;
        scrap.UpdateScrapAmount(amountScrap);
    }
}
