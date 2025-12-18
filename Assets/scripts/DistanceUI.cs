using UnityEngine;
using TMPro; // or UnityEngine.UI for normal Text

public class DistanceUI : MonoBehaviour
{
    public ArrowDistance arrowScript;   // drag arrow object here
    public TextMeshProUGUI distanceText; // or Text

    void Update()
    {
        distanceText.text = $"{arrowScript.DistanceToTarget:0} m";
    }
}
