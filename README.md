# Lefkogeia REST API test server

Lefkogeia is a server to test your REST APIs with. When it runs, it accepts every request made on the configured IP/port and returns an HTTP 200 "Thank you for your {method name, GET/POST etc}".

It logs all requests in a directory imaginatively called ```logs```. It creates an access.txt file where all requests are written, and one file per request (000001.txt, 000002.txt etc) in which the request's payload (e.g. an xml or a json) is written.

## Usage

The primary use of Lefkogeia is to test/debug/troubleshoot REST API and web services clients. You run it (see release notes on that) and get your client to call it. It will log whatever was sent, allowing you to troubleshoot whatever problem you might have.

## Configuration

To configure Lefkogeia, edit the appsettings.json file with a test editor.

An example appsettings.json file to serve multiple addresses & ports would be:
```json
{
	"Logging": {
		"LogLevel": {
			"Default": "Debug",
			"System": "Information",
			"Microsoft": "Information"
		}
	},
	"Host": {
		"Url": [
			"http://localhost:6800",
			"http://server1:7777",
			"http://147.102.43.3:4545",
		]
	}
}
```

Paths are not yet supported in URLs, so if you change http://server1:7777 to http://server1:7777/testapi you will get an error. This is planned for the next release.

Also note that in order to use https:// you need to generate a certificate by running 
```Batchfile
dotnet dev-certs https --trust
```
For more info see https://go.microsoft.com/fwlink/?linkid=848054.

## But why "Lefkogeia", what does this even mean?

Because it's such a beautiful place! Lefkogeia is a small village in southern Crete, Greece, with amazing beaches like Ammoudi, Shinaria, Klisidi and more. You can read more in [Tripadvisor](https://www.tripadvisor.com/Tourism-g1190439-Lefkogia_Rethymnon_Prefecture_Crete-Vacations.html).
