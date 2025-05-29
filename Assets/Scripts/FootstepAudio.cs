using UnityEngine;
using UnityEngine.Playables;

public class FootstepAudio : MonoBehaviour
{
    [SerializeField] private AudioClip leftFootClip;  // Audio untuk langkah kiri
    [SerializeField] private AudioClip rightFootClip; // Audio untuk langkah kanan
    [SerializeField] private float stepInterval = 0.5f; // Interval antar langkah (detik)

    private AudioSource audioSource;
    private bool isPlaying = false; // Status apakah suara sedang diputar
    private bool isLeftStep = true; // Menentukan langkah awal (kiri/kanan)
    private float stepTimer = 0f;

    void Start()
    {
        // Ambil komponen AudioSource dari GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Jika suara sedang aktif, putar langkah secara otomatis
        if (isPlaying)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f; // Reset timer
            }
        }
    }

    void PlayFootstep()
    {
        // Pilih audio berdasarkan langkah kiri/kanan
        AudioClip clipToPlay = isLeftStep ? leftFootClip : rightFootClip;

        // Pastikan AudioClip tidak null
        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }

        // Ganti langkah berikutnya (kiri -> kanan, kanan -> kiri)
        isLeftStep = !isLeftStep;
    }

    // Fungsi untuk memulai suara (dipanggil dari Timeline)
    public void StartFootsteps()
    {
        isPlaying = true;
        stepTimer = 0f; // Reset timer saat memulai
    }

    // Fungsi untuk menghentikan suara (dipanggil dari Timeline)
    public void StopFootsteps()
    {
        isPlaying = false;
        audioSource.Stop();
        stepTimer = 0f; // Reset timer saat berhenti
    }
}