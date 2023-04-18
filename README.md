# Console Application to run Liquid

The end goal of this project is to be used in WSL2 or a Linux Machine (Instructions are for Ubuntu/Debian)

## Prerequesites

You need to install .NET6, CloudLiquid and TransformData.ContentFactory

## Run

First install the .NET deb package

```bash
dotnet tool install --global dotnet-deb
dotnet deb install
```

Afterwards,

```bash
dotnet build
dotnet deb
sudo apt install ./bin/Debug/net6.0/LiquidConsole.1.0.0.deb
```

This will output a deb file that you will install on your linux machine, now to run is quite simple

```bash
LiquidConsole ./tests/data/input.json ./tests/data/example.liquid ./tests/data/output.json
```

Now you can use this program anywhere on your linux machine.