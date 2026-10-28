using Conde.Latinos;
using Xunit;

namespace Conde.Latinos.Testes;

public class QuadradoTestes
{
    [Fact]
    public void OQuadradoNasceVazio()
    {
        var q = new Quadrado(4);
        Assert.Equal(16, q.Vazias);
        Assert.Equal(Quadrado.Vazio, q[2, 3]);
    }

    [Fact]
    public void OQuadradoCiclicoEhATabelaDeSomas()
    {
        var q = Quadrado.Ciclico(3);
        Assert.Equal("0 1 2 / 1 2 0 / 2 0 1", q.ToString());
    }

    [Fact]
    public void ATranspostaTrocaLinhaPorColuna()
    {
        var q = new Quadrado(new[,] { { 0, 1 }, { 1, 0 } });
        Assert.Equal(q, q.Transposta());

        var outro = new Quadrado(new[,] { { 0, 1, 2 }, { 2, 0, 1 }, { 1, 2, 0 } });
        Assert.Equal("0 2 1 / 1 0 2 / 2 1 0", outro.Transposta().ToString());
    }

    [Fact]
    public void AReduzidaDeixaAPrimeiraLinhaEAPrimeiraColunaEmOrdem()
    {
        var q = new Quadrado(new[,] { { 2, 0, 1 }, { 1, 2, 0 }, { 0, 1, 2 } });
        var reduzida = q.Reduzida();
        Assert.True(Juiz.EhReduzido(reduzida));
    }

    [Fact]
    public void OQuadradoCiclicoJaEhReduzido()
    {
        for (var n = 1; n <= 6; n++)
            Assert.True(Juiz.EhReduzido(Quadrado.Ciclico(n)));
    }

    [Fact]
    public void QuadradoNaoQuadradoNaoExiste()
    {
        Assert.Throws<ArgumentException>(() => new Quadrado(new[,] { { 0, 1, 2 }, { 1, 2, 0 } }));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Quadrado(0));
    }

    [Fact]
    public void ACopiaNaoCompartilhaCelula()
    {
        var q = Quadrado.Ciclico(3);
        var copia = q.Copia();
        copia[0, 0] = 2;
        Assert.Equal(0, q[0, 0]);
        Assert.NotEqual(q, copia);
    }

    [Fact]
    public void DoisQuadradosIguaisTemOMesmoCodigo()
    {
        Assert.Equal(Quadrado.Ciclico(4), Quadrado.Ciclico(4));
        Assert.Equal(Quadrado.Ciclico(4).GetHashCode(), Quadrado.Ciclico(4).GetHashCode());
    }

    [Fact]
    public void CelulaVaziaApareceComoPontoNoTexto()
    {
        var q = new Quadrado(2);
        q[0, 0] = 1;
        Assert.Equal("1 . / . .", q.ToString());
    }
}
