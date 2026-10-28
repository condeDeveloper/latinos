using System.Numerics;
using Conde.Latinos;
using Xunit;

namespace Conde.Latinos.Testes;

/// <summary>Todo número do README está preso aqui.</summary>
public class MedidasTestes
{
    [Theory]
    [InlineData(1, 1L, "1", 0L)]
    [InlineData(2, 1L, "2", 2L)]
    [InlineData(3, 1L, "12", 15L)]
    [InlineData(4, 4L, "576", 136L)]
    [InlineData(5, 56L, "161280", 3_965L)]
    [InlineData(6, 9_408L, "812851200", 981_042L)]
    public void CadaLinhaDaContagem(int ordem, long reduzidos, string total, long nos)
    {
        var linha = Medidas.Contando().Single(l => l.Ordem == ordem);
        Assert.Equal(reduzidos, linha.Reduzidos);
        Assert.Equal(BigInteger.Parse(total), linha.Total);
        Assert.Equal(nos, linha.Nos);
        Assert.True(linha.Bate);
    }

    [Fact]
    public void AVarreduraUmAUmConfereAContaAteAOrdemCinco()
    {
        foreach (var linha in Medidas.Contando().Where(l => l.Ordem <= 5))
        {
            Assert.Equal(linha.Total, linha.Varridos);
            Assert.True(linha.Bate);
        }
    }

    [Fact]
    public void AOrdemSeteJaNaoCabeNaTabela()
    {
        var sete = Medidas.Sete();
        Assert.Equal(16_942_080L, sete.Reduzidos);
        Assert.Equal(BigInteger.Parse("61479419904000"), sete.Total);
        Assert.Equal(2_719_886_890L, sete.Nos);
    }

    [Fact]
    public void AOrdemSeteCustaMaisNosQueTodasAsAnterioresJuntas()
    {
        var anteriores = Medidas.Contando().Sum(l => l.Nos);
        Assert.Equal(985_160L, anteriores);
        Assert.True(Medidas.Sete().Nos > 2_000L * anteriores);
    }

    [Theory]
    [InlineData(1, 3, 6L)]
    [InlineData(2, 3, 12L)]
    [InlineData(1, 4, 24L)]
    [InlineData(2, 4, 216L)]
    [InlineData(3, 4, 576L)]
    [InlineData(1, 5, 120L)]
    [InlineData(2, 5, 5_280L)]
    [InlineData(3, 5, 66_240L)]
    [InlineData(4, 5, 161_280L)]
    public void CadaLinhaDosRetangulos(int linhas, int ordem, long retangulos)
    {
        var linha = Medidas.Retangulos().Single(l => l.Linhas == linhas && l.Ordem == ordem);
        Assert.Equal(retangulos, linha.Retangulos);
        Assert.Equal(retangulos, linha.Completaram);
    }

    [Fact]
    public void NenhumRetanguloLatinoFalhaEmCompletar()
    {
        foreach (var linha in Medidas.Retangulos())
            Assert.Equal(0, linha.Retangulos - linha.Completaram);
    }

    [Theory]
    [InlineData(2, 0, false, 2L, 2L, 0)]
    [InlineData(3, 3, true, 12L, 0L, 3)]
    [InlineData(4, 0, false, 576L, 432L, 8)]
    [InlineData(5, 15, true, 161_280L, 0L, 15)]
    public void CadaLinhaDasTransversais(
        int ordem, int doCiclico, bool decompoe, long latinos, long semNenhuma, int maior)
    {
        var linha = Medidas.Transversal().Single(l => l.Ordem == ordem);
        Assert.Equal(doCiclico, linha.DoCiclico);
        Assert.Equal(decompoe, linha.CiclicoDecompoe);
        Assert.Equal(latinos, linha.Latinos);
        Assert.Equal(semNenhuma, linha.SemNenhuma);
        Assert.Equal(maior, linha.Maior);
    }

    [Fact]
    public void EmOrdemImparTodoQuadradoTemTransversal()
    {
        foreach (var linha in Medidas.Transversal().Where(l => l.Ordem % 2 == 1))
            Assert.Equal(0, linha.SemNenhuma);
    }

    [Fact]
    public void TresQuartosDosQuadradosDeOrdemQuatroNaoTemTransversal()
    {
        var linha = Medidas.Transversal().Single(l => l.Ordem == 4);
        Assert.Equal(0.75, (double)linha.SemNenhuma / linha.Latinos, 3);
    }

    [Theory]
    [InlineData(2, 2, 16L, 4L)]
    [InlineData(3, 3, 1_278L, 324L)]
    [InlineData(4, 4, 239_760L, 13_824L)]
    public void CadaLinhaDeEvans(int ordem, int celulas, long parciais, long travados)
    {
        var linha = Medidas.Evans().Single(l => l.Ordem == ordem);
        Assert.Equal(celulas, linha.Celulas);
        Assert.Equal(parciais, linha.Parciais);
        Assert.Equal(travados, linha.Travados);
    }

    [Fact]
    public void OPrimeiroTravamentoAconteceSempreEmNCelulas()
    {
        foreach (var linha in Medidas.Evans())
        {
            Assert.Equal(linha.Ordem, linha.Celulas);
            Assert.True(linha.Travados > 0);
        }
    }

    [Theory]
    [InlineData(2, 8L)]
    [InlineData(3, 270L)]
    [InlineData(4, 25_920L)]
    public void ComUmaCelulaAMenosNadaTrava(int ordem, long parciais)
    {
        var (total, travados) = Medidas.ComUmaCelulaAMenos(ordem);
        Assert.Equal(parciais, total);
        Assert.Equal(0, travados);
    }

    [Theory]
    [InlineData(2, 1L, 0L, 1L)]
    [InlineData(3, 1L, 1L, 0L)]
    [InlineData(4, 4L, 1L, 3L)]
    [InlineData(5, 56L, 6L, 50L)]
    public void CadaLinhaDosOrtogonais(int ordem, long reduzidos, long comPar, long semPar)
    {
        var linha = Medidas.Ortogonais().Single(l => l.Ordem == ordem);
        Assert.Equal(reduzidos, linha.Reduzidos);
        Assert.Equal(comPar, linha.ComPar);
        Assert.Equal(semPar, linha.SemPar);
    }

    [Fact]
    public void TerParOrtogonalEhRaro()
    {
        var cinco = Medidas.Ortogonais().Single(l => l.Ordem == 5);
        Assert.Equal(0.107, (double)cinco.ComPar / cinco.Reduzidos, 3);
    }
}
