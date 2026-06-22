using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class PromediosSystem : MonoBehaviour
{
    [Title("Entradas de notas")]

    [LabelText("Laboratorios (8)")]
    public List<TMP_InputField> laboratorios;

    [LabelText("Avances del proyecto (8)")]
    public List<TMP_InputField> avancesProyecto;

    [LabelText("Exámenes teóricos (4)")]
    public List<TMP_InputField> examenesTeoria;

    [Title("Resultado")]
    public TextMeshProUGUI textoResultado;

    [Button("Calcular promedio")]
    public void CalcularPromedio()
    {
        double promedioLab = CalcularPromedioLista(laboratorios);
        double promedioProyecto = CalcularPromedioLista(avancesProyecto);
        double promedioTeoria = CalcularPromedioLista(examenesTeoria);

        double notaFinal =
            (promedioLab * 0.40) +
            (promedioProyecto * 0.40) +
            (promedioTeoria * 0.20);

        textoResultado.text = notaFinal.ToString("F2");
    }

    private double CalcularPromedioLista(List<TMP_InputField> inputs)
    {
        if (inputs == null || inputs.Count == 0)
            return 0;

        double suma = 0;
        int notasValidas = 0;

        foreach (TMP_InputField input in inputs)
        {
            if (input == null)
                continue;

            if (double.TryParse(input.text, out double nota))
            {
                nota = Mathf.Clamp((float)nota, 0f, 20f);

                suma += nota;
                notasValidas++;
            }
        }

        return notasValidas > 0 ? suma / notasValidas : 0;
    }
}