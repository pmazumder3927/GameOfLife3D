using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material cubeMaterial;
    [SerializeField] private Game game;

    public UnityEngine.UI.Text infoText;

    private Renderer selectedRenderer;
    private GameObject selectedObject;

    private bool buildMode = false;
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

        if (Input.GetMouseButtonDown(0))
        {
            if (buildMode)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100))
                {
                    Debug.Log(hit.transform.name);
                    Cell hitCell = hit.transform.gameObject.GetComponent<Cell>();
                    hitCell.SetProps(new CellProperties(!hitCell.GetProps().alive));
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100))
                {
                    if (selectedRenderer)
                    {
                        selectedRenderer.material = cubeMaterial;
                    }
                    Debug.Log(hit.transform.name);
                    Cell hitCell = hit.transform.gameObject.GetComponent<Cell>();
                    selectedRenderer = hit.transform.gameObject.GetComponent<Renderer>();
                    if (selectedRenderer != null)
                    {
                        selectedRenderer.material = selectedMaterial;
                    }
                    Vector3 hitPos = hitCell.transform.position;
                    infoText.text = String.Format("Hit Cube: {0} {1} {2}, neighbors: {3}", hitPos.x, hitPos.y, hitPos.z, game.checkNeighbors(Mathf.RoundToInt(hitPos.x), Mathf.RoundToInt(hitPos.y), Mathf.RoundToInt(hitPos.z)));
                }
            }
        }

        else if (Input.GetMouseButtonDown(1))
        {
            if (buildMode)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100))
                {
                    Debug.Log(hit.transform.name);
                    Cell hitCell = hit.transform.gameObject.GetComponent<Cell>();
                }
                game.CreateAtCoordinate(hit.point + hit.normal * 0.1f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            buildMode = !buildMode;
        }
    }
}