# What it is

A simulation of a page table to visualize the process of mapping virtual addresses to physical ones.

**Disclaimer**: This is just a small project that was written in a very short amount of time.

# How to run

You need to have dotnet installed. If you are using macOS and use the package manager [Homebrew](https://brew.sh), do the following:

```sh
# Optional
brew update

brew install dotnet
```

After that, just switch to the project root and run the following commands:

```sh
# If you are not already in this project's root folder. The prefix might differ depending on where you are.
cd Paging/

dotnet run Program.cs
```

# Limitations

- Virtual addresses can only be provided in decimal. An implementation for hexadecimal is possible but wasn't done due to time constraints.
- The page table is implemented for 32bit systems and therefore only works for 32bit addresses. An implementation for 64bit does not make sense however, because even with the 32bit implementation, I had to artificially cap the amount of index bits to 20 or else you would run out of memory.
- There is no implementation of 2 level paging due to time constraints.
