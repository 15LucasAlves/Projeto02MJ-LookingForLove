using UnityEngine;
using System.Collections.Generic;

public class TesteRandomizer : MonoBehaviour
{
    void Start()
    {
        // 1. Instanciar o randomizer
        Randomizer randomizer = new Randomizer();

        // 2. Faz um loop para testar da ronda 1 até à 10
        for (int ronda = 1; ronda <= 10; ronda++)
        {
            List<ElementoJogo> resultado = randomizer.GerarRonda(ronda);

            // 3. Constroi uma string com o resultado para ser fácil de ler na consola
            string textoRonda = $"<color=yellow><b>--- RONDA {ronda} ---</b></color>\n";

            foreach (var elemento in resultado)
            {
                // Se for o copas, metemos a bold para destacar
                if (elemento.Forma == Forma.Copas)
                    textoRonda += $"<b>-> {elemento.Forma} {elemento.Cor}</b>\n";
                else
                    textoRonda += $"- {elemento.Forma} {elemento.Cor}\n";
            }

            // 4. Imprime na consola do Unity
            Debug.Log(textoRonda);
        }
    }
}