using UnityEngine;
using UnityEngine.UI; // Obrigatório para aceder ao RawImage
using System.Collections.Generic;
using System.Linq;

public enum Direcao { Cima, Baixo, Esquerda, Direita }

// Classe para ligares as tuas 4 RawImages (brancas) do Canvas ao código
[System.Serializable]
public class SlotMesa
{
    public Direcao direcao;
    public RawImage renderizador; // Alterado para RawImage
}

// Classe para associares as tuas Texture2D às combinações
[System.Serializable]
public class TexturaVisual
{
    public Forma Forma;
    public Cor Cor;
    public Texture2D Textura; // Alterado para Texture2D
}

public class TesteRandomizer : MonoBehaviour
{
    [Header("Configuração da Mesa")]
    public List<SlotMesa> slotsMesa; // Arrasta as RawImages do teu Canvas para aqui

    [Header("Dicionário de Texturas")]
    public List<TexturaVisual> dicionarioTexturas;

    [Header("Mecânica de Jogo")]
    public Direcao direcaoCorreta; // Guarda a resposta certa desta ronda

    void Start()
    {
        // Exemplo: Iniciar a ronda 9
        IniciarRonda(0);
    }

    public void IniciarRonda(int numeroRonda)
    {
        Randomizer randomizer = new Randomizer();
        List<ElementoJogo> elementosGerados = randomizer.GerarRonda(numeroRonda);

        // 1. Limpar as texturas de todos os 4 slots antes de começar
        foreach (var slot in slotsMesa)
        {
            slot.renderizador.texture = null; // Limpa a RawImage
        }

        // 2. Embaralhar a ordem dos slots para que os elementos calhem em direções aleatórias
        List<SlotMesa> slotsAleatorios = slotsMesa.OrderBy(x => Random.value).ToList();

        // 3. Aplicar as texturas geradas aos slots
        for (int i = 0; i < elementosGerados.Count; i++)
        {
            ElementoJogo elemento = elementosGerados[i];
            SlotMesa slot = slotsAleatorios[i];

            // Procurar a textura correta no dicionário
            TexturaVisual visual = dicionarioTexturas.Find(v => v.Forma == elemento.Forma && v.Cor == elemento.Cor);

            if (visual != null)
            {
                // Aplica a Texture2D à RawImage
                slot.renderizador.texture = visual.Textura;
            }

            // 4. Se este elemento for o Copas, definimos esta direção como a resposta correta
            if (elemento.Forma == Forma.Copas)
            {
                direcaoCorreta = slot.direcao;
                Debug.Log($"<color=green>A direção correta é: {direcaoCorreta}</color>");
            }
        }
    }
}