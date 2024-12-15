# UltimateOrb.nanoFramework

Welcome to the UltimateOrb.nanoFramework project! This repository contains various utility libraries for the .NET nanoFramework.

## Table of Contents
- [UltimateOrb.nanoFramework.System.IO.BinaryReader](#ultimateorbnanoframeworksystemiobinaryreader)
- [UltimateOrb.nanoFramework.System.IO.StreamExtensions](#ultimateorbnanoframeworksystemiostreamextensions)
- [UltimateOrb.nanoFramework.System.SpanChar](#ultimateorbnanoframeworksystemspanchar)
- [UltimateOrb.nanoFramework.System.IO.Compression.DeflateStream.DecompressOnly](#ultimateorbnanoframeworksystemiocompressiondeflatestreamdecompressonly)
- [Getting Started](#getting-started)
- [Building and Running](#building-and-running)
- [Contributing](#contributing)
- [License](#license)
- [Third-Party Notices](#third-party-notices)

## UltimateOrb.nanoFramework.System.IO.BinaryReader
This library provides `System.IO.BinaryReader` for the .NET nanoFramework.

## UltimateOrb.nanoFramework.System.IO.StreamExtensions
This library provides extension methods for stream manipulation. It adds convenience methods that simplify working with streams in the .NET nanoFramework.

### Features:
- `ReadAtLeast` and `ReadExactly` stream extension methods

## UltimateOrb.nanoFramework.System.SpanChar
This library provides `System.SpanChar`. `System.SpanChar` is similar to `System.SpanByte` in the .NET nanoFramework.

## UltimateOrb.nanoFramework.System.IO.Compression.DeflateStream.DecompressOnly
This library provides `System.IO.Compression.DeflateStream`. Only decompression is supported.

## Getting Started
To get started with any of these libraries, simply add the relevant NuGet package to your project. You can find the packages on [NuGet.org](https://www.nuget.org/).

### Prerequisites:
- .NET nanoFramework SDK

## Building and Running
To build the project, run `dotnet build` or open the solution file `UltimateOrb.nanoFramework.sln` in your preferred IDE and build the solution.

### Creating NuGet Packages
To create NuGet packages, `cd` to the project directory and use the `nuget pack` command to create the NuGet package. You can download the NuGet executable from [NuGet.org](https://www.nuget.org/).

## Contributing
We welcome contributions! If you have ideas for improvements or have found a bug, feel free to open an issue or submit a pull request.

## License
This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## Third-Party Notices
This project uses source code from the .NET Foundation and third parties. Below are the notices for third-party components used within this project:

- **.NET Foundation and contributors**: This project includes code from the .NET Runtime and the .NET nanoFramework. Please refer to their [web page](https://dotnetfoundation.org/about) and [license](https://github.com/nanoframework/Home/blob/main/LICENSE.md) for more details.
- **zlib** and ports: The UltimateOrb.nanoFramework.System.IO.Compression.DeflateStream libraries adopt code from the zlib C library and its Java/C# ports. All of these portions are covered by the MIT license or compatible licenses.
  - https://github.com/madler/zlib/blob/develop/LICENSE
  - http://www.componentace.com/data/ZLIB_.NET/license.txt
  - https://github.com/zyborg/zlib.net/blob/master/LICENSE
  - https://github.com/ymnk/jzlib/blob/master/LICENSE.txt
- TODO: Any other third-party libraries or components should be listed here with appropriate links to their licenses and acknowledgments.
