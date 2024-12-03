# VLAN Segregation Testing Tool

This tool is designed to test VLAN segregation by attempting to ping the gateway IP addresses within different VLANs. If the tool can successfully ping a gateway IP, it indicates a lack of segregation between the VLANs.

## Features

- Pings gateway IP addresses to test for VLAN segregation.
- Outputs results in a clear tabular format, showing successful pings.
- Provides verbose output during the ping process for real-time feedback.

## Requirements

- .NET Core SDK
