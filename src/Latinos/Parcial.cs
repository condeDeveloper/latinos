namespace Conde.Latinos;

/// <summary>Um preenchimento parcial que a busca não conseguiu fechar.</summary>
public readonly record struct Impossivel(int Ordem, int Celulas, string Exemplo, long Total, long Travados);

/// <summary>
/// Quadrados latinos pela metade. A pergunta é se o que já está escrito deixa
/// terminar, e a resposta muda conforme o que está escrito for uma linha inteira
/// ou um punhado de células soltas.
/// </summary>
public static class Parcial
{
    /// <summary>
    /// Dá para completar? Busca simples, célula a célula, na primeira vaga que
    /// encontrar.
    /// </summary>
    public static bool PodeCompletar(Quadrado parcial)
    {
        ArgumentNullException.ThrowIfNull(parcial);
        if (!Juiz.EhLatinoParcial(parcial)) return false;
        return Completa(parcial.Copia(), 0);
    }

    /// <summary>A completação, ou null quando não existe.</summary>
    public static Quadrado? Completar(Quadrado parcial)
    {
        ArgumentNullException.ThrowIfNull(parcial);
        if (!Juiz.EhLatinoParcial(parcial)) return null;
        var copia = parcial.Copia();
        return Completa(copia, 0) ? copia : null;
    }

    private static bool Completa(Quadrado quadrado, int posicao)
    {
        var n = quadrado.Ordem;
        if (posicao == n * n) return true;

        var i = posicao / n;
        var j = posicao % n;
        if (quadrado[i, j] != Quadrado.Vazio) return Completa(quadrado, posicao + 1);

        for (var s = 0; s < n; s++)
        {
            if (!Contagem.Cabe(quadrado, i, j, s)) continue;
            quadrado[i, j] = s;
            if (Completa(quadrado, posicao + 1)) return true;
            quadrado[i, j] = Quadrado.Vazio;
        }

        quadrado[i, j] = Quadrado.Vazio;
        return false;
    }

    /// <summary>
    /// Todo retângulo latino de k linhas por n colunas completa num quadrado
    /// latino. É o teorema de Hall de 1945 aplicado às colunas, e aqui ele é
    /// conferido em todos os retângulos que existem, um por um.
    /// </summary>
    public static (long Retangulos, long Completaram) TodoRetanguloCompleta(int linhas, int ordem)
    {
        long total = 0, completaram = 0;
        var quadrado = new Quadrado(ordem);

        void Passo(int i, int j)
        {
            if (i == linhas)
            {
                total++;
                if (PodeCompletar(quadrado)) completaram++;
                return;
            }
            if (j == ordem) { Passo(i + 1, 0); return; }

            for (var s = 0; s < ordem; s++)
            {
                if (!Contagem.Cabe(quadrado, i, j, s)) continue;
                quadrado[i, j] = s;
                Passo(i, j + 1);
                quadrado[i, j] = Quadrado.Vazio;
            }
        }

        Passo(0, 0);
        return (total, completaram);
    }

    /// <summary>
    /// Varre todos os preenchimentos parciais com exatamente
    /// <paramref name="celulas"/> células, em ordem de posição, e conta quantos
    /// não completam. O primeiro número de células em que aparece um travado é a
    /// resposta de Evans para aquela ordem.
    /// </summary>
    public static Impossivel ComQuantasCelulasTrava(int ordem, int celulas)
    {
        if (ordem < 1) throw new ArgumentOutOfRangeException(nameof(ordem));
        if (celulas < 0 || celulas > ordem * ordem) throw new ArgumentOutOfRangeException(nameof(celulas));

        var quadrado = new Quadrado(ordem);
        long total = 0, travados = 0;
        string exemplo = string.Empty;

        void Passo(int posicao, int faltam)
        {
            if (faltam == 0)
            {
                total++;
                if (!PodeCompletar(quadrado))
                {
                    travados++;
                    if (exemplo.Length == 0) exemplo = quadrado.ToString();
                }
                return;
            }

            // Não adianta continuar se as posições que sobram não dão para o que falta.
            if (ordem * ordem - posicao < faltam) return;

            for (var p = posicao; p < ordem * ordem; p++)
            {
                var i = p / ordem;
                var j = p % ordem;
                for (var s = 0; s < ordem; s++)
                {
                    if (!Contagem.Cabe(quadrado, i, j, s)) continue;
                    quadrado[i, j] = s;
                    Passo(p + 1, faltam - 1);
                    quadrado[i, j] = Quadrado.Vazio;
                }
            }
        }

        Passo(0, celulas);
        return new Impossivel(ordem, celulas, exemplo, total, travados);
    }
}
