using UnityEngine;
using UnityEngine.InputSystem;

public class MouseComponent : MonoBehaviour
{
    private SpriteRenderer mouseRenderer;
    //Cursor textures
    [SerializeField] private Sprite targetCursor, menuCursor;
    [SerializeField] private float mouseZPosition=-99.5f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float epsilon=0.001f;

    private float initialOrthographicSize;
    private Vector2 initialScale;



    void Start()
    {
        Cursor.visible=false;
        mouseRenderer = GetComponent<SpriteRenderer>();
        initialOrthographicSize = mainCamera.orthographicSize;
        initialScale = transform.localScale;
    }



    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z=mouseZPosition;
        transform.position = mousePosition;

        if((mainCamera.orthographicSize-initialOrthographicSize)>epsilon)
        {
            transform.localScale = initialScale * (mainCamera.orthographicSize/initialOrthographicSize);
        }
    }



    public void SetMenuCursor()
    {
        mouseRenderer.sprite=menuCursor;
    }



    public void SetTargetCursor()
    {
        mouseRenderer.sprite=targetCursor;
    }
}
