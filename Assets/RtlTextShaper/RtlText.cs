using UnityEngine;
using TMPro;

[ExecuteAlways]
public class RtlText : MonoBehaviour
{
    [TextArea(3, 10)]
    [Tooltip("Type your logical (un‑shaped) Arabic/Persian text here.")]
    public string Text;

    TMP_Text _textMesh;

    private void OnEnable()
    {
        _textMesh = GetComponent<TMP_Text>();
        UpdateShapedText();
    }

    private void OnValidate()
    {
        if (_textMesh == null) _textMesh = GetComponent<TMP_Text>();
        _textMesh.isRightToLeftText = true;
        UpdateShapedText();
    }

    private void UpdateShapedText()
    {
        if (_textMesh == null) return;
        _textMesh.text = RtlTextShaper.FixTextForTextMeshPro(Text);
    }
}