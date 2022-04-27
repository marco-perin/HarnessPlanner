using UnityEngine;

public class SaveLoadPanelManager : MonoBehaviour
{
    public TMPro.TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        inputField.text = SaveManager.Instance.saveFileName;
    }

    public void SetFileName(string fileName)
    {
        SaveManager.Instance.SetFileName(fileName);
    }
}
