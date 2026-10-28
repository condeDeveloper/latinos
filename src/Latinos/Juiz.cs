namespace Conde.Latinos;

/// <summary>
/// A definição, e nada além dela: um quadrado é latino quando cada símbolo
/// aparece uma vez em cada linha e uma vez em cada coluna. O juiz não sabe contar
/// quadrados, não sabe reduzir e não sabe o que é transversal.
/// </summary>
public static class Juiz
{
    public static bool EhLatino(Quadrado quadrado)
    {
        ArgumentNullException.ThrowIfNull(quadrado);
        var n = quadrado.Ordem;

        for (var i = 0; i < n; i++)
        {
            var naLinha = new bool[n];
            var naColuna = new bool[n];
            for (var j = 0; j < n; j++)
            {
                var a = quadrado[i, j];
                var b = quadrado[j, i];
                if (a is Quadrado.Vazio || b is Quadrado.Vazio) return false;
                if (a < 0 || a >= n || b < 0 || b >= n) return false;
                if (naLinha[a] || naColuna[b]) return false;
                naLinha[a] = true;
                naColuna[b] = true;
            }
        }

        return true;
    }

    /// <summary>
    /// Latino parcial: o que está preenchido não se repete em linha nem em coluna,
    /// e o resto pode estar vazio. É o estado em que toda busca passa.
    /// </summary>
    public static bool EhLatinoParcial(Quadrado quadrado)
    {
        ArgumentNullException.ThrowIfNull(quadrado);
        var n = quadrado.Ordem;

        for (var i = 0; i < n; i++)
        {
            var naLinha = new bool[n];
            var naColuna = new bool[n];
            for (var j = 0; j < n; j++)
            {
                var a = quadrado[i, j];
                if (a != Quadrado.Vazio)
                {
                    if (a < 0 || a >= n || naLinha[a]) return false;
                    naLinha[a] = true;
                }

                var b = quadrado[j, i];
                if (b != Quadrado.Vazio)
                {
                    if (b < 0 || b >= n || naColuna[b]) return false;
                    naColuna[b] = true;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Reduzido: primeira linha e primeira coluna em ordem. Toda contagem deste
    /// repositório passa por aqui, porque é o reduzido que a varredura enumera.
    /// </summary>
    public static bool EhReduzido(Quadrado quadrado)
    {
        if (!EhLatino(quadrado)) return false;
        for (var k = 0; k < quadrado.Ordem; k++)
            if (quadrado[0, k] != k || quadrado[k, 0] != k)
                return false;
        return true;
    }

    /// <summary>
    /// Ortogonais: sobrepondo os dois quadrados, os n² pares (a,b) aparecem todos,
    /// cada um uma vez. É a pergunta dos 36 oficiais de Euler.
    /// </summary>
    public static bool SaoOrtogonais(Quadrado a, Quadrado b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);
        if (a.Ordem != b.Ordem) throw new ArgumentException("ordens diferentes");
        if (!EhLatino(a) || !EhLatino(b)) return false;

        var n = a.Ordem;
        var visto = new bool[n * n];
        for (var i = 0; i < n; i++)
            for (var j = 0; j < n; j++)
            {
                var par = a[i, j] * n + b[i, j];
                if (visto[par]) return false;
                visto[par] = true;
            }

        return true;
    }

    /// <summary>
    /// Transversal: n células, uma em cada linha, uma em cada coluna, com os n
    /// símbolos diferentes. Dito à mão: uma diagonal torta que vê tudo.
    /// </summary>
    public static bool EhTransversal(Quadrado quadrado, IReadOnlyList<int> colunaDaLinha)
    {
        ArgumentNullException.ThrowIfNull(quadrado);
        ArgumentNullException.ThrowIfNull(colunaDaLinha);
        var n = quadrado.Ordem;
        if (colunaDaLinha.Count != n) return false;

        var coluna = new bool[n];
        var simbolo = new bool[n];
        for (var i = 0; i < n; i++)
        {
            var j = colunaDaLinha[i];
            if (j < 0 || j >= n || coluna[j]) return false;
            coluna[j] = true;

            var s = quadrado[i, j];
            if (s == Quadrado.Vazio || simbolo[s]) return false;
            simbolo[s] = true;
        }

        return true;
    }
}
