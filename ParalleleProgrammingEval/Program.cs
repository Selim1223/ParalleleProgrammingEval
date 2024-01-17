using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Console.WriteLine("Calcul de la somme en parallèle...");
        Task<int> sumTask = Task.Run(() => CalculateSum(Enumerable.Range(1, 3000)));

        Console.WriteLine("Traitement des fichiers en parallèle...");

        // Tâches pour le fichier Eval_file1
        Task<int> wordCountFile1Task = CountWordsAsync("Eval_file1.txt");
        Task<int> loremIpsumCountFile1Task = CountOccurrencesAsync("Eval_file1.txt", "Lorem ipsum");

        // Tâches pour le fichier Eval_file2
        Task<int> wordCountFile2Task = CountWordsAsync("Eval_file2.txt");
        Task<int> loremIpsumCountFile2Task = CountOccurrencesAsync("Eval_file2.txt", "Lorem ipsum");

        await Task.WhenAll(sumTask, wordCountFile1Task, loremIpsumCountFile1Task, wordCountFile2Task, loremIpsumCountFile2Task);

        stopwatch.Stop();

        Console.WriteLine($"Résultat du calcul de la somme : {sumTask.Result}");
        Console.WriteLine($"Nombre de mots dans le fichier Eval_file1 : {wordCountFile1Task.Result}");
        Console.WriteLine($"Occurrences de 'Lorem ipsum' dans le fichier Eval_file1 : {loremIpsumCountFile1Task.Result}");
        Console.WriteLine($"Nombre de mots dans le fichier Eval_file2 : {wordCountFile2Task.Result}");
        Console.WriteLine($"Occurrences de 'Lorem ipsum' dans le fichier Eval_file2 : {loremIpsumCountFile2Task.Result}");

        int finalSum = sumTask.Result + wordCountFile1Task.Result + loremIpsumCountFile1Task.Result +
                        wordCountFile2Task.Result + loremIpsumCountFile2Task.Result;
        Console.WriteLine($"Somme finale des résultats : {finalSum}");

        Console.WriteLine($"Temps de traitement total : {stopwatch.ElapsedMilliseconds} ms");
    }

    static int CalculateSum(IEnumerable<int> numbers)
    {
        return numbers.Sum();
    }

    static async Task<int> CountWordsAsync(string filePath)
    {
        Console.WriteLine($"Traitement du fichier {filePath} - Comptage des mots...");
        string content = await File.ReadAllTextAsync(filePath);
        return content.Split(' ','\n').Length;
    }

    static async Task<int> CountOccurrencesAsync(string filePath, string target)
    {
        Console.WriteLine($"Traitement du fichier {filePath} - Recherche des occurrences de '{target}'...");
        string content = await File.ReadAllTextAsync(filePath);
        return content.Split(new[] { target }, StringSplitOptions.None).Length - 1;
    }
}
