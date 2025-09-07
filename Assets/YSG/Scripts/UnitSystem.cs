using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Rigidbody2D))]
public class UnitSystem : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Color originalColor;

    [Range(0.5f, 3f)] public float mass = 1f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        originalColor = sr.color;
    }

    private void Start()
    {

    }

    private void Update()
    {
        ApplyMass();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        ApplyMass();
    }
#endif

    private void ApplyMass()
    {
        rb.mass = mass;
        transform.localScale = Vector3.one * mass;
    }

    public void AddImpulse(Vector2 _impulse)
    {
        rb.AddForce(_impulse, ForceMode2D.Impulse);
    }

    public void SetSelected(bool _selected)
    {
        if (sr == null) return;
        sr.color = _selected ? Color.red : originalColor;
    }
}
