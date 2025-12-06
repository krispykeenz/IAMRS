using System.Net.Http.Json;
using System.Text.Json;

var apiUrl = args.Length > 0 ? args[0] : "http://localhost:5000";
var machineCount = args.Length > 1 && int.TryParse(args[1], out var m) ? m : 5;
var intervalMs = args.Length > 2 && int.TryParse(args[2], out var i) ? i : 2000;

Console.WriteLine($"IAMRS Telemetry Simulator");
Console.WriteLine($"API: {apiUrl}");
Console.WriteLine($"Machines: {machineCount}");
Console.WriteLine($"Interval: {intervalMs}ms");
Console.WriteLine("Press Ctrl+C to stop...");
Console.WriteLine();

var machines = Enumerable.Range(1, machineCount).Select(i => $"SIM-{i:D3}").ToList();
var random = new Random();
using var http = new HttpClient { BaseAddress = new Uri(apiUrl) };

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

// Seed machines first
foreach (var code in machines)
{
    var createDto = new
    {
        machineCode = code,
        name = $"Simulated Machine {code}",
        type = random.Next(0, 10),
        location = $"Simulation Bay {random.Next(1, 5)}"
    };
    try
    {
        var resp = await http.PostAsJsonAsync("/api/machines", createDto, cts.Token);
        if (resp.IsSuccessStatusCode)
            Console.WriteLine($"[SEED] Created machine {code}");
        else if ((int)resp.StatusCode == 409 || (int)resp.StatusCode == 400)
            Console.WriteLine($"[SEED] Machine {code} may already exist");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[SEED] Failed to create {code}: {ex.Message}");
    }
}

Console.WriteLine();
Console.WriteLine("Starting telemetry loop...");

while (!cts.Token.IsCancellationRequested)
{
    foreach (var code in machines)
    {
        // Simulate realistic telemetry with occasional spikes
        var baseTemp = 65.0 + random.NextDouble() * 10;
        var spike = random.NextDouble() < 0.05; // 5% chance of spike
        var temperature = spike ? baseTemp + 20 + random.NextDouble() * 15 : baseTemp;
        var vibration = 1.0 + random.NextDouble() * 3 + (spike ? 5 : 0);
        var pressure = 5.0 + random.NextDouble() * 2;

        var telemetry = new
        {
            machineId = code,
            temperature,
            vibration,
            pressure,
            timestamp = DateTime.UtcNow
        };

        try
        {
            var resp = await http.PostAsJsonAsync("/api/telemetry", telemetry, cts.Token);
            var status = resp.IsSuccessStatusCode ? "OK" : $"ERR:{(int)resp.StatusCode}";
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {code} T={temperature:F1}°C V={vibration:F2} P={pressure:F1} -> {status}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {code} -> FAIL: {ex.Message}");
        }
    }

    try { await Task.Delay(intervalMs, cts.Token); } catch { break; }
}

Console.WriteLine("Simulator stopped.");
