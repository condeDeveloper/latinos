using Conde.Latinos;
using Xunit;

namespace Conde.Latinos.Testes;

public class ParcialTestes
{
    [Fact]
    public void OQuadradoVazioCompleta()
    {
        for (var n = 1; n <= 5; n++)
            Assert.True(Parcial.PodeCompletar(new Quadrado(n)));
    }

    [Fact]
    public void OQuadradoJaProntoCompleta()
    {
        Assert.True(Parcial.PodeCompletar(Quadrado.Ciclico(5)));
    }

    [Fact]
    public void DuasCelulasQueTravamUmQuadradoDeDois()
    {
        // 0 no canto de cima e 1 no canto de baixo pedem 1 e 0 na outra diagonal,
        // e aí a coluna repete.
        var q = new Quadrado(2);
        q[0, 0] = 0;
        q[1, 1] = 0;
        Assert.True(Parcial.PodeCompletar(q));

        var trava = new Quadrado(2);
        trava[0, 0] = 0;
        trava[1, 1] = 1;
        Assert.False(Parcial.PodeCompletar(trava));
    }

    [Fact]
    public void AComplecaoDevolvidaEhLatinaEMantemOQueJaEstava()
    {
        var parcial = new Quadrado(5);
        parcial[0, 0] = 3;
        parcial[2, 4] = 1;
        var cheio = Parcial.Completar(parcial);
        Assert.NotNull(cheio);
        Assert.True(Juiz.EhLatino(cheio!));
        Assert.Equal(3, cheio![0, 0]);
        Assert.Equal(1, cheio[2, 4]);
    }

    [Fact]
    public void ParcialInvalidoNaoCompletaENaoDevolveNada()
    {
        var q = new Quadrado(3);
        q[0, 0] = 2;
        q[0, 1] = 2;
        Assert.False(Parcial.PodeCompletar(q));
        Assert.Null(Parcial.Completar(q));
    }

    [Theory]
    [InlineData(1, 3, 6)]
    [InlineData(2, 3, 12)]
    [InlineData(2, 4, 216)]
    [InlineData(3, 4, 576)]
    [InlineData(3, 5, 66240)]
    public void TodoRetanguloLatinoCompleta(int linhas, int ordem, long quantos)
    {
        var (total, completaram) = Parcial.TodoRetanguloCompleta(linhas, ordem);
        Assert.Equal(quantos, total);
        Assert.Equal(total, completaram);
    }

    [Theory]
    [InlineData(2, 8)]
    [InlineData(3, 270)]
    [InlineData(4, 25920)]
    public void ComUmaCelulaAMenosQueAOrdemNadaTrava(int ordem, long parciais)
    {
        var achado = Parcial.ComQuantasCelulasTrava(ordem, ordem - 1);
        Assert.Equal(parciais, achado.Total);
        Assert.Equal(0, achado.Travados);
    }

    [Theory]
    [InlineData(2, 16, 4)]
    [InlineData(3, 1278, 324)]
    public void ComCelulasIguaisAOrdemJaTrava(int ordem, long parciais, long travados)
    {
        var achado = Parcial.ComQuantasCelulasTrava(ordem, ordem);
        Assert.Equal(parciais, achado.Total);
        Assert.Equal(travados, achado.Travados);
        Assert.NotEqual(string.Empty, achado.Exemplo);
    }

    [Fact]
    public void OExemploQueTravaNaOrdemTresEhLatinoParcialDeVerdade()
    {
        var achado = Parcial.ComQuantasCelulasTrava(3, 3);
        Assert.Equal("0 1 . / . . 2 / . . .", achado.Exemplo);

        var q = new Quadrado(3);
        q[0, 0] = 0;
        q[0, 1] = 1;
        q[1, 2] = 2;
        Assert.True(Juiz.EhLatinoParcial(q));
        Assert.False(Parcial.PodeCompletar(q));
    }

    [Fact]
    public void CelulasDemaisOuDeMenosNaoSaoPergunta()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Parcial.ComQuantasCelulasTrava(3, 10));
        Assert.Throws<ArgumentOutOfRangeException>(() => Parcial.ComQuantasCelulasTrava(3, -1));
    }
}
