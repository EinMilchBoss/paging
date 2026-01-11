using System.Text;

namespace Paging;

public static class Program
{
    public static void Main(string[] args)
    {
        var ptIndexBitCount = ReadPageTableIndexBits();
        var ptAddressOffset = ReadPageTableAddressOffset(ptIndexBitCount);

        var pt = PageTable.CreateByOffset(ptIndexBitCount, ptAddressOffset);
        
        var shouldPrintPt = UserInput.GetConfirmation($"Do you want to print the page table ({pt.EntryCount} entries)?");
        if (shouldPrintPt)
            Console.WriteLine(AddressInformation(pt));

        var virtAddress = UserInput.GetInput<uint>("What virtual address should be converted?");
        var (physAddress, logs) = pt.TranslateToPhysical(virtAddress);
        Console.WriteLine(string.Join('\n', logs));
        Console.WriteLine($"Physical address: 0x{physAddress:X8} ({physAddress}).");
    }

    private static int ReadPageTableIndexBits()
    {
        // If we allow more then a certain amount of bits for indices, we would run out of memory.
        // I artificially cap it to the following amount.
        const int maxSize = 20;
        
        while (true)
        {
            var pageIndexBits = UserInput.GetInput<int>("Provide the amount of bits reserved for the page index.");
            if (pageIndexBits is >= 1 and <= maxSize) return pageIndexBits;
            
            Console.Error.WriteLine("Input has to be in range of 1 to 20. Try again.");
        }
    }

    private static int ReadPageTableAddressOffset(int indexBitCount)
    {
        var entryCount = (int)Math.Pow(2, indexBitCount);
        while (true)
        {
            var ptOffset = UserInput.GetInput<int>("What offset should the page table addresses have?");
            if (0 <= ptOffset && ptOffset < entryCount) return ptOffset;

            Console.Error.WriteLine($"Input has to be in range of 0 to {entryCount - 1}. Try again.");
        }
    }

    private static string AddressInformation(PageTable pt)
    {
        StringBuilder sb = new();
        for (var i = 0; i < pt.EntryCount; i++) 
            sb.AppendLine($"Index {i,8}: 0x{pt[i]:X8}.");
        return sb.ToString();
    }
}