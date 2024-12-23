# VLAN Segregation Testing Tool

This tool is designed to test VLAN segregation by attempting to ping the gateway IP addresses within different VLANs. If the tool can successfully ping a gateway IP, it indicates a lack of segregation between the VLANs.

## Features

- Ping gateway IP addresses to test the VLAN segregation.
- Outputs results in a clear table format, showing successful ping.
- Provid verbose output during the ping process for real-time feedback.

## Requirements

- .NET Core SDK
