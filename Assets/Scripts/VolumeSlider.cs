using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;

    [SerializeField] private AudioSource _playerFootsteps;

    [SerializeField] private AudioSource _volume;

    private void OnEnable()
    {
        _volumeSlider.onValueChanged.AddListener(OnChange);
    }

    private void OnDisable()
    {
        _volumeSlider.onValueChanged.RemoveListener(OnChange);
    }

    private void OnChange(float num)
    {
        _volume.volume = num;
    }

}
