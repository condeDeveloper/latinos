using Conde.Latinos;
using Xunit;

namespace Conde.Latinos.Testes;

public class ContagemTestes
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 4)]
    [InlineData(5, 56)]
    [InlineData(6, 9408)]
    public void QuantosReduzidosExistem(int ordem, long quantos)
    {
        Assert.Equal(quantos, Contagem.ContaReduzidos(ordem).Quantos);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 12)]
    [InlineData(4, 576)]
    [InlineData(5, 161280)]
    public void AVarreduraDeTodosBateComAContaPelosReduzidos(int ordem, long total)
    {
        var varridos = Contagem.Todos(ordem).LongCount();
        Assert.Equal(total, varridos);

        var reduzidos = Contagem.ContaReduzidos(ordem).Quantos;
        Assert.Equal(total, Contagem.TotalPelosReduzidos(ordem, reduzidos));
    }

    [Fact]
    public void TodoQuadradoVarridoPassaPeloJuiz()
    {
        foreach (var ordem in new[] { 1, 2, 3, 4 })
            foreach (var quadrado in Contagem.Todos(ordem))
                Assert.True(Juiz.EhLatino(quadrado), quadrado.ToString());
    }

    [Fact]
    public void TodoReduzidoVarridoEhReduzidoDeVerdade()
    {
        foreach (var ordem in new[] { 2, 3, 4, 5 })
            foreach (var quadrado in Contagem.Reduzidos(ordem))
                Assert.True(Juiz.EhReduzido(quadrado), quadrado.ToString());
    }

    [Fact]
    public void AVarreduraNaoRepeteNenhumQuadrado()
    {
        foreach (var ordem in new[] { 2, 3, 4 })
        {
            var todos = Contagem.Todos(ordem).ToList();
            Assert.Equal(todos.Count, todos.Distinct().Count());
        }
    }

    [Fact]
    public void ReduzirQualquerQuadradoDaUmReduzido()
    {
        foreach (var quadrado in Contagem.Todos(4))
            Assert.True(Juiz.EhReduzido(quadrado.Reduzida()), quadrado.ToString());
    }

    [Theory]
    [InlineData(1, 3, 6)]
    [InlineData(2, 3, 12)]
    [InlineData(2, 4, 216)]
    [InlineData(3, 4, 576)]
    [InlineData(2, 5, 5280)]
    [InlineData(3, 5, 66240)]
    public void QuantosRetangulosLatinosExistem(int linhas, int ordem, long quantos)
    {
        Assert.Equal(quantos, Contagem.ContaRetangulos(linhas, ordem));
    }

    [Fact]
    public void ORetanguloDeNLinhasEhOQuadradoInteiro()
    {
        for (var n = 2; n <= 5; n++)
            Assert.Equal(Contagem.Todos(n).LongCount(), Contagem.ContaRetangulos(n, n));
    }

    [Fact]
    public void APrimeiraLinhaSozinhaDaFatorialDeN()
    {
        for (var n = 1; n <= 6; n++)
            Assert.Equal((long)Contagem.Fatorial(n), Contagem.ContaRetangulos(1, n));
    }

    [Fact]
    public void FatorialDeZeroEhUm()
    {
        Assert.Equal(1, Contagem.Fatorial(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Contagem.Fatorial(-1));
    }

    [Fact]
    public void OrdemZeroNaoEhPergunta()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Contagem.ContaReduzidos(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Contagem.Todos(0).ToList());
    }
}
