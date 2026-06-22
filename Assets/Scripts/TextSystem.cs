using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Collections.Generic;
public class TextSystem : MonoBehaviour
{
    public List<TMP_InputField> Valores;
    public TextMeshProUGUI TextResult;


    void Start()
    {
        
    }

    [Button]
    public void PruebaPromedios()
    {
        double.TryParse(Valores[0].text, out double num1);
        double.TryParse(Valores[1].text, out double num2);
        double.TryParse(Valores[2].text, out double num3);
       

        double promedio = (num1 + num2 + num3) / Valores.Count;

        TextResult.text = promedio.ToString("F2");
    }
    void Update()
    {
        
    }
}
