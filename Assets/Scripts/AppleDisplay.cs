using TMPro;
using UnityEngine;

public class AppleDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _textCounter;

    private void OnEnable()
    {
        AppleController._applePicked += OnApplePicked;
    }

    private void OnDisable()
    {
        AppleController._applePicked -= OnApplePicked;
    }

    private void OnApplePicked(int count)
    {
        _textCounter.text = count.ToString();
    }
}
