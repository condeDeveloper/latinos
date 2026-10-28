using Conde.Latinos;

var comando = args.Length > 0 ? args[0] : "tudo";

if (comando is "contagem" or "tudo") Contando();
if (comando is "sete" or "tudo") Sete();
if (comando is "retangulos" or "tudo") Retangulos();
if (comando is "transversais" or "tudo") Transversais();
if (comando is "evans" or "tudo") Evans();
if (comando is "ortogonais" or "tudo") Ortogonais();

static void Contando()
{
    Console.WriteLine();
    Console.WriteLine("QUANTOS QUADRADOS LATINOS EXISTEM. A varredura enumera os reduzidos; a conta faz o resto.");
    Console.WriteLine();
    Console.WriteLine($"  {"ordem",6} {"reduzidos",12} {"total",16} {"nos",14} {"varrido um a um",17} {"bate",6}");

    foreach (var l in Medidas.Contando())
        Console.WriteLine($"  {l.Ordem,6} {l.Reduzidos,12:n0} {l.Total,16:n0} {l.Nos,14:n0} {(l.Varridos < 0 ? "-" : l.Varridos.ToString("n0")),17} {(l.Bate ? "sim" : "NAO"),6}");

    Console.WriteLine();
}

static void Sete()
{
    Console.WriteLine();
    Console.WriteLine("A ORDEM 7 SOZINHA: a varredura que ja nao cabe na tabela de cima.");
    Console.WriteLine();
    var l = Medidas.Sete();
    Console.WriteLine($"  reduzidos  {l.Reduzidos,16:n0}");
    Console.WriteLine($"  total      {l.Total,16:n0}");
    Console.WriteLine($"  nos        {l.Nos,16:n0}");
    Console.WriteLine();
}

static void Retangulos()
{
    Console.WriteLine();
    Console.WriteLine("TODO RETANGULO LATINO COMPLETA. Conferido em todos os que existem.");
    Console.WriteLine();
    Console.WriteLine($"  {"linhas",7} {"ordem",6} {"retangulos",12} {"completaram",12} {"falharam",9}");

    foreach (var l in Medidas.Retangulos())
        Console.WriteLine($"  {l.Linhas,7} {l.Ordem,6} {l.Retangulos,12:n0} {l.Completaram,12:n0} {l.Retangulos - l.Completaram,9:n0}");

    Console.WriteLine();
}

static void Transversais()
{
    Console.WriteLine();
    Console.WriteLine("TRANSVERSAIS: uma celula por linha, uma por coluna, os n simbolos.");
    Console.WriteLine();
    Console.WriteLine($"  {"ordem",6} {"no ciclico",11} {"decompoe",9} {"latinos",10} {"sem nenhuma",12} {"maior",7}");

    foreach (var l in Medidas.Transversal())
        Console.WriteLine($"  {l.Ordem,6} {l.DoCiclico,11:n0} {(l.CiclicoDecompoe ? "sim" : "nao"),9} {l.Latinos,10:n0} {l.SemNenhuma,12:n0} {l.Maior,7:n0}");

    Console.WriteLine();
}

static void Evans()
{
    Console.WriteLine();
    Console.WriteLine("QUANTAS CELULAS SOLTAS TRAVAM a completacao. Sempre n, nunca n-1.");
    Console.WriteLine();
    Console.WriteLine($"  {"ordem",6} {"celulas",8} {"parciais",10} {"travados",9}  exemplo");

    foreach (var l in Medidas.Evans())
        Console.WriteLine($"  {l.Ordem,6} {l.Celulas,8} {l.Parciais,10:n0} {l.Travados,9:n0}  {l.Exemplo}");

    Console.WriteLine();
    foreach (var ordem in new[] { 2, 3, 4 })
    {
        var (parciais, travados) = Medidas.ComUmaCelulaAMenos(ordem);
        Console.WriteLine($"  ordem {ordem} com {ordem - 1} celulas: {parciais,10:n0} parciais, {travados} travados");
    }
    Console.WriteLine();
}

static void Ortogonais()
{
    Console.WriteLine();
    Console.WriteLine("QUEM TEM PAR ORTOGONAL, quadrado reduzido por quadrado reduzido.");
    Console.WriteLine();
    Console.WriteLine($"  {"ordem",6} {"reduzidos",11} {"com par",9} {"sem par",9}");

    foreach (var l in Medidas.Ortogonais())
        Console.WriteLine($"  {l.Ordem,6} {l.Reduzidos,11:n0} {l.ComPar,9:n0} {l.SemPar,9:n0}");

    Console.WriteLine();
}
