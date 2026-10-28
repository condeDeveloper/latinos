namespace Conde.Latinos;

/// <summary>
/// Um quadrado n por n com símbolos de 0 a n-1. A classe não exige que ele seja
/// latino: quem decide isso é o <see cref="Juiz"/>, e é de propósito, porque
/// metade das perguntas deste repositório é sobre quadrados que ainda não são
/// latinos ou que nunca vão ser.
/// </summary>
public sealed class Quadrado : IEquatable<Quadrado>
{
    public const int Vazio = -1;

    private readonly int[] _celulas;

    public Quadrado(int ordem)
    {
        if (ordem < 1) throw new ArgumentOutOfRangeException(nameof(ordem));
        Ordem = ordem;
        _celulas = new int[ordem * ordem];
        Array.Fill(_celulas, Vazio);
    }

    public Quadrado(int[,] celulas)
    {
        ArgumentNullException.ThrowIfNull(celulas);
        if (celulas.GetLength(0) != celulas.GetLength(1))
            throw new ArgumentException("o quadrado precisa ser quadrado", nameof(celulas));

        Ordem = celulas.GetLength(0);
        _celulas = new int[Ordem * Ordem];
        for (var i = 0; i < Ordem; i++)
            for (var j = 0; j < Ordem; j++)
                _celulas[i * Ordem + j] = celulas[i, j];
    }

    private Quadrado(int ordem, int[] celulas)
    {
        Ordem = ordem;
        _celulas = celulas;
    }

    public int Ordem { get; }

    public int this[int linha, int coluna]
    {
        get => _celulas[linha * Ordem + coluna];
        set => _celulas[linha * Ordem + coluna] = value;
    }

    /// <summary>Quantas células ainda não têm símbolo.</summary>
    public int Vazias => _celulas.Count(c => c == Vazio);

    public Quadrado Copia() => new(Ordem, (int[])_celulas.Clone());

    /// <summary>
    /// A transposta. Num quadrado latino ela também é latina: trocar o papel de
    /// linha e coluna é um dos seis conjugados.
    /// </summary>
    public Quadrado Transposta()
    {
        var saida = new Quadrado(Ordem);
        for (var i = 0; i < Ordem; i++)
            for (var j = 0; j < Ordem; j++)
                saida[j, i] = this[i, j];
        return saida;
    }

    /// <summary>
    /// A forma reduzida: renomeia os símbolos para que a primeira linha fique em
    /// ordem e depois ordena as linhas pela primeira coluna. Dois quadrados que só
    /// diferem no nome dos símbolos e na ordem das linhas viram o mesmo.
    /// </summary>
    public Quadrado Reduzida()
    {
        var nome = new int[Ordem];
        for (var j = 0; j < Ordem; j++) nome[this[0, j]] = j;

        var linhas = new List<int[]>();
        for (var i = 0; i < Ordem; i++)
        {
            var linha = new int[Ordem];
            for (var j = 0; j < Ordem; j++) linha[j] = nome[this[i, j]];
            linhas.Add(linha);
        }

        linhas.Sort((a, b) => a[0].CompareTo(b[0]));

        var saida = new Quadrado(Ordem);
        for (var i = 0; i < Ordem; i++)
            for (var j = 0; j < Ordem; j++)
                saida[i, j] = linhas[i][j];
        return saida;
    }

    /// <summary>
    /// O quadrado cíclico: a tabela de somas de Z_n. É o quadrado latino que
    /// qualquer um escreveria primeiro, e é por isso que vale olhar para ele com
    /// atenção.
    /// </summary>
    public static Quadrado Ciclico(int ordem)
    {
        var saida = new Quadrado(ordem);
        for (var i = 0; i < ordem; i++)
            for (var j = 0; j < ordem; j++)
                saida[i, j] = (i + j) % ordem;
        return saida;
    }

    public bool Equals(Quadrado? outro) =>
        outro is not null && outro.Ordem == Ordem && _celulas.AsSpan().SequenceEqual(outro._celulas);

    public override bool Equals(object? obj) => Equals(obj as Quadrado);

    public override int GetHashCode()
    {
        var h = new HashCode();
        h.Add(Ordem);
        foreach (var c in _celulas) h.Add(c);
        return h.ToHashCode();
    }

    public override string ToString()
    {
        var linhas = new string[Ordem];
        for (var i = 0; i < Ordem; i++)
        {
            var celulas = new string[Ordem];
            for (var j = 0; j < Ordem; j++)
                celulas[j] = this[i, j] == Vazio ? "." : this[i, j].ToString();
            linhas[i] = string.Join(" ", celulas);
        }
        return string.Join(" / ", linhas);
    }
}
