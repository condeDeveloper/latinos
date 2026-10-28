using System.Numerics;

namespace Conde.Latinos;

/// <summary>O que uma varredura custou e o que ela achou.</summary>
public readonly record struct Varredura(int Ordem, long Quantos, long Nos);

/// <summary>
/// Contar quadrados latinos. A varredura enumera os REDUZIDOS, que são os que têm
/// a primeira linha e a primeira coluna em ordem, e a conta n!(n-1)! devolve o
/// total. O caminho curto e o caminho longo são conferidos um contra o outro
/// enquanto a varredura completa ainda cabe na memória do dia.
/// </summary>
public static class Contagem
{
    /// <summary>Todos os quadrados latinos de uma ordem, um por um. Só para ordem pequena.</summary>
    public static IEnumerable<Quadrado> Todos(int ordem)
    {
        if (ordem < 1) throw new ArgumentOutOfRangeException(nameof(ordem));
        var quadrado = new Quadrado(ordem);
        foreach (var achado in Preenche(quadrado, 0, 0, _ => true))
            yield return achado;
    }

    /// <summary>Os quadrados latinos reduzidos de uma ordem.</summary>
    public static IEnumerable<Quadrado> Reduzidos(int ordem)
    {
        if (ordem < 1) throw new ArgumentOutOfRangeException(nameof(ordem));
        var quadrado = new Quadrado(ordem);
        for (var k = 0; k < ordem; k++)
        {
            quadrado[0, k] = k;
            quadrado[k, 0] = k;
        }

        foreach (var achado in Preenche(quadrado, 1, 1, _ => true))
            yield return achado;
    }

    /// <summary>
    /// Conta os reduzidos sem guardar nenhum, e devolve junto o número de nós
    /// abertos. O custo é o que separa a ordem 6 da ordem 7.
    /// </summary>
    public static Varredura ContaReduzidos(int ordem)
    {
        if (ordem < 1) throw new ArgumentOutOfRangeException(nameof(ordem));

        var quadrado = new Quadrado(ordem);
        for (var k = 0; k < ordem; k++)
        {
            quadrado[0, k] = k;
            quadrado[k, 0] = k;
        }

        long achados = 0, nos = 0;
        var naLinha = new bool[ordem, ordem];
        var naColuna = new bool[ordem, ordem];
        for (var k = 0; k < ordem; k++)
        {
            naLinha[0, k] = true;
            naColuna[0, k] = true;
            naLinha[k, k] = true;
            naColuna[k, k] = true;
        }

        void Passo(int i, int j)
        {
            if (i == ordem) { achados++; return; }
            if (j == ordem) { Passo(i + 1, 1); return; }

            for (var s = 0; s < ordem; s++)
            {
                nos++;
                if (naLinha[i, s] || naColuna[j, s]) continue;
                naLinha[i, s] = true;
                naColuna[j, s] = true;
                quadrado[i, j] = s;
                Passo(i, j + 1);
                naLinha[i, s] = false;
                naColuna[j, s] = false;
                quadrado[i, j] = Quadrado.Vazio;
            }
        }

        Passo(1, 1);
        return new Varredura(ordem, achados, nos);
    }

    /// <summary>
    /// O total a partir dos reduzidos: n! (n-1)! vezes a quantidade de reduzidos.
    /// Cada quadrado latino vira um reduzido renomeando símbolos e reordenando
    /// linhas, e cada reduzido volta a dar n!(n-1)! quadrados diferentes.
    /// </summary>
    public static BigInteger TotalPelosReduzidos(int ordem, long reduzidos) =>
        Fatorial(ordem) * Fatorial(ordem - 1) * reduzidos;

    public static BigInteger Fatorial(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        BigInteger saida = 1;
        for (var k = 2; k <= n; k++) saida *= k;
        return saida;
    }

    /// <summary>
    /// Retângulo latino: as primeiras k linhas de um quadrado latino de ordem n.
    /// Quantos existem, contados por varredura.
    /// </summary>
    public static long ContaRetangulos(int linhas, int ordem)
    {
        if (linhas < 1 || linhas > ordem) throw new ArgumentOutOfRangeException(nameof(linhas));

        var naLinha = new bool[ordem, ordem];
        var naColuna = new bool[ordem, ordem];
        long achados = 0;

        void Passo(int i, int j)
        {
            if (i == linhas) { achados++; return; }
            if (j == ordem) { Passo(i + 1, 0); return; }

            for (var s = 0; s < ordem; s++)
            {
                if (naLinha[i, s] || naColuna[j, s]) continue;
                naLinha[i, s] = true;
                naColuna[j, s] = true;
                Passo(i, j + 1);
                naLinha[i, s] = false;
                naColuna[j, s] = false;
            }
        }

        Passo(0, 0);
        return achados;
    }

    private static IEnumerable<Quadrado> Preenche(Quadrado quadrado, int linha, int coluna, Func<Quadrado, bool> aceita)
    {
        var n = quadrado.Ordem;
        if (linha == n)
        {
            if (aceita(quadrado)) yield return quadrado.Copia();
            yield break;
        }

        if (coluna == n)
        {
            foreach (var achado in Preenche(quadrado, linha + 1, 0, aceita)) yield return achado;
            yield break;
        }

        if (quadrado[linha, coluna] != Quadrado.Vazio)
        {
            foreach (var achado in Preenche(quadrado, linha, coluna + 1, aceita)) yield return achado;
            yield break;
        }

        for (var s = 0; s < n; s++)
        {
            if (!Cabe(quadrado, linha, coluna, s)) continue;
            quadrado[linha, coluna] = s;
            foreach (var achado in Preenche(quadrado, linha, coluna + 1, aceita)) yield return achado;
            quadrado[linha, coluna] = Quadrado.Vazio;
        }
    }

    internal static bool Cabe(Quadrado quadrado, int linha, int coluna, int simbolo)
    {
        var n = quadrado.Ordem;
        for (var k = 0; k < n; k++)
        {
            if (quadrado[linha, k] == simbolo) return false;
            if (quadrado[k, coluna] == simbolo) return false;
        }
        return true;
    }
}
