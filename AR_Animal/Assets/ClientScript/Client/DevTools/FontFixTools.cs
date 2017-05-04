using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FontFixTools : MonoBehaviour
{
    public enum FontType
    {
        NGUI,
        Unity,

    }

    public FontType fType = FontType.Unity;
    
}
