# Quadrados latinos

Quadrados latinos do zero em C# e .NET 8: cada símbolo uma vez em cada linha e uma
vez em cada coluna. É o sudoku sem as caixinhas, a tabela de um torneio em que
ninguém joga duas vezes no mesmo horário, e o desenho de um experimento agrícola
em que cada adubo tem de cair uma vez em cada faixa de terra.

A pergunta que organiza o repositório é **quantos existem**, e a resposta é
varrida. O que a varredura encontra pelo caminho é mais interessante que a
contagem: quadrado sem transversal é a maioria, par ortogonal é raro, e três
células soltas bastam para travar um quadrado de ordem 3.

```bash
dotnet test testes/Latinos.Testes/Latinos.Testes.csproj -c Release    # 113 testes
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release    # todas as tabelas
```

## Quantos existem

```
   ordem    reduzidos            total            nos   varrido um a um   bate
       1            1                1              0                 1    sim
       2            1                2              2                 2    sim
       3            1               12             15                12    sim
       4            4              576            136               576    sim
       5           56          161,280          3,965           161,280    sim
       6        9,408      812,851,200        981,042                 -    sim
```

A varredura enumera os **reduzidos**, que são os quadrados com a primeira linha e a
primeira coluna em ordem, e a conta `n!(n-1)!` devolve o total: renomear os símbolos
dá n! quadrados e reordenar as linhas que sobram dá (n-1)!.

A coluna `varrido um a um` é a conferência que importa. Até a ordem 5 a varredura
monta **todos** os quadrados latinos, sem reduzir nada, e conta um por um: 12, 576,
161.280. Os números batem com a conta. Da ordem 6 em diante essa conferência deixa
de caber, e é por isso que ela é feita enquanto cabe. **Conta e varredura não são
duas confirmações da mesma coisa: a conta assume que a redução é uma bijeção, e a
varredura não assume nada.**

A ordem 7 sai numa tabela própria porque sozinha custa 2.700 vezes mais que todas
as anteriores juntas:

```
  reduzidos        16,942,080
  total      61,479,419,904,000
  nos            2,719,886,890
```

Sessenta e um trilhões de quadrados latinos de ordem 7. Com 11 símbolos o número já
tem 51 dígitos, e ninguém jamais vai varrer um desses.

## Todo retângulo latino completa

```
   linhas  ordem   retangulos  completaram  falharam
        1      3            6            6         0
        2      3           12           12         0
        1      4           24           24         0
        2      4          216          216         0
        3      4          576          576         0
        1      5          120          120         0
        2      5        5,280        5,280         0
        3      5       66,240       66,240         0
        4      5      161,280      161,280         0
```

Um retângulo latino é um pedaço de quadrado: k linhas cheias, as de baixo vazias.
Em 1945 Hall provou que esse pedaço **sempre** dá para terminar, aplicando o teorema
dos casamentos às colunas. Aqui o teorema não é citado: todos os retângulos que
existem são montados e completados, um por um, e a coluna `falharam` é zero em
todas as linhas.

## Três células soltas travam um quadrado de ordem 3

```
   ordem  celulas   parciais  travados  exemplo
       2        2         16         4  0 . / . 1
       3        3      1,278       324  0 1 . / . . 2 / . . .
       4        4    239,760    13,824  0 1 2 . / . . . 3 / . . . . / . . . .

  ordem 2 com 1 celula:         8 parciais, 0 travados
  ordem 3 com 2 celulas:      270 parciais, 0 travados
  ordem 4 com 3 celulas:   25,920 parciais, 0 travados
```

A linha de cima e a de baixo contam a mesma história de dois lados. **Com n-1
células soltas nada trava**, em nenhuma ordem, em nenhum arranjo: são 8, 270 e
25.920 preenchimentos conferidos um a um, todos completáveis. **Com n células o
travamento aparece**, e é o que Evans conjecturou em 1960.

O que a varredura acrescentou ao enunciado é que o travamento **não é exceção**.
Em ordem 3, 324 dos 1.278 preenchimentos de três células não completam: um em cada
quatro. O exemplo mínimo cabe numa linha: `0 1 .` em cima e um `2` no meio da
segunda linha. O 2 na última coluna da linha de baixo não tem onde ir, e nenhuma
escolha anterior pode ser desfeita, porque não houve escolha anterior.

Repare como o travamento é diferente do retângulo latino: o retângulo tem **mais**
células preenchidas e completa sempre. **Não é a quantidade de informação que
atrapalha, é a forma dela.**

## Transversal é o que falta na maioria

Uma transversal é uma diagonal torta: n células, uma por linha, uma por coluna, com
os n símbolos diferentes.

```
   ordem  no ciclico  decompoe    latinos  sem nenhuma   maior
       2           0       nao          2            2       0
       3           3       sim         12            0       3
       4           0       nao        576          432       8
       5          15       sim    161,280            0      15
```

