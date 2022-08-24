using System.Collections;
using System.Text;
using UnityEngine;

public class DebugCountdown : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI text;
    [SerializeField]
    private float timeInSeconds;

    private float timeRemaining;
    private StringBuilder strBuilder;

    // Start is called before the first frame update
    void Start()
    {
        strBuilder = new StringBuilder();
        timeRemaining = timeInSeconds; 
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        SetTimeString();
    }

    private void SetTimeString()
    {
        strBuilder.Clear();

        int minutes = (int)(timeRemaining / 60f);
        int seconds = (int)(timeRemaining % 60f);
        int decis = ((int)(timeRemaining * 100) % 100);

        strBuilder.Append(minutes).Append(':').Append(seconds.ToString("00")).Append('.').Append(decis.ToString("00"));
        text.text = strBuilder.ToString();
    }
}
