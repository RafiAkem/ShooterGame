using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    // Kecepatan gerakan pesawat
    [SerializeField] private float moveSpeed = 5.0f;

    // Referensi ke Rigidbody2D pesawat
    private Rigidbody2D rb;
    // Referensi ke skrip VirtualJoystick
    private VirtualJoystick virtualJoystick;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Pastikan ada Rigidbody2D terpasang pada GameObject ini
        if (!rb)
        {
            Debug.LogError("SpaceshipController: Rigidbody2D component not found on this GameObject!");
            enabled = false; // Nonaktifkan skrip jika tidak ada Rigidbody2D
            return;
        }

        // Cari objek joystick di Scene (pastikan nama objeknya benar!)
        // Nama "JoystickBackground" adalah nama GameObject tempat skrip VirtualJoystick terpasang
        GameObject joystickObject = GameObject.Find("JoystickBackground");
        if (joystickObject)
        {
            virtualJoystick = joystickObject.GetComponent<VirtualJoystick>();
            if (!virtualJoystick)
            {
                Debug.LogError("SpaceshipController: VirtualJoystick script not found on JoystickBackground object!");
            }
        }
        else
        {
            Debug.LogError("SpaceshipController: 'JoystickBackground' GameObject not found in the scene! Make sure your joystick UI setup is correct.");
        }

        // Penting: Bekukan rotasi Rigidbody2D agar tidak berputar secara fisik
        rb.freezeRotation = true;
    }

    void FixedUpdate() // Gunakan FixedUpdate untuk operasi fisika yang konsisten
    {
        if (virtualJoystick == null) return; // Hentikan jika joystick belum ditemukan

        // Baca input dari VirtualJoystick
        Vector2 movementInput = virtualJoystick.InputVector;

        // Gerakan Linear (Translasi)
        // Gunakan .normalized untuk memastikan kecepatan konstan di semua arah (termasuk diagonal)
        Vector2 velocity = movementInput.normalized * moveSpeed;
        rb.velocity = velocity;

        // --- Bagian Rotasi Dihilangkan ---
        // Jika movementInput != Vector2.zero, tidak ada kode rotasi di sini
    }
}