O quadrado cíclico, a tabela de somas de Z_n, é o primeiro que qualquer um escreve.
**Em ordem par ele não tem transversal nenhuma**, e a razão cabe numa linha: a soma
dos símbolos de uma transversal teria de ser a soma de todos os símbolos, e em Z_n
com n par essas duas somas diferem por n/2.

A surpresa está na coluna `sem nenhuma`. Em ordem 4, **432 dos 576 quadrados
latinos não têm transversal alguma**: três quartos. O cíclico não é um caso
patológico de livro, é a maioria silenciosa. Já em ordem ímpar o zero é absoluto:
todos os 12 de ordem 3 e todos os 161.280 de ordem 5 têm transversal, que é o que
Ryser conjecturou em 1967 e ninguém provou até hoje.

E quando tem, tem muita: o cíclico de ordem ímpar sai com **3, 15, 133 e 2025**
transversais em ordem 3, 5, 7 e 9. Não são n, uma por deslocamento da diagonal:
são todas as diagonais tortas que existem, e elas explodem.

## Par ortogonal é raro

```
   ordem   reduzidos   com par   sem par
       2           1         0         1
       3           1         1         0
       4           4         1         3
       5          56         6        50
```

Dois quadrados são ortogonais quando, sobrepostos, os n² pares de símbolos aparecem
todos, cada um uma vez. É o que Euler pediu em 1782 com os 36 oficiais: seis
regimentos, seis patentes, um quadrado 6 por 6 em que cada combinação aparece uma
vez.

Um quadrado só entra num par ortogonal se puder ser fatiado em n transversais que
não se cruzam, e é assim que o par é construído aqui: cada transversal da
decomposição vira um símbolo do companheiro. Como quase ninguém tem transversal, **o
par ortogonal é raro**: 1 dos 4 reduzidos de ordem 4 e 6 dos 56 de ordem 5, 10,7%.

## O que as medidas me corrigiram

**Achei que o cíclico de ordem ímpar tivesse n transversais.** A conta parecia
óbvia: a diagonal principal funciona, e deslocando-a de uma em uma coluna dão n
diagonais. Escrevi o teste com `Assert.Equal(n, ...)` e ele quebrou no 5, que tem
15. Depois quebraria pior: 133 em ordem 7 e 2.025 em ordem 9. As diagonais
deslocadas são transversais, sim, mas são só as que eu consegui imaginar. A
varredura conta **todas as diagonais tortas**, e "as que eu imagino" não é um
subconjunto honesto de "as que existem".

**Esperava que quadrado sem transversal fosse a exceção.** O cíclico de ordem par é
o exemplo que todo texto dá, com cara de caso patológico escolhido a dedo. Em
ordem 4, 432 dos 576 não têm nenhuma. O patológico é ter.

**Chamei de "a mesma conferência" duas coisas diferentes.** Ia escrever que a
varredura dos reduzidos confirma a conta `n!(n-1)!`. Não confirma: a varredura dos
reduzidos é *entrada* da conta. Quem confere é a varredura de todos os quadrados
sem redução nenhuma, que só cabe até a ordem 5, e foi para isso que ela ficou no
repositório mesmo sendo inútil dali em diante.

**Supus que preencher mais células atrapalhasse mais.** Um retângulo latino de 4
linhas num quadrado de ordem 5 tem 20 células escritas e completa sempre; quatro
células soltas num quadrado de ordem 4 travam 13.824 vezes. A forma do que já está
escrito decide, não a quantidade.

## Os arquivos

| arquivo | o que faz |
|---|---|
| `src/Latinos/Quadrado.cs` | a grade, a transposta, a forma reduzida e o quadrado cíclico |
| `src/Latinos/Juiz.cs` | a definição: linha e coluna sem repetição. E ortogonalidade e transversal pela definição |
| `src/Latinos/Contagem.cs` | a varredura de todos, a dos reduzidos e a conta `n!(n-1)!` |
| `src/Latinos/Transversais.cs` | as diagonais tortas, a decomposição e o par ortogonal construído a partir dela |
| `src/Latinos/Parcial.cs` | completar o que está pela metade, e a varredura que acha o que trava |
| `src/Latinos/Medidas.cs` | as cinco tabelas deste README |
| `ferramentas/Medidor` | imprime as tabelas, uma por comando |

## Rodando

```bash
dotnet test testes/Latinos.Testes/Latinos.Testes.csproj -c Release

dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- contagem
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- sete
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- retangulos
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- transversais
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- evans
dotnet run --project ferramentas/Medidor/Medidor.csproj -c Release -- ortogonais
```

C# 12 e .NET 8. Zero dependências fora do xUnit, e só nos testes.

113 testes. Os números deste README estão presos em `MedidasTestes`.
