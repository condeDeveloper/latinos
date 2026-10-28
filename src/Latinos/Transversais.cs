namespace Conde.Latinos;

/// <summary>
/// Transversais: n células, uma por linha, uma por coluna, com os n símbolos
/// todos diferentes. Quem tem transversal pode ser decomposto; quem não tem não
/// entra em par ortogonal nenhum, e por isso essa contagem decide mais coisa do
/// que parece.
/// </summary>
public static class Transversais
{
    /// <summary>Todas as transversais, cada uma como a coluna escolhida em cada linha.</summary>
    public static List<int[]> Todas(Quadrado quadrado)
    {
        ArgumentNullException.ThrowIfNull(quadrado);
        var n = quadrado.Ordem;
        var achadas = new List<int[]>();
        var escolha = new int[n];
        var colunaUsada = new bool[n];
        var simboloUsado = new bool[n];

        void Passo(int linha)
        {
            if (linha == n) { achadas.Add((int[])escolha.Clone()); return; }

            for (var j = 0; j < n; j++)
            {
                if (colunaUsada[j]) continue;
                var s = quadrado[linha, j];
                if (s == Quadrado.Vazio || simboloUsado[s]) continue;

                colunaUsada[j] = true;
                simboloUsado[s] = true;
                escolha[linha] = j;
                Passo(linha + 1);
                colunaUsada[j] = false;
                simboloUsado[s] = false;
            }
        }

        Passo(0);
        return achadas;
    }

    public static int Quantas(Quadrado quadrado) => Todas(quadrado).Count;

    /// <summary>
    /// Um quadrado se decompõe em transversais quando dá para cobrir as n² células
    /// com n transversais que não se cruzam. É exatamente o que um par ortogonal
    /// precisa, e é por isso que quem não tem transversal nenhuma está fora.
    /// </summary>
    public static bool TemDecomposicao(Quadrado quadrado)
    {
        ArgumentNullException.ThrowIfNull(quadrado);
        var n = quadrado.Ordem;
        var todas = Todas(quadrado);
        var usada = new bool[n, n];
        var escolhidas = 0;

        bool Passo(int indice)
        {
            if (escolhidas == n) return true;
            for (var k = indice; k < todas.Count; k++)
            {
                var t = todas[k];
                var livre = true;
                for (var i = 0; i < n && livre; i++)
                    if (usada[i, t[i]])
                        livre = false;
                if (!livre) continue;

                for (var i = 0; i < n; i++) usada[i, t[i]] = true;
                escolhidas++;
                if (Passo(k + 1)) return true;
                escolhidas--;
                for (var i = 0; i < n; i++) usada[i, t[i]] = false;
            }
            return false;
        }

        return Passo(0);
    }

    /// <summary>
    /// O quadrado ortogonal construído a partir de uma decomposição: cada
    /// transversal vira um símbolo. É a ponte entre transversal e par ortogonal,
    /// e devolve null quando a decomposição não existe.
    /// </summary>
    public static Quadrado? ParOrtogonal(Quadrado quadrado)
    {
        ArgumentNullException.ThrowIfNull(quadrado);
        var n = quadrado.Ordem;
        var todas = Todas(quadrado);
        var saida = new Quadrado(n);
        var usada = new bool[n, n];
        var quantas = 0;

        bool Passo(int indice)
        {
            if (quantas == n) return true;
            for (var k = indice; k < todas.Count; k++)
            {
                var t = todas[k];
                var livre = true;
                for (var i = 0; i < n && livre; i++)
                    if (usada[i, t[i]])
                        livre = false;
                if (!livre) continue;

                for (var i = 0; i < n; i++)
                {
                    usada[i, t[i]] = true;
                    saida[i, t[i]] = quantas;
                }
                quantas++;
                if (Passo(k + 1)) return true;
                quantas--;
                for (var i = 0; i < n; i++)
                {
                    usada[i, t[i]] = false;
                    saida[i, t[i]] = Quadrado.Vazio;
                }
            }
            return false;
        }

        return Passo(0) ? saida : null;
    }
}
