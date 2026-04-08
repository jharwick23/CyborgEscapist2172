using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrapUI : MonoBehaviour
{
    // References
    [SerializeField] private TextMeshProUGUI scrapText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScrapAmount(float amt)
    {
        scrapText.text = $"{amt:0}";
    }
}
