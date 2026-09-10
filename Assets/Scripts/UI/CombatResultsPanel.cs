using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatResultsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultsText;

    private const string VictoryText = "Victory!";
    private const string DefeatText = "Defeat!";

    public void Initialize(bool isVictory)
    {
        resultsText.SetText($"{(isVictory ? VictoryText : DefeatText)}");
    }

    public void OnPlayAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void OnExitButton()
    {
        Application.Quit();
    }
}
