using UnityEngine;

public class AspectRatioEnforcer : MonoBehaviour
{
    public float targetAspectRatio = 16.0f / 9.0f; // Target rasio 16:9

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("AspectRatioEnforcer requires a Camera component!");
            enabled = false;
            return;
        }
        UpdateAspectRatio();
    }

    void Update()
    {
        // Opsional: panggil ini jika resolusi layar bisa berubah saat runtime (misal: rotasi perangkat)
        // Kalau tidak, Awake() cukup.
        // UpdateAspectRatio();
    }

    void UpdateAspectRatio()
    {
        // Hitung rasio aspek aktual layar
        float currentAspectRatio = (float)Screen.width / Screen.height;

        // Jika rasio aktual lebih lebar dari target (perlu pillarbox)
        if (currentAspectRatio > targetAspectRatio)
        {
            float widthNormalized = targetAspectRatio / currentAspectRatio;
            float startX = (1f - widthNormalized) / 2f;
            cam.rect = new Rect(startX, 0f, widthNormalized, 1f);
        }
        // Jika rasio aktual lebih sempit dari target (perlu letterbox)
        else if (currentAspectRatio < targetAspectRatio)
        {
            float heightNormalized = currentAspectRatio / targetAspectRatio;
            float startY = (1f - heightNormalized) / 2f;
            cam.rect = new Rect(0f, startY, 1f, heightNormalized);
        }
        // Jika rasio sama, gunakan seluruh layar
        else
        {
            cam.rect = new Rect(0f, 0f, 1f, 1f);
        }
    }
}