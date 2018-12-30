# Solr SPF - Solr Query Debugging/Explanation

This Repository contains the full source for for [Solr SPF](https://www.solrspf.com/), a tool built to help Solr developers better understand Solr debug explain files and tune their Solr queries.

For more information about this tool, or to use it now for free, visit [www.solrspf.com](https://www.solrspf.com/)

## Upcoming changes
* Upgrade from .NET Core 2.0 to .NET Core 2.2
* Prebuilt release/installation package
* Tuning suggestion tools

## Installation / Usage
**Note -** SolrSPF is currently free to use at [www.solrspf.com](https://www.solrspf.com/). Installation is only necessary if you would like to host SolrSPF on your own infrastructure.

### Requirements
* .NET Core 2.0 SDK *(or newer)*
* .NET Core 2.0 Runtime *(or newer)*
* Visual Studio 2017
* Microsoft SQL Server

### Application Setup
1. Pull the latest code from the master branch of this repository
1. Build and publish the application to your desired web server
    * Web server must have the [.Net Core Runtime](https://dotnet.microsoft.com/download/dotnet-core/2.2) installed 
1. Create a new SQL server database with credentials that have read and write access
1. Update application connection strings for your new database
1. Success