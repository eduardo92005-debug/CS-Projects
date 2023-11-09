using System;
using System.Collections.Generic;
using System.Text.Json;

class GanhoDeCapital
{
    static void Main()
    {
        List<string> inputLines = new List<string>();
        
        while (true)
        {
            string line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;

            inputLines.Add(line);
        }

        List<string> outputLines = ProcessarOperacoes(inputLines);

        foreach (string outputLine in outputLines)
        {
            Console.WriteLine(outputLine);
        }
    }

    static List<string> ProcessarOperacoes(List<string> inputLines)
    {
        List<string> outputLines = new List<string>();

        foreach (string inputLine in inputLines)
        {
            List<Operacao> operacoes = JsonSerializer.Deserialize<List<Operacao>>(inputLine);

            // Implementar a lógica de cálculo do imposto para cada operação
            List<decimal> impostos = CalcularImpostos(operacoes);

            // Converter os impostos para o formato JSON e adicionar à lista de saída
            string outputLine = JsonSerializer.Serialize(impostos);
            outputLines.Add(outputLine);
        }

        return outputLines;
    }

    class Operacao
    {
        public string Operation { get; set; }
        public decimal UnitCost { get; set; }
        public int Quantity { get; set; }
    }

    static List<decimal> CalcularImpostos(List<Operacao> operacoes)
    {
        List<decimal> impostos = new List<decimal>();
        decimal precoMedioPonderado = 0;
        decimal prejuizoPassado = 0;

        foreach (Operacao operacao in operacoes)
        {
            if (operacao.Operation == "buy")
            {
                // Lógica para operação de compra
                decimal valorCompra = operacao.UnitCost * operacao.Quantity;
                precoMedioPonderado = ((precoMedioPonderado * (operacoes.Count - 1)) + valorCompra) / operacoes.Count;
            }
            else if (operacao.Operation == "sell")
            {
                // Lógica para operação de venda
                decimal valorVenda = operacao.UnitCost * operacao.Quantity;
                decimal lucroOuPrejuizo = valorVenda - (operacao.Quantity * precoMedioPonderado);

                if (lucroOuPrejuizo > 0)
                {
                    decimal imposto = lucroOuPrejuizo * 0.2m;
                    impostos.Add(imposto);
                }
                else
                {
                    // Deduzir prejuízo de lucros futuros
                    prejuizoPassado += lucroOuPrejuizo;
                    impostos.Add(0);
                }
            }
        }

        return impostos;
    }
}
