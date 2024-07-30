using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public partial class BuildingManager : MonoBehaviour
{
    public Button addButton;
    public Button removeButton;

    public BuildingMode currentMode;

    void Start()
    {
        addButton.onClick.AddListener(() => { currentMode = BuildingMode.ADD; });
        removeButton.onClick.AddListener(() => { currentMode = BuildingMode.REMOVE; });
    }


}
