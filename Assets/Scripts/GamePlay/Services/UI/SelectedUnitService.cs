using Source.GamePlay.Services.Unit.Instance;
using Source.GamePlay.Services.Unit.Instance.Types;
using Source.GamePlay.Static.ScriptableObjects;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.GamePlay.Services.UI
{
    public class SelectedUnitService : MonoBehaviour
    {
        [field: SerializeField]
        private List<Button> SpecialButtons { get; set; }
        [SerializeField]
        private TMP_Text SelectedLabel;

        private void Start()
        {
            DisableSpecialButtons();
        }

        private void DisableSpecialButtons()
        {
            foreach (Button button in SpecialButtons)
            {
                button.gameObject.SetActive(false);
            }
        }

        public void OnNewSelection(IEnumerable<UnitService> selectedUnits)
        {
            ProcessUnits(selectedUnits);
        }

        private void ProcessUnits(IEnumerable<UnitService> selectedUnits)
        {
            Dictionary<UnitType, List<UnitService>> unitWithSpecials = new Dictionary<UnitType, List<UnitService>>();
            List<string> unitLabels = new List<string>();

            foreach (UnitService unit in selectedUnits)
            {
                string typeName = unit.UnitTypeService.GetType().Name;
                unitLabels.Add(typeName);

                switch (typeName)
                {
                    case nameof(VanguardUnitType):
                        AddTypeToDict(unitWithSpecials, UnitType.Vanguard, unit);
                        break;
                    default:
                        break;
                }
            }

            DisplaySelectedUnits(unitLabels);
            DisplaySpecialButtons(unitWithSpecials);
        }

        private void AddTypeToDict(Dictionary<UnitType, List<UnitService>> unitTypeDict, UnitType type, UnitService unit)
        {
            if (unitTypeDict.TryGetValue(type, out List<UnitService> unitList))
            {
                unitList.Add(unit);
            }
            else
            {
                unitTypeDict.Add(type, new List<UnitService>() { unit });
            }
        }

        private void DisplaySelectedUnits(List<string> selectedUnitTypes)
        {
            string label = "Selected Units: ";

            if (selectedUnitTypes.Count == 0)
            {
                label += "None";
            }
            else
            {
                foreach (string type in selectedUnitTypes)
                {
                    label += $"{type}, ";
                }
            }

            SelectedLabel.text = label;
        }

        private void DisplaySpecialButtons(Dictionary<UnitType, List<UnitService>> unitsWithSpecial)
        {
            int currentButtonIndex = 0;
            if (unitsWithSpecial.TryGetValue(UnitType.Vanguard, out List<UnitService> vanguardUnits))
            {
                SetSpecialButton(currentButtonIndex, vanguardUnits);
                currentButtonIndex ++;
            }

            while (currentButtonIndex < SpecialButtons.Count)
            {
                SpecialButtons[currentButtonIndex].gameObject.SetActive(false);
                currentButtonIndex++;
            }
        }

        private void SetSpecialButton(int buttonIndex, List<UnitService> units)
        {
            Button currentButton = SpecialButtons[buttonIndex];

            currentButton.gameObject.SetActive(true);
            currentButton.onClick.RemoveAllListeners();
            currentButton.onClick.AddListener(() =>
            {
                foreach (UnitService unit in units)
                {
                    unit.UnitTypeService.Special();
                }
            });
        }
    }
}

