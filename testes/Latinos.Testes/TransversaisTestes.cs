using Conde.Latinos;
using Xunit;

namespace Conde.Latinos.Testes;

public class TransversaisTestes
{
    [Fact]
    public void OCiclicoDeOrdemParNaoTemTransversalNenhuma()
    {
        foreach (var n in new[] { 2, 4, 6, 8 })
            Assert.Empty(Transversais.Todas(Quadrado.Ciclico(n)));
    }

    [Theory]
    [InlineData(3, 3)]
    [InlineData(5, 15)]
    [InlineData(7, 133)]
    [InlineData(9, 2025)]
    public void OCiclicoDeOrdemImparTemTransversalEElasExplodem(int ordem, int quantas)
    {
        // Não são n: são 3, 15, 133, 2025. O cíclico de ordem ímpar é o quadrado
        // com mais transversais que existe, e não o que tem uma diagonal de sorte.
        Assert.Equal(quantas, Transversais.Quantas(Quadrado.Ciclico(ordem)));
    }

    [Fact]
    public void TodaTransversalAchadaPassaPeloJuiz()
    {
        foreach (var n in new[] { 3, 4, 5 })
            foreach (var quadrado in Contagem.Reduzidos(n))
                foreach (var t in Transversais.Todas(quadrado))
                    Assert.True(Juiz.EhTransversal(quadrado, t), quadrado.ToString());
    }

    [Fact]
    public void OCiclicoImparSeDecompoeEmTransversais()
    {
        Assert.True(Transversais.TemDecomposicao(Quadrado.Ciclico(3)));
        Assert.True(Transversais.TemDecomposicao(Quadrado.Ciclico(5)));
    }

    [Fact]
    public void OCiclicoParNaoSeDecompoe()
    {
        Assert.False(Transversais.TemDecomposicao(Quadrado.Ciclico(4)));
    }

    [Fact]
    public void ADecomposicaoViraUmParOrtogonalDeVerdade()
    {
        foreach (var n in new[] { 3, 5 })
        {
            var ciclico = Quadrado.Ciclico(n);
            var par = Transversais.ParOrtogonal(ciclico);
            Assert.NotNull(par);
            Assert.True(Juiz.EhLatino(par!));
            Assert.True(Juiz.SaoOrtogonais(ciclico, par!));
        }
    }

    [Fact]
    public void SemTransversalNaoHaParOrtogonal()
    {
        Assert.Null(Transversais.ParOrtogonal(Quadrado.Ciclico(4)));
        Assert.Null(Transversais.ParOrtogonal(Quadrado.Ciclico(2)));
    }

    [Fact]
    public void QuemTemParOrtogonalTemPeloMenosNTransversais()
    {
        foreach (var quadrado in Contagem.Reduzidos(5))
            if (Transversais.ParOrtogonal(quadrado) is not null)
                Assert.True(Transversais.Quantas(quadrado) >= 5, quadrado.ToString());
    }

    [Fact]
    public void OMaiorNumeroDeTransversaisPorOrdem()
    {
        Assert.Equal(3, Contagem.Todos(3).Max(Transversais.Quantas));
        Assert.Equal(8, Contagem.Todos(4).Max(Transversais.Quantas));
    }
}
