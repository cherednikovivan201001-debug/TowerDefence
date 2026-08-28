using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] private AudioSource _triggerAudio;

    [SerializeField] private string _phrase;

    [SerializeField] private GameObject _model;

    [SerializeField] private ParticleSystem _pickupParticle;

    [SerializeField] private float _rotateSpeed = 3f;

    [SerializeField] private float _floatSpeed = 3f;

    [SerializeField] private float _floatRange = 3f;

    private float _coordY;

    private bool _isUsed = false;

    private AppleController _controller;

    public void Start()
    {
        _triggerAudio = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (_isUsed || other.GetComponentInParent<PlayerController>() == null)
            return;

        _isUsed = true;

        if (_triggerAudio != null && !_triggerAudio.isPlaying)
        {
            _triggerAudio.Play();
            Debug.Log(_phrase);
        }

        if (_model != null)
            _model.SetActive(false);

        if (_controller != null)
            _controller.ApplePickedCommand();

        if (_pickupParticle != null)
            _pickupParticle.Play();
    }

        

    public void SetupApple(AppleController controller)
    {
        _controller = controller;

        _coordY = transform.position.y;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);

        float y = _coordY + Mathf.Sin(Time.time * _floatSpeed) * _floatRange;

        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    //public void OnTriggerExit(Collider other)
    //{
    //    if (!_triggerAudio.isPlaying)
    //    {
    //        _triggerAudio.Stop();
    //    }
    //}


}
