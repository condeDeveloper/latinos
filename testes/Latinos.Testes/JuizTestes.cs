using Conde.Latinos;
using Xunit;

namespace Conde.Latinos.Testes;

public class JuizTestes
{
    [Fact]
    public void OCiclicoEhLatinoEmQualquerOrdem()
    {
        for (var n = 1; n <= 8; n++)
            Assert.True(Juiz.EhLatino(Quadrado.Ciclico(n)));
    }

    [Fact]
    public void SimboloRepetidoNaLinhaReprova()
    {
        Assert.False(Juiz.EhLatino(new Quadrado(new[,] { { 0, 0 }, { 1, 1 } })));
    }

    [Fact]
    public void SimboloRepetidoNaColunaReprova()
    {
        Assert.False(Juiz.EhLatino(new Quadrado(new[,] { { 0, 1 }, { 0, 1 } })));
    }

    [Fact]
    public void QuadradoComCelulaVaziaNaoEhLatinoMasPodeSerParcial()
    {
        var q = new Quadrado(3);
        q[0, 0] = 0;
        Assert.False(Juiz.EhLatino(q));
        Assert.True(Juiz.EhLatinoParcial(q));
    }

    [Fact]
    public void ParcialComConflitoReprova()
    {
        var q = new Quadrado(3);
        q[0, 0] = 1;
        q[0, 2] = 1;
        Assert.False(Juiz.EhLatinoParcial(q));
    }

    [Fact]
    public void SimboloForaDoAlfabetoReprova()
    {
        var q = new Quadrado(new[,] { { 0, 5 }, { 1, 0 } });
        Assert.False(Juiz.EhLatino(q));
    }

    [Fact]
    public void ReduzidoPedeAPrimeiraLinhaEAColunaEmOrdem()
    {
        Assert.True(Juiz.EhReduzido(new Quadrado(new[,] { { 0, 1 }, { 1, 0 } })));
        Assert.False(Juiz.EhReduzido(new Quadrado(new[,] { { 1, 0 }, { 0, 1 } })));
    }

    [Fact]
    public void OParOrtogonalDeOrdemTresEhOClassico()
    {
        var a = new Quadrado(new[,] { { 0, 1, 2 }, { 1, 2, 0 }, { 2, 0, 1 } });
        var b = new Quadrado(new[,] { { 0, 1, 2 }, { 2, 0, 1 }, { 1, 2, 0 } });
        Assert.True(Juiz.SaoOrtogonais(a, b));
    }

    [Fact]
    public void UmQuadradoNaoEhOrtogonalASiMesmoQuandoOrdemMaiorQueUm()
    {
        var a = Quadrado.Ciclico(3);
        Assert.False(Juiz.SaoOrtogonais(a, a));
    }

    [Fact]
    public void OrdensDiferentesNaoSeComparam()
    {
        Assert.Throws<ArgumentException>(() => Juiz.SaoOrtogonais(Quadrado.Ciclico(3), Quadrado.Ciclico(4)));
    }

    [Fact]
    public void ADiagonalDoCiclicoDeOrdemImparEhTransversal()
    {
        // No cíclico de ordem 3 a diagonal principal tem 0, 2, 1: os três símbolos.
        Assert.True(Juiz.EhTransversal(Quadrado.Ciclico(3), new[] { 0, 1, 2 }));
    }

    [Fact]
    public void ADiagonalDoCiclicoDeOrdemParNaoEh()
    {
        // Em ordem 4 a diagonal tem 0, 2, 0, 2.
        Assert.False(Juiz.EhTransversal(Quadrado.Ciclico(4), new[] { 0, 1, 2, 3 }));
    }

    [Fact]
    public void TransversalPrecisaDeUmaColunaPorLinha()
    {
        Assert.False(Juiz.EhTransversal(Quadrado.Ciclico(3), new[] { 0, 0, 1 }));
        Assert.False(Juiz.EhTransversal(Quadrado.Ciclico(3), new[] { 0, 1 }));
    }
}
