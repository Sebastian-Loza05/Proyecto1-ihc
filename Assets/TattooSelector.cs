using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TattooSelector : MonoBehaviour
{
    [Header("UI")]
    public GameObject buttonPrefab;
    public Transform contentParent;

    [Header("Lista")]
    public List<GameObject> tattooPrefabs;
    public List<Sprite>   tattooIcons;

    [Header("Placement Controller")]
    public TattooPlacementController placementController;

    // Interno
    private List<Button> buttons = new List<Button>();
    private int selectedIndex = -1;

    void Start()
    {
        for (int i = 0; i < tattooPrefabs.Count; i++)
        {
            var btnGO = Instantiate(buttonPrefab, contentParent);
            var img   = btnGO.GetComponentInChildren<Image>();
            img.sprite = tattooIcons[i];

            int index = i;
            var btn = btnGO.GetComponent<Button>();
            btn.onClick.AddListener(() => OnTattooButtonClicked(index));
            buttons.Add(btn);
        }
    }

    void OnTattooButtonClicked(int index)
    {
        // 1) Desmarca el anterior
        if (selectedIndex >= 0)
            SetButtonBackgroundColor(buttons[selectedIndex], Color.white);

        // 2) Marca el nuevo
        selectedIndex = index;
        SetButtonBackgroundColor(buttons[index], Color.gray);

        // 3) Cambia el prefab en el placement controller
        placementController.SelectTattooPrefab(tattooPrefabs[index]);
    }

    void SetButtonBackgroundColor(Button btn, Color c)
    {
        var colors = btn.colors;
        colors.normalColor = c;
        btn.colors = colors;
    }
}
