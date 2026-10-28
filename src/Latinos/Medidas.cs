using System.Numerics;

namespace Conde.Latinos;

/// <summary>
/// As medidas do README. Os testes conferem o que estes métodos devolvem; o
/// medidor só imprime.
/// </summary>
public static class Medidas
{
    public readonly record struct LinhaContagem(
        int Ordem, long Reduzidos, BigInteger Total, long Nos, long Varridos, bool Bate);

    public readonly record struct LinhaRetangulo(
        int Linhas, int Ordem, long Retangulos, long Completaram);

    public readonly record struct LinhaTransversal(
        int Ordem, int DoCiclico, bool CiclicoDecompoe, long Latinos, long SemNenhuma, int Maior);

    public readonly record struct LinhaEvans(
        int Ordem, int Celulas, long Parciais, long Travados, string Exemplo);

    public readonly record struct LinhaOrtogonal(
        int Ordem, long Reduzidos, long ComPar, long SemPar);

    /// <summary>
    /// A contagem dos reduzidos por varredura, o total pela conta n!(n-1)!, e a
    /// conferência contra a varredura de TODOS os quadrados enquanto ela couber.
    /// </summary>
    public static List<LinhaContagem> Contando(int ate = 6)
    {
        var linhas = new List<LinhaContagem>();
        for (var n = 1; n <= ate; n++)
        {
            var varredura = Contagem.ContaReduzidos(n);
            var total = Contagem.TotalPelosReduzidos(n, varredura.Quantos);

            long todos = -1;
            var bate = true;
            if (n <= 5)
            {
                todos = Contagem.Todos(n).LongCount();
                bate = total == todos;
            }

            linhas.Add(new LinhaContagem(n, varredura.Quantos, total, varredura.Nos, todos, bate));
        }
        return linhas;
    }

    /// <summary>
    /// A ordem 7 sozinha. Ela sai da tabela de cima porque sozinha custa mais do
    /// que todas as outras juntas, e porque o total já não cabe num long.
    /// </summary>
    public static LinhaContagem Sete()
    {
        var varredura = Contagem.ContaReduzidos(7);
        var total = Contagem.TotalPelosReduzidos(7, varredura.Quantos);
        return new LinhaContagem(7, varredura.Quantos, total, varredura.Nos, -1, true);
    }

    /// <summary>
    /// Todo retângulo latino completa. Aqui isso não é citado: é conferido em
    /// todos os retângulos de k linhas que existem.
    /// </summary>
    public static List<LinhaRetangulo> Retangulos()
    {
        var linhas = new List<LinhaRetangulo>();
        foreach (var (k, n) in new[] { (1, 3), (2, 3), (1, 4), (2, 4), (3, 4), (1, 5), (2, 5), (3, 5), (4, 5) })
        {
            var (total, completaram) = Parcial.TodoRetanguloCompleta(k, n);
            linhas.Add(new LinhaRetangulo(k, n, total, completaram));
        }
        return linhas;
    }

    /// <summary>
    /// Transversais do quadrado cíclico, que é o quadrado que qualquer um
    /// escreveria primeiro, e a varredura de todos os latinos à procura de quem
    /// não tem transversal nenhuma.
    /// </summary>
    public static List<LinhaTransversal> Transversal(int ate = 5)
    {
        var linhas = new List<LinhaTransversal>();
        for (var n = 2; n <= ate; n++)
        {
            var ciclico = Quadrado.Ciclico(n);
            var doCiclico = Transversais.Quantas(ciclico);
            var decompoe = doCiclico > 0 && Transversais.TemDecomposicao(ciclico);

            long latinos = 0, semNenhuma = 0;
            var maior = 0;
            foreach (var quadrado in Contagem.Todos(n))
            {
                latinos++;
                var quantas = Transversais.Quantas(quadrado);
                if (quantas == 0) semNenhuma++;
                if (quantas > maior) maior = quantas;
            }

            linhas.Add(new LinhaTransversal(n, doCiclico, decompoe, latinos, semNenhuma, maior));
        }
        return linhas;
    }

    /// <summary>
    /// O menor número de células soltas que trava a completação, por ordem, com um
    /// exemplo. Evans conjecturou em 1960 que n-1 células nunca travam, e o
    /// primeiro travamento aparece sempre em n células.
    /// </summary>
    public static List<LinhaEvans> Evans(int ate = 4)
    {
        var linhas = new List<LinhaEvans>();
        for (var n = 2; n <= ate; n++)
            for (var celulas = 1; celulas <= n; celulas++)
            {
                var achado = Parcial.ComQuantasCelulasTrava(n, celulas);
                if (achado.Travados > 0 || celulas == n)
                {
                    linhas.Add(new LinhaEvans(n, celulas, achado.Total, achado.Travados, achado.Exemplo));
                    break;
                }
            }
        return linhas;
    }

    /// <summary>
    /// Quantos quadrados reduzidos têm um par ortogonal. É a pergunta dos 36
    /// oficiais, feita quadrado por quadrado em vez de por teorema.
    /// </summary>
    public static List<LinhaOrtogonal> Ortogonais(int ate = 5)
    {
        var linhas = new List<LinhaOrtogonal>();
        for (var n = 2; n <= ate; n++)
        {
            long reduzidos = 0, comPar = 0;
            foreach (var quadrado in Contagem.Reduzidos(n))
            {
                reduzidos++;
                if (Transversais.ParOrtogonal(quadrado) is not null) comPar++;
            }
            linhas.Add(new LinhaOrtogonal(n, reduzidos, comPar, reduzidos - comPar));
        }
        return linhas;
    }

    /// <summary>
    /// O número de células que Evans pede, por ordem, conferido também pelo lado
    /// de baixo: com n-1 células nenhum preenchimento trava.
    /// </summary>
    public static (long Parciais, long Travados) ComUmaCelulaAMenos(int ordem) =>
        Parcial.ComQuantasCelulasTrava(ordem, ordem - 1) is var r ? (r.Total, r.Travados) : default;
}
