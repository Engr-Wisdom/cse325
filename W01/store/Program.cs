using Newtonsoft.Json;
using System.Text;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesFiles = FindFiles(storesDirectory);

var report = CreateSalesSummary(salesFiles);

var reportDirectory = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(reportDirectory);

var reportPath = Path.Combine(reportDirectory, "sales-summary.txt");

File.WriteAllText(reportPath, report);

Console.WriteLine($"Sales summary created: {reportPath}");


IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(
        folderName,
        "sales.json",
        SearchOption.AllDirectories
    );

    foreach (var file in foundFiles)
    {
        salesFiles.Add(file);
    }

    return salesFiles;
}


string CreateSalesSummary(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    StringBuilder report = new StringBuilder();

    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);

        SalesData? data =
            JsonConvert.DeserializeObject<SalesData>(salesJson);

        double fileTotal = data?.Total ?? 0;

        salesTotal += fileTotal;

        report.AppendLine(
            $"{Path.GetRelativePath(currentDirectory, file)}: {fileTotal:C}"
        );
    }

    report.AppendLine();
    report.AppendLine($"Total Sales: {salesTotal:C}");

    return report.ToString();
}


record SalesData(double Total);