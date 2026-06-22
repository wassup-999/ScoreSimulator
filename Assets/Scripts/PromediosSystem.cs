using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class PromediosSystem : MonoBehaviour
{
    public enum TipoCursoTecsup
    {
        SoloAulaYExamen,        // Aula 70% | Examen Final 30%
        AulaLabYExamen,         // Laboratorio 40% | Aula 30% | Examen Final 30%
        SoloTallerYExamen       // Taller 70% | Examen Final 30%
    }

    [Title("Configuración del Curso")]
    [LabelText("Esquema de Evaluación")]
    public TipoCursoTecsup tipoCurso = TipoCursoTecsup.AulaLabYExamen;

    [Title("Entradas de Notas (0 - 20)")]

    [LabelText("Laboratorios / Talleres (Fila 1)")]
    public List<TMP_InputField> notasLaboratorio;

    [LabelText("Notas Continuas de Aula (Fila 2)")]
    public List<TMP_InputField> notasAula;

    [LabelText("Exámenes Parciales Extra (Fila 3 - 4 inputs)")]
    public List<TMP_InputField> examenesParciales;

    [LabelText("Examen Final (Único Input)")]
    public TMP_InputField examenFinalInput;

    [Title("Resultado Oficial Tecsup")]
    public TextMeshProUGUI textoResultado;

    [Button("Calcular Promedio Tecsup")]
    public void CalcularPromedio()
    {
        double promedioLab = CalcularPromedioLista(notasLaboratorio);

        // Calculamos los dos componentes de la teoría por separado
        double promedioNotasContinuas = CalcularPromedioLista(notasAula);
        double promedioParciales = CalcularPromedioLista(examenesParciales);

        // Combinamos la evaluación continua de aula con sus exámenes parciales (50% y 50%)
        // para consolidar la nota final del criterio de "Aula"
        double promedioAulaDefinitivo = (promedioNotasContinuas + promedioParciales) / 2.0;
        if (examenesParciales == null || examenesParciales.Count == 0) promedioAulaDefinitivo = promedioNotasContinuas;

        double notaExamenFinal = ObtenerNotaIndividual(examenFinalInput);
        double notaFinalDecimal = 0;

        // Fórmulas oficiales de Tecsup aplicando el promedio de aula consolidado
        switch (tipoCurso)
        {
            case TipoCursoTecsup.SoloAulaYExamen:
                notaFinalDecimal = (promedioAulaDefinitivo * 0.70) + (notaExamenFinal * 0.30);
                break;

            case TipoCursoTecsup.AulaLabYExamen:
                notaFinalDecimal = (promedioLab * 0.40) + (promedioAulaDefinitivo * 0.30) + (notaExamenFinal * 0.30);
                break;

            case TipoCursoTecsup.SoloTallerYExamen:
                notaFinalDecimal = (promedioLab * 0.70) + (notaExamenFinal * 0.30);
                break;
        }

        // Redondeo institucional: Fracciones >= 0.5 suben al entero superior
        int notaFinalOficial = (int)System.Math.Round(notaFinalDecimal, System.MidpointRounding.AwayFromZero);
        notaFinalOficial = System.Math.Clamp(notaFinalOficial, 0, 20);

        textoResultado.text = notaFinalOficial.ToString();
    }

    private double CalcularPromedioLista(List<TMP_InputField> inputs)
    {
        if (inputs == null || inputs.Count == 0) return 0;

        double suma = 0;
        int notasValidas = 0;

        foreach (var input in inputs)
        {
            if (input == null) continue;
            string text = input.text.Trim();
            if (string.IsNullOrEmpty(text)) continue;

            if (double.TryParse(text, out double nota))
            {
                nota = System.Math.Clamp(nota, 0.0, 20.0);
                suma += nota;
                notasValidas++;
            }
        }

        return notasValidas > 0 ? (suma / notasValidas) : 0;
    }

    private double ObtenerNotaIndividual(TMP_InputField input)
    {
        if (input == null || string.IsNullOrEmpty(input.text.Trim())) return 0;

        if (double.TryParse(input.text.Trim(), out double nota))
        {
            return System.Math.Clamp(nota, 0.0, 20.0);
        }
        return 0;
    }
}