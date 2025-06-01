using UnityEngine;
using UnityEngine.EventSystems; // Penting untuk deteksi input UI

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // Referensi ke RectTransform dari latar belakang joystick dan handle (pegangan)
    [SerializeField] private RectTransform joystickBackground;
    [SerializeField] private RectTransform joystickHandle;

    // Radius maksimum handle bisa bergerak dari tengah
    [SerializeField] private float joystickRange = 100f;

    // Vektor input yang akan dibaca oleh skrip pemain
    private Vector2 inputVector;
    private Vector2 joystickOriginalPos; // Posisi asli latar belakang joystick (untuk reset handle)

    // Properti publik untuk membaca input dari skrip lain
    public Vector2 InputVector { get { return inputVector; } }

    void Start()
    {
        // Simpan posisi awal latar belakang joystick
        joystickOriginalPos = joystickBackground.position;
        // Pastikan handle berada di tengah saat game dimulai
        joystickHandle.anchoredPosition = Vector2.zero;
    }

    // Dipanggil saat jari menyentuh area joystick
    public void OnPointerDown(PointerEventData eventData)
    {
        // Langsung panggil OnDrag agar handle bergerak saat disentuh
        OnDrag(eventData);
    }

    // Dipanggil saat jari digeser di atas area joystick
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        // Konversi posisi sentuhan layar ke posisi lokal di dalam RectTransform joystickBackground
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground, eventData.position, eventData.pressEventCamera, out pos))
        {
            // Normalisasi posisi handle relatif terhadap ukuran latar belakang joystick
            // (pos.x dan pos.y akan berkisar antara -0.5 hingga 0.5)
            pos.x = (pos.x / joystickBackground.sizeDelta.x);
            pos.y = (pos.y / joystickBackground.sizeDelta.y);

            // Kalikan 2 untuk membuat rentang -1 hingga 1, ini adalah vektor input mentah
            inputVector = new Vector2(pos.x * 2, pos.y * 2);

            // Batasi magnitude (panjang) inputVector agar tidak melebihi 1.0f
            // Ini memastikan joystick tidak memberikan input lebih dari "penuh" saat ditarik terlalu jauh
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Gerakkan handle joystick sesuai dengan inputVector dan joystickRange
            joystickHandle.anchoredPosition = inputVector * joystickRange;
        }
    }

    // Dipanggil saat jari dilepaskan dari area joystick
    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset input dan kembalikan handle ke posisi tengah
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }
}