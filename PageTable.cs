namespace Paging;

public class PageTable
{
    public uint this[int index] => Addresses[index];
    public required uint[] Addresses { get; init; }
    
    public int EntryCount { get; private init; }
    private int IndexBitCount { get; init; }
    private int OffsetBitCount { get; init; }

    private PageTable()
    {}

    public static PageTable CreateByOffset(int indexBitCount, int addressOffset)
    {
        if (indexBitCount is < 1 or > 31)
            throw new ArgumentOutOfRangeException(nameof(indexBitCount));
        
        var entryCount = (int)Math.Pow(2, indexBitCount);
        if (addressOffset < 0 || entryCount <= addressOffset)
            throw new ArgumentOutOfRangeException(nameof(addressOffset));
        
        var offsetBitCount = 32 - indexBitCount;

        return new PageTable
        {
            IndexBitCount = indexBitCount,
            OffsetBitCount = offsetBitCount,
            EntryCount = entryCount,
            Addresses = CreateAddressesByAddressOffset(entryCount, offsetBitCount, addressOffset)
        };
    }

    private static uint[] CreateAddressesByAddressOffset(int entryCount, int offsetBitCount, int offset)
    {
        var pageTable = new uint[entryCount];
        
        var counter = 0;
        for (var i = offset; i < entryCount; i++)
        {
            pageTable[i] = (uint)counter << offsetBitCount;
            counter += 1;
        }
        for (var i = 0; i < offset; i++)
        {
            pageTable[i] = (uint)counter << offsetBitCount;
            counter += 1;
        }

        return pageTable;
    }

    public (uint physAddress, List<string> logs) TranslateToPhysical(uint virtualAddress)
    {
        List<string> logs = [];
        
        var index = virtualAddress >> OffsetBitCount;
        logs.Add($"Index of virtual address: 0x{virtualAddress:X8} >> {OffsetBitCount} = 0x{index:X8} ({index})");
        
        var offset = virtualAddress & (uint.MaxValue >> IndexBitCount);
        logs.Add($"Offset of virtual address: 0x{virtualAddress:X8} & (0x{uint.MaxValue:X8} >> {IndexBitCount}) = 0x{offset:X8}");

        var physAddress = Addresses[index] | offset;
        logs.Add($"Physical address: addresses[index] | offset = 0x{Addresses[index]:X8} | 0x{offset:X8} = 0x{physAddress:X8}");

        return (physAddress, logs);
    }
}