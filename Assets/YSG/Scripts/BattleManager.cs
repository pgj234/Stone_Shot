using UnityEngine;
using UnityEngine.EventSystems;

public class BattleManager : MonoBehaviour
{
    public Camera cam;
    public LineRenderer aimLine;
    public LayerMask unitLayer;

    public float power = 5f;
    public Vector2 powerRange = new Vector2(0.05f, 2.5f);

    private UnitSystem selected;
    private Vector2 dragStartWorld;
    private bool isDragging;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
        if (aimLine == null) aimLine = GetComponent<LineRenderer>();

        if (aimLine != null)
        {
            aimLine.positionCount = 2;
            aimLine.enabled = false;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!GameManager.instance.PlayerTurnCheck()) return;

#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    #region 입력
    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
            BeginDrag(Input.mousePosition);
        else if (Input.GetMouseButton(0))
            DoDrag(Input.mousePosition);
        else if (Input.GetMouseButtonUp(0))
            EndDrag(Input.mousePosition);
    }

    private void HandleTouch()
    {
        if (Input.touchCount == 0) return;
        Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began)
            BeginDrag(t.position, t.fingerId);
        else if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
            DoDrag(t.position);
        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            EndDrag(t.position);
    }

    private bool PointerOverUI(int fingerId = -1) => EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId);

    private Vector2 ScreenToWorld(Vector2 _screenPos) => cam.ScreenToWorldPoint(_screenPos);

    private bool CanSelect(UnitSystem _unit)
    {
        if (_unit == null || !_unit.CompareTag("Player")) return false;

        var rb = _unit.GetComponent<Rigidbody2D>();
        return rb != null && rb.linearVelocity.sqrMagnitude <= 0.01f;
    }
    #endregion

    #region 드래그
    private void BeginDrag(Vector2 _screenPos, int _fingerId = -1)
    {
        if (PointerOverUI(_fingerId)) return;

        Vector2 world = ScreenToWorld(_screenPos);
        RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero, 0.01f, unitLayer);

        if (hit.collider != null && hit.collider.TryGetComponent(out UnitSystem _unit) && CanSelect(_unit))
        {
            selected = _unit;
            selected.SetSelected(true);

            dragStartWorld = world;
            isDragging = true;
            if (aimLine != null) aimLine.enabled = true;
        }
    }

    private void DoDrag(Vector2 _screenPos)
    {
        if (!isDragging || selected == null) return;

        UpdateAimLine();
    }

    private void EndDrag(Vector2 _screenPos)
    {
        if (selected != null) selected.SetSelected(false);

        if (!isDragging) return;

        Vector2 endWorld = ScreenToWorld(_screenPos);
        Vector2 drag = endWorld - dragStartWorld;
        Vector2 shotDir = -drag;

        float dist = shotDir.magnitude;
        dist = Mathf.Clamp(dist, powerRange.x, powerRange.y);

        if (selected != null)
        {
            Vector2 impulse = shotDir.normalized * dist * power;

            if (impulse.magnitude == 0) return;

            selected.AddImpulse(impulse);
        }

        isDragging = false;
        selected = null;
        if (aimLine != null) aimLine.enabled = false;

        GameManager.instance.TurnChange();
    }
    #endregion

    #region 조준
    private void UpdateAimLine()
    {
        if (!isDragging || selected == null || aimLine == null) return;

        Vector3 start;
        var rb = selected.GetComponent<Rigidbody2D>();
        if (rb != null) start = rb.worldCenterOfMass;
        else start = selected.transform.position;

#if UNITY_EDITOR
        Vector3 cur = ScreenToWorld(Input.mousePosition);
#else
        Vector3 cur = ScreenToWorld(Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : (Vector3)Input.mousePosition);
#endif

        Vector3 dir = (start - cur);
        float dist = Mathf.Clamp(dir.magnitude, powerRange.x, powerRange.y);
        Vector3 end = start + dir.normalized * dist;

        aimLine.SetPosition(0, start);
        aimLine.SetPosition(1, end);
    }
    #endregion
}
