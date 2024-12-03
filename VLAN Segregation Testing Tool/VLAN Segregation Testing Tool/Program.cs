using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {


        Console.WriteLine("#######################################-MJ-#############################################");
        Console.WriteLine("--------------------------------------------------------------------------------------\n");
        Console.WriteLine("VLAN Segregation Testing Tool");
        Console.WriteLine("This tool is designed to test VLAN segregation by attempting to ping the gateway IP addresses within different VLANs.");
        Console.WriteLine("If the tool can successfully ping a gateway IP, it indicates a lack of segregation between the VLANs.");
        Console.WriteLine("Please ensure your IP list file is structured correctly before proceeding.\n");

        // Describe the expected file content and structure
        Console.WriteLine("The IP list file should be a CSV file with the following structure:");
        Console.WriteLine("Each line should contain a VLAN name and an IP address, separated by a comma.");
        Console.WriteLine("Example:");
        Console.WriteLine("valn1,10.10.5.1");
        Console.WriteLine("valn2,10.10.6.2\n");
        Console.WriteLine("To Test The Internet connection add 'Google DNS,8.8.8.8' to your file");
        Console.WriteLine("--------------------------------------------------------------------------------------\n");
        Console.WriteLine("########################################################################################");


        Console.Write("Please enter the path to the IP list file: ");
        string ipListPath = Console.ReadLine();


        if (!File.Exists(ipListPath))
        {
            Console.WriteLine("The file does not exist. Please check the path and try again.");
            return;
        }


        string[] lines = File.ReadAllLines(ipListPath);


        ConcurrentBag<(string VlanName, string IpAddress, IPStatus Status)> pingResults = new ConcurrentBag<(string, string, IPStatus)>();


        Parallel.ForEach(lines.Skip(1), line =>
        {
            var parts = line.Split(',');
            if (parts.Length == 2)
            {
                string vlanName = parts[0];
                string ipAddress = parts[1];


                Console.WriteLine($"Pinging {vlanName} ({ipAddress})...");

                PingReply reply = Ping(ipAddress);
                if (reply != null)
                {
                    pingResults.Add((vlanName, ipAddress, reply.Status));
                }
            }
        });


        var successfulPings = pingResults.Where(r => r.Status == IPStatus.Success).ToList();


        Console.WriteLine("\nSuccessful Pings:");
        Console.WriteLine("----------------------------");
        Console.WriteLine("| VLAN Name | IP Address   |");
        Console.WriteLine("----------------------------");
        foreach (var result in successfulPings)
        {
            Console.WriteLine($"| {result.VlanName.PadRight(9)} | {result.IpAddress.PadRight(11)} |");
        }
        Console.WriteLine("----------------------------");

        Console.Beep(400, 5000);
    }

    static PingReply Ping(string ipAddress)
    {
        using (Ping pingSender = new Ping())
        {
            PingOptions options = new PingOptions();
            options.DontFragment = true;

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            try
            {
                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);
                return reply;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error pinging {ipAddress}: {ex.Message}");
                return null;
            }
        }
    }
}