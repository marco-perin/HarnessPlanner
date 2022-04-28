using UnityEngine;

public class SaveLoadPanelManager : MonoBehaviour
{
    public TMPro.TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        inputField.text = SaveManager.Instance.saveFileName.Replace(SaveManager.Instance.Extension, "");
    }

    public void SetFileName(string fileName)
    {
        SaveManager.Instance.SetFileName(fileName);
    }
}
