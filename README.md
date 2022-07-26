# alps.net.api

This C# library provides the functionality to create and modify in-memory PASS process models.
These models might either be imported from an ontology (given in the owl/rdf format) or created from scratch.

The library currently supports the [Standard PASS](https://github.com/I2PM/Standard-PASS-Ontology) as well as the Abstract Layered PASS created by Matthes Elstermann.
For more information have a look at the [wiki pages](https://github.com/I2PM/alps.net.api/wiki) or at the HTML/XML doc inside the classes (reason why the project mostly consists of html code).

A library with the name of this repository is also published in the NuGet store and available for download.
Currently the library is targeting .NET Core 3.1 (netcoreapp3.1) as well as .NET Framework 4.8 (net48)

## Supported Functionality
- Subject oriented in-memory modeling in C#
- Import models from OWL/RDF formatted files
  - Dynamic import supporting own OntologyClass definitions
  - Dynamic import supporting own C# class extensions
- Model modification by editing underlying triple store graph
- Export models to OWL/RDF formatted files

## Desired future functionality
- Support for running ontology reasoners at runtime
- Support for using remote triple store graphs as model data backend
