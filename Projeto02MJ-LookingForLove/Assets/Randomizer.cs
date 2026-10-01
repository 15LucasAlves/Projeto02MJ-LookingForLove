using System;
using System.Collections.Generic;
using System.Linq;

// Definição de cores e Formas
public enum Cor { Vermelho, Verde, Azul, Amarelo }
public enum Forma { Paus, Ouros, Copas, Espadas }

//Classe que representa um objeto no ecra
public class  ElementoJogo
{
    public Forma Forma { get; set; }
    public Cor Cor { get; set; }

    public ElementoJogo(Forma forma, Cor cor)
    {
        Forma = forma;
        Cor = cor;
    }
}

public class Randomizer
{
    private Random random = new Random();

    //Metodo de cada ronda
    public List<ElementoJogo> GerarRonda(int numeroRonda)
    {
        List<ElementoJogo> elementoEcra = new List<ElementoJogo>();

        // Lista de formas já usadas na ronda
        List<Forma> formasUsadas = new List<Forma>();

        switch (numeroRonda)
        {
            case 1:
                // 1º: 1 Copas vermelho
                elementoEcra.Add(new ElementoJogo(Forma.Copas, Cor.Vermelho));
                break;
            case 2:
                // 2º: 1 Copas vermelho + 1 simbolo
                elementoEcra.Add(new ElementoJogo(Forma.Copas, Cor.Vermelho));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas));
                break;
            case 3:
                elementoEcra.Add(new ElementoJogo(Forma.Copas, CorAleatoria(Cor.Vermelho)));    // Copas NÃO vermelho
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas, Cor.Vermelho));            // Símbolo VERMELHO
                break;
            case 4:
            case 5:
                // 4º e 5º: 1 Copas + 1 simbolo
                elementoEcra.Add(new ElementoJogo(Forma.Copas, CorAleatoria()));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas));
                break;
            case 6:
            case 7:
                // 6º e 7º: 1 Copas + 2 simbolos
                elementoEcra.Add(new ElementoJogo(Forma.Copas, CorAleatoria()));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas));
                break;
            case 8:
                // 8º: 1 Copas + 2 simbolos (3 da mesma cor)
                Cor corRonda8 = CorAleatoria();
                elementoEcra.Add(new ElementoJogo(Forma.Copas, corRonda8));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas, corRonda8));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas, corRonda8));
                break;
            case 9:
                // 9º: 1 Copas + 3 simbolos (4 da mesma cor)
                Cor corRonda9 = CorAleatoria();
                elementoEcra.Add(new ElementoJogo(Forma.Copas, corRonda9));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas, corRonda9));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas, corRonda9));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas, corRonda9));
                break;
            case 10:
                // 10º: 1 Copas + 3 simbolos
                elementoEcra.Add(new ElementoJogo(Forma.Copas, CorAleatoria()));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas));
                elementoEcra.Add(GerarSimboloAleatorio(formasUsadas));
                break;
        }
        //Embaralhar a lista de elementos no ecra para que a copas não seja sempre o primeiro
        return elementoEcra.OrderBy(x => random.Next()).ToList();
    }
    //Funções auxiliares para gerar cores e formas aleatórias
    private ElementoJogo GerarSimboloAleatorio(List<Forma> formasUsadas, Cor? corFixa = null)
    {
        //Pega em todas as formas possíveis, exceto Copas e as já usadas
        var formasDisponiveis = Enum.GetValues(typeof(Forma))
            .Cast<Forma>()
            .Where(f => f != Forma.Copas && !formasUsadas.Contains(f))
            .ToList();
        
        //Escolhe uma forma aleatória das disponíveis
        Forma formaEscolhida = formasDisponiveis[random.Next(formasDisponiveis.Count)];
        
        //Adiciona a forma escolhida à lista de formas usadas
        formasUsadas.Add(formaEscolhida);

        //Escolhe uma cor aleatória, ou usa a cor fixa se fornecida
        Cor corEscolhida = corFixa ?? CorAleatoria();

        return new ElementoJogo(formaEscolhida, corEscolhida);
    }

    private Cor CorAleatoria(Cor? corExcluida = null)
    {
        Array cores = Enum.GetValues(typeof(Cor));
        Cor corEscolhida;
        do
        {
            corEscolhida = (Cor)cores.GetValue(random.Next(cores.Length));
        } while (corExcluida.HasValue && corEscolhida == corExcluida.Value);
        return corEscolhida;
    }

    private Forma FormaAleatoria()
    {
        Array formas = Enum.GetValues(typeof(Forma));
        Forma formaEscolhida;
        do
        {
            formaEscolhida = (Forma)formas.GetValue(random.Next(formas.Length));
        } while (formaEscolhida == Forma.Copas); // Evitar gerar Copas aleatoriamente
        return formaEscolhida;
    }
}