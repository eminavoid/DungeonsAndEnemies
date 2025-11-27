using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI classDescriptionText;
    [SerializeField] private TextMeshProUGUI classNameText;

    [Header("Datos")]
    [SerializeField] private ClassData defaultClass; // Para seleccionar uno por defecto

    private ClassData currentSelection;

    private void Start()
    {
        // Seleccionar el primero por defecto para que no esté vacío
        if (defaultClass != null) SelectClass(defaultClass);
    }

    // Este método lo llamaremos desde los Botones de la UI
    public void SelectClass(ClassData data)
    {
        currentSelection = data;

        // Actualizar UI visualmente
        if (classNameText) classNameText.text = data.className;
        if (classDescriptionText) classDescriptionText.text = data.description;

        // Guardar en el GameManager
        GameManager.Instance.SetClass(data);
    }

    public void OnStartButton()
    {
        if (currentSelection != null)
        {
            GameManager.Instance.StartGame();
        }
    }
